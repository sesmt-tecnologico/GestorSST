using GISCore.Business.Abstract;
using GISModel.DTO.Conta;
using GISModel.DTO.Permissoes;
using GISModel.Entidades;
using GISModel.Enums;
using Ninject;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace GISCore.Business.Concrete
{
    public class UsuarioBusiness : BaseBusiness<Usuario>, IUsuarioBusiness
    {

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public IUsuarioPerfilBusiness UsuarioPerfilBusiness { get; set; }

        [Inject]
        public IPerfilBusiness PerfilBusiness { get; set; }

        [Inject]
        public IDepartamentoBusiness DepartamentoBusiness { get; set; }

        public AutenticacaoModel ValidarCredenciais(AutenticacaoModel autenticacaoModel)
        {
            autenticacaoModel.Login = autenticacaoModel.Login.Trim().ToUpper().Replace(".", "").Replace("-", "").Replace("/", "");

            //Buscar usuário sem validar senha, para poder determinar se a validação da senha será com AD ou com a senha interna do GIS
            List<Usuario> lUsuarios = Consulta.Where(u => string.IsNullOrEmpty(u.UsuarioExclusao) &&
                                                     (u.Login.Replace(".", "").Replace("-", "").Replace("/", "").Equals(autenticacaoModel.Login) || u.Email.Equals(autenticacaoModel.Login))).ToList();

            if (lUsuarios.Count == 0)
            {
                //throw new Exception("Não foi possível identificar o seu cadastro.");


                //Este código é específico para a CEMIG, logo, se eu encontrar o login na rede da cemig, eu assumo que é integração com AD e não pelo sistema

                CEMIGInfoService.InfoServiceClient wfInfo = new CEMIGInfoService.InfoServiceClient();
                CEMIGInfoService.Empregado emp = wfInfo.RecuperarDadosEmpregado(new CEMIGInfoService.DadosEmpregado()
                {
                    ChaveAcesso = autenticacaoModel.Login,
                    IgnorarDesligamento = true
                });

                if (emp == null)
                {
                    throw new Exception("Não foi possível identificar o cadastro '" + autenticacaoModel.Login + "'.");
                }
                else
                {
                    Empresa empresa = EmpresaBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.NomeFantasia.Equals(emp.Empresa.Nome));
                    if (empresa == null)
                    {
                        throw new Exception("A empresa vinculada a chave '" + autenticacaoModel.Login + "' não está cadastrada. Acione o Administrador do sistema para a regularização.");
                    }

                    Empresa empBase = EmpresaBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.NomeFantasia.Equals("CEMIG"));
                    if (empBase == null)
                    {
                        empBase = empresa;
                    }

                    Departamento dep = DepartamentoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Sigla.Equals(emp.Orgao.Sigla));
                    if (dep == null)
                    {
                        //Criar órgão

                        //Buscar Hierarquia dos órgãos
                        List<CEMIGInfoService.Orgao> estrutura = DefinirHierarquiaOrganizacional(emp.Orgao.UA, wfInfo);
                        if (estrutura?.Count == 0)
                        {
                            throw new Exception("Não foi localizado o órgão da chave '" + autenticacaoModel.Login + "'. Acione o Administrador do sistema para a regularização.");
                        }
                        else
                        {
                            Departamento depAnterior = null;
                            int i = 0;
                            List<NivelHierarquico> niveis = DepartamentoBusiness.BuscarNiveis();
                            for (i = estrutura.Count - 1; i >= 0; i--)
                            {

                                string siglaFind = estrutura[i].Sigla;

                                Guid UKNivel = Guid.NewGuid();

                                if (estrutura[i].NivelHierarquico.ToUpper().Equals("DIR"))
                                {
                                    NivelHierarquico nivel = niveis.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Nome.Equals("Diretoria"));
                                    UKNivel = nivel.UniqueKey;
                                }
                                else if (estrutura[i].NivelHierarquico.ToUpper().Equals("SUP"))
                                {
                                    NivelHierarquico nivel = niveis.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Nome.Equals("Superintendência"));
                                    UKNivel = nivel.UniqueKey;
                                }
                                else if (estrutura[i].NivelHierarquico.ToUpper().Equals("GER"))
                                {
                                    NivelHierarquico nivel = niveis.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Nome.Equals("Gerência"));
                                    UKNivel = nivel.UniqueKey;
                                }

                                Departamento depFind = DepartamentoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Sigla.Equals(siglaFind));
                                if (depFind == null)
                                {

                                    Guid? UKDepVinculado = null;
                                    if (depAnterior != null)
                                        UKDepVinculado = depAnterior.UniqueKey;

                                    depFind = new Departamento()
                                    {
                                        Sigla = estrutura[i].Sigla,
                                        Codigo = estrutura[i].UA,
                                        Descricao = estrutura[i].NomeCompleto,
                                        UniqueKey = Guid.NewGuid(),
                                        UKDepartamentoVinculado = UKDepVinculado,
                                        UKEmpresa = empBase.UniqueKey,
                                        UKNivelHierarquico = UKNivel,
                                        UsuarioInclusao = autenticacaoModel.Login
                                    };

                                    DepartamentoBusiness.Inserir(depFind);
                                    depAnterior = depFind;

                                }
                                else
                                {
                                    depAnterior = depFind;
                                }

                                if (estrutura[i].Sigla.Equals(emp.Orgao.Sigla))
                                {
                                    dep = depFind;
                                }
                            }

                        }
                    }

                    Usuario objU = new Usuario()
                    {
                        Login = emp.Matricula,
                        Nome = emp.Nome,
                        Email = emp.Email,
                        TipoDeAcesso = TipoDeAcesso.AD,
                        UKDepartamento = dep.UniqueKey,
                        UKEmpresa = empresa.UniqueKey,
                        UsuarioInclusao = autenticacaoModel.Login
                    };

                    Inserir(objU);

                    List<VMPermissao> vmPer = new List<VMPermissao>();

                    Perfil perfil = PerfilBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Nome.Equals("Básico"));
                    if (perfil != null)
                    {
                        UsuarioPerfilBusiness.Inserir(new UsuarioPerfil()
                        {
                            UKConfig = dep.UniqueKey,
                            UKPerfil = perfil.UniqueKey,
                            UKUsuario = objU.UniqueKey,
                            UsuarioInclusao = autenticacaoModel.Login
                        });

                        vmPer.Add(new VMPermissao()
                        {
                            UKPerfil = perfil.UniqueKey,
                            Perfil = perfil.Nome,
                            UKConfig = dep.UniqueKey,
                            Config = dep.Sigla
                        });
                    }

                    return new AutenticacaoModel()
                    {
                        UniqueKey = objU.UniqueKey,
                        Login = objU.Login,
                        Nome = objU.Nome,
                        Email = objU.Email,
                        TipoDeAcesso = objU.TipoDeAcesso,
                        Permissoes = vmPer
                    };

                }

            }
            else if (lUsuarios.Count > 1)
            {
                throw new Exception("Não foi possível identificar o seu cadastro.");
            }
            else
            {
                if (lUsuarios[0].TipoDeAcesso == TipoDeAcesso.AD)
                {
                    //Login, validando a senha no AD
                    Guid UKEmpresa = lUsuarios[0].UKEmpresa;
                    Empresa emp = EmpresaBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(UKEmpresa));
                    if (emp == null)
                    {
                        throw new Exception("Nâo foi possível encontrar a empresa vinculada ao perfil do usuário.");
                    }
                    else
                    {
                        if (Convert.ToBoolean(ConfigurationManager.AppSettings["AD:DMZ"]))
                        {
                            //Chamar web service para validar a senha no AD
                            WSAutenticacao.CartaoCorporativo ws = new WSAutenticacao.CartaoCorporativo();
                            //ws.Url = emp.URL_WS;

                            string rs = ws.LoginAD(autenticacaoModel.Login, autenticacaoModel.Senha);
                            if (rs.Equals("\"-1\""))
                            {
                                throw new Exception("Login ou senha incorreto.");
                            }
                            else
                            {
                                Guid IDUsuario = lUsuarios[0].UniqueKey;

                                List<VMPermissao> listapermissoes = new List<VMPermissao>();

                                listapermissoes.AddRange(from usuarioperfil in UsuarioPerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                         join perfil in PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usuarioperfil.UKPerfil equals perfil.UniqueKey
                                                         join empresa in EmpresaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList() on usuarioperfil.UKConfig equals empresa.UniqueKey
                                                         where usuarioperfil.UKUsuario.Equals(IDUsuario)
                                                         select new VMPermissao { UKPerfil = perfil.UniqueKey, Perfil = perfil.Nome, UKConfig = empresa.UniqueKey, Config = empresa.NomeFantasia });

                                listapermissoes.AddRange(from usuarioperfil in UsuarioPerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                         join perfil in PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usuarioperfil.UKPerfil equals perfil.UniqueKey
                                                         join dep in DepartamentoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList() on usuarioperfil.UKConfig equals dep.UniqueKey
                                                         where usuarioperfil.UKUsuario.Equals(IDUsuario)
                                                         select new VMPermissao { UKPerfil = perfil.UniqueKey, Perfil = perfil.Nome, UKConfig = dep.UniqueKey, Config = dep.Sigla });

                                if (listapermissoes.Count == 0)
                                {
                                    throw new Exception("O usuário não possui permissão para acessar o sistema. Entre em contato com o Administrador.");
                                }

                                return new AutenticacaoModel() { UniqueKey = lUsuarios[0].UniqueKey, Login = lUsuarios[0].Login, Nome = lUsuarios[0].Nome, Email = lUsuarios[0].Email, TipoDeAcesso = lUsuarios[0].TipoDeAcesso, Permissoes = listapermissoes };
                            }

                        }
                        else
                        {

                            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, emp.URL_AD))
                            {
                                if (pc.ValidateCredentials(autenticacaoModel.Login, autenticacaoModel.Senha))
                                    return null;
                                else
                                    throw new Exception("Login ou senha incorretos.");
                            }
                        }
                    }
                }
                else
                {
                    //Login, validando a senha interna no CIS
                    Guid IDUsuario = lUsuarios[0].UniqueKey;

                    string senhaCifrada = CreateHashFromPassword(autenticacaoModel.Senha);

                    Usuario oUsuario = Consulta.FirstOrDefault(p => p.UniqueKey.Equals(IDUsuario) && p.Senha.Equals(senhaCifrada));
                    if (oUsuario != null)
                    {
                        List<VMPermissao> listapermissoes = new List<VMPermissao>();

                        listapermissoes.AddRange(from usuarioperfil in UsuarioPerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                                 join perfil in PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usuarioperfil.UKPerfil equals perfil.UniqueKey
                                                 where usuarioperfil.UKUsuario.Equals(IDUsuario)
                                                 select new VMPermissao { Perfil = perfil.Nome });

                        //listapermissoes.AddRange(from usuarioperfil in UsuarioPerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                        //                         join perfil in PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usuarioperfil.UKPerfil equals perfil.UniqueKey
                        //                         where usuarioperfil.UKUsuario.Equals(IDUsuario)
                        //                         select new VMPermissao { Perfil = perfil.Nome });

                        if (listapermissoes.Count == 0)
                        {
                            throw new Exception("O usuário não possui permissão para acessar o sistema. Entre em contato com o Administrador.");
                        }

                        return new AutenticacaoModel() { UniqueKey = IDUsuario, Login = oUsuario.Login, Nome = oUsuario.Nome, Email = oUsuario.Email, TipoDeAcesso = lUsuarios[0].TipoDeAcesso, Permissoes = listapermissoes };
                    }
                    else
                    {
                        throw new Exception("Login ou senha incorretos.");
                    }
                }
            }

        }


        private List<CEMIGInfoService.Orgao> DefinirHierarquiaOrganizacional(string Orgao, CEMIGInfoService.InfoServiceClient wfInfo)
        {

            List<CEMIGInfoService.Orgao> estrutura = new List<CEMIGInfoService.Orgao>();

            CEMIGInfoService.Orgao orgaoAnalisado = null;

            string uaParaAnalisar = Orgao;

            while (uaParaAnalisar != "0000")
            {
                orgaoAnalisado = wfInfo.RecuperarDadosOrgao(new CEMIGInfoService.DadosOrgao() { UA = uaParaAnalisar });

                estrutura.Add(orgaoAnalisado);

                uaParaAnalisar = orgaoAnalisado.UASuperior;
            }

            return estrutura;

        }

        public byte[] RecuperarAvatar(string login)
        {
            try
            {
                WCF_Suporte.SuporteClient WCFSuporte = new WCF_Suporte.SuporteClient();

                return WCFSuporte.BuscarFotoPerfil(new WCF_Suporte.DadosUsuario()
                {
                    Login = login
                });

            }
            catch (FaultException<WCF_Suporte.FaultSTARSServices> ex)
            {
                throw new Exception(ex.Detail.Detalhes);
            }
        }

        public void SalvarAvatar(string login, string imageStringBase64, string extensaoArquivo)
        {
            try
            {
                WCF_Suporte.SuporteClient WCFSuporte = new WCF_Suporte.SuporteClient();
                WCFSuporte.SalvarFotoPerfil(new WCF_Suporte.DadosUsuario()
                {
                    Login = login
                }, imageStringBase64);
            }
            catch (FaultException<WCF_Suporte.FaultSTARSServices> ex)
            {
                throw new Exception(ex.Detail.Detalhes);
            }
        }

        public override void Inserir(Usuario usuario)
        {
            if (Consulta.Any(u => u.Login.Equals(usuario.Login)))
                throw new InvalidOperationException("Não é possível inserir usuário com o mesmo login.");

            string senha = usuario.Senha;

            usuario.Senha = CreateHashFromPassword(usuario.Senha);

            base.Inserir(usuario);

            //Enviar Email
            var client = new SendGridClient(ConfigurationManager.AppSettings["SendGridAPIKey"]);
            var from = new EmailAddress("antonio.hpereira@icloud.com");
            var subject = "Seja bem-vindo ao GESTOR";

            var to = new EmailAddress(usuario.Email);
            
            string sHTML = "<p>Olá " + usuario.Nome + "! Você foi cadastrado no sistema GESTOR.</p>" +
                           "<p>Segue abaixo os seus dados de acesso.</p> <br />" +
                           "<p>Login: <strong>" + usuario.Login + "</strong></p>" +
                           "<p>Senha: <strong>" + senha + "</strong></p> <br />" +
                           "<p>Atenciosamente,</p>" +
                           "<p>Equipe GESTOR</p>"; ;

            var msg = MailHelper.CreateSingleEmail(from, to, subject, string.Empty, sHTML);

            var response = client.SendEmailAsync(msg).Result;


        }

        public override void Alterar(Usuario entidade)
        {
            if (Consulta.Any(u => string.IsNullOrEmpty(u.UsuarioExclusao) && !u.UniqueKey.Equals(entidade.UniqueKey) && u.Login.Equals(entidade.Login.Trim())))
                throw new InvalidOperationException("Não é possível atualizar este usuário, pois o Login já está sendo usado por outro usuário.");

            Usuario temp = Consulta.FirstOrDefault(p => p.UniqueKey.Equals(entidade.UniqueKey));
            if (temp == null)
            {
                throw new Exception("Não foi possível encontrar o usuário através da identificação fornecida.");
            }
            else
            {
                temp.UsuarioExclusao = entidade.UsuarioInclusao;
                temp.DataExclusao = DateTime.Now;
                base.Alterar(temp);

                entidade.Senha = temp.Senha;
                entidade.DataInclusao = temp.DataInclusao;
                base.Inserir(entidade);
            }
        }

        public void DefinirSenha(NovaSenhaViewModel novaSenhaViewModel)
        {
            Usuario oUsuario = Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(novaSenhaViewModel.UKUsuario));
            if (oUsuario == null)
            {
                throw new Exception("Não foi possível localizar o usuário através da identificação.");
            }
            else
            {
                oUsuario.UsuarioExclusao = oUsuario.Login;
                Terminar(oUsuario);


                Usuario oUser = new Usuario()
                {
                    CPF = oUsuario.CPF,
                    Email = oUsuario.Email,
                    Login = oUsuario.Login,
                    Nome = oUsuario.Nome,
                    Senha = CreateHashFromPassword(novaSenhaViewModel.ConfirmarNovaSenha),
                    TipoDeAcesso = oUsuario.TipoDeAcesso,
                    UKDepartamento = oUsuario.UKDepartamento,
                    UKEmpresa = oUsuario.UKEmpresa,
                    UniqueKey = oUsuario.UniqueKey
                };
                base.Inserir(oUser);


                EnviarEmailParaUsuarioSenhaAlterada(oUsuario);
            }
        }

        public void SolicitarAcesso(string email)
        {
            List<Usuario> listaUsuarios = Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.Email.ToLower().Equals(email.ToLower())).ToList();
            if (listaUsuarios.Count() > 1 || listaUsuarios.Count() < 1)
            {
                listaUsuarios = Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.Login.ToLower().Equals(email.ToLower())).ToList();
                if (listaUsuarios.Count() > 1 || listaUsuarios.Count() < 1)
                {
                    throw new Exception("Não foi possível localizar este usuário no sistema através do e-mail. Tente novamente ou procure o Administrador.");
                }
            }

            EnviarEmailParaUsuarioSolicacaoAcesso(listaUsuarios[0]);
        }

        #region E-mails

        private void EnviarEmailParaUsuarioSolicacaoAcesso(Usuario usuario)
        {
            string sRemetente = ConfigurationManager.AppSettings["Web:Remetente"];
            string sSMTP = ConfigurationManager.AppSettings["Web:SMTP"];

            MailMessage mail = new MailMessage(sRemetente, usuario.Email);

            string PrimeiroNome = GISHelpers.Utils.Severino.PrimeiraMaiusculaTodasPalavras(usuario.Nome);
            if (PrimeiroNome.Contains(" "))
                PrimeiroNome = PrimeiroNome.Substring(0, PrimeiroNome.IndexOf(" "));

            mail.Subject = PrimeiroNome + ", este é o link para redinir sua senha";
            mail.Body = "<html style=\"font-family: Verdana; font-size: 11pt;\"><body>Olá, " + PrimeiroNome + ".";
            mail.Body += "<br /><br />";
            mail.Body += "<span style=\"color: #222;\">Redefina sua senha para começar novamente.";
            mail.Body += "<br /><br />";

            string sLink = "http://localhost:26717/Conta/DefinirNovaSenha/" + WebUtility.UrlEncode(GISHelpers.Utils.Criptografador.Criptografar(usuario.ID + "#" + DateTime.Now.ToString("yyyyMMdd"), 1)).Replace("%", "_@");

            mail.Body += "Para alterar sua senha do GiS, clique <a href=\"" + sLink + "\">aqui</a> ou cole o seguinte link no seu navegador.";
            mail.Body += "<br /><br />";
            mail.Body += sLink;
            mail.Body += "<br /><br />";
            mail.Body += "O link é válido por 24 horas, portanto, utilize-o imediatamente.";
            mail.Body += "<br /><br />";
            mail.Body += "Obrigado por utilizar o GiS!<br />";
            mail.Body += "<strong>Gestão Inteligente da Segurança</strong>";
            mail.Body += "</span>";
            mail.Body += "<br /><br />";
            mail.Body += "<span style=\"color: #aaa; font-size: 10pt; font-style: italic;\">Mensagem enviada automaticamente, favor não responder este email.</span>";
            mail.Body += "</body></html>";

            mail.IsBodyHtml = true;
            mail.BodyEncoding = Encoding.UTF8;


            SmtpClient smtpClient = new SmtpClient(sSMTP, 587);

            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = ConfigurationManager.AppSettings["Web:Remetente"],
                Password = "sesmtajt"
            };

            smtpClient.EnableSsl = true;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };

            smtpClient.Send(mail);

        }

        private void EnviarEmailParaUsuarioSenhaAlterada(Usuario usuario)
        {
            //string sRemetente = ConfigurationManager.AppSettings["Web:Remetente"];
            //string sSMTP = ConfigurationManager.AppSettings["Web:SMTP"];

            //MailMessage mail = new MailMessage(sRemetente, usuario.Email);

            //string PrimeiroNome = GISHelpers.Utils.Severino.PrimeiraMaiusculaTodasPalavras(usuario.Nome);
            //if (PrimeiroNome.Contains(" "))
            //    PrimeiroNome = PrimeiroNome.Substring(0, PrimeiroNome.IndexOf(" "));

            //mail.Subject = PrimeiroNome + ", sua senha foi redefinida.";

            //mail.Body = "<html style=\"font-family: Verdana; font-size: 11pt;\"><body>Olá, " + PrimeiroNome + ".";
            //mail.Body += "<br /><br />";
            //mail.Body += "<span style=\"color: #222;\">Você redefiniu sua senha do GiS.";
            //mail.Body += "<br /><br />";
            //mail.Body += "Obrigado por utilizar o GiS!<br />";
            //mail.Body += "<strong>Gestão Inteligente da Segurança</strong>";
            //mail.Body += "</span>";
            //mail.Body += "<br /><br />";
            //mail.Body += "<span style=\"color: #aaa; font-size: 10pt; font-style: italic;\">Mensagem enviada automaticamente, favor não responder este email.</span>";
            //mail.Body += "</body></html>";

            //mail.IsBodyHtml = true;
            //mail.BodyEncoding = Encoding.UTF8;


            //SmtpClient smtpClient = new SmtpClient(sSMTP, 587);

            //smtpClient.Credentials = new System.Net.NetworkCredential()
            //{
            //    UserName = ConfigurationManager.AppSettings["Web:Remetente"],
            //    Password = "sesmtajt"
            //};

            //smtpClient.EnableSsl = true;
            //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
            //        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            //        System.Security.Cryptography.X509Certificates.X509Chain chain,
            //        System.Net.Security.SslPolicyErrors sslPolicyErrors)
            //{
            //    return true;
            //};

            //smtpClient.Send(mail);

        }

        private void EnviarEmailParaUsuarioRecemCriadoSistema(Usuario usuario)
        {
            string sRemetente = ConfigurationManager.AppSettings["Web:Remetente"];
            string sSMTP = ConfigurationManager.AppSettings["Web:SMTP"];

            MailMessage mail = new MailMessage(sRemetente, usuario.Email);

            string PrimeiroNome = GISHelpers.Utils.Severino.PrimeiraMaiusculaTodasPalavras(usuario.Nome);
            if (PrimeiroNome.Contains(" "))
                PrimeiroNome = PrimeiroNome.Substring(0, PrimeiroNome.IndexOf(" "));

            mail.Subject = PrimeiroNome + ", seja bem-vindo!";
            mail.Body = "<html style=\"font-family: Verdana; font-size: 11pt;\"><body>Olá, " + PrimeiroNome + ";";
            mail.Body += "<br /><br />";

            string NomeUsuarioInclusao = usuario.UsuarioInclusao;
            Usuario uInclusao = Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.Login.Equals(usuario.UsuarioInclusao));
            if (uInclusao != null && !string.IsNullOrEmpty(uInclusao.Nome))
                NomeUsuarioInclusao = uInclusao.Nome;


            string sLink = "http://localhost:26717/Conta/DefinirNovaSenha/" + WebUtility.UrlEncode(GISHelpers.Utils.Criptografador.Criptografar(usuario.ID + "#" + DateTime.Now.ToString("yyyyMMdd"), 1)).Replace("%", "_@");

            mail.Body += "Você foi cadastrado no sistema GiS - Gestão Inteligente da Segurança pelo " + GISHelpers.Utils.Severino.PrimeiraMaiusculaTodasPalavras(NomeUsuarioInclusao) + ".";
            mail.Body += "<br /><br />";
            mail.Body += "Clique <a href=\"" + sLink + "\">aqui</a> para ativar sua conta ou cole o seguinte link no seu navegador.";
            mail.Body += "<br /><br />";
            mail.Body += sLink;
            mail.Body += "<br /><br />";
            mail.Body += "Obrigado por utilizar o GiS!<br />";
            mail.Body += "<strong>Gestão Inteligente da Segurança</strong>";
            mail.Body += "<br /><br />";
            mail.Body += "<span style=\"color: #ccc; font-style: italic;\">Mensagem enviada automaticamente, favor não responder este email.</span>";
            mail.Body += "</body></html>";

            mail.IsBodyHtml = true;
            mail.BodyEncoding = Encoding.UTF8;

            SmtpClient smtpClient = new SmtpClient(sSMTP, 587);

            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = ConfigurationManager.AppSettings["Web:Remetente"],
                Password = "sesmtajt"
            };

            smtpClient.EnableSsl = true;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };

            smtpClient.Send(mail);

        }

        private void EnviarEmailParaUsuarioRecemCriadoAD(Usuario usuario)
        {
            string sRemetente = ConfigurationManager.AppSettings["Web:Remetente"];
            string sSMTP = ConfigurationManager.AppSettings["Web:SMTP"];

            MailMessage mail = new MailMessage(sRemetente, usuario.Email);

            string PrimeiroNome = GISHelpers.Utils.Severino.PrimeiraMaiusculaTodasPalavras(usuario.Nome);
            if (PrimeiroNome.Contains(" "))
                PrimeiroNome = PrimeiroNome.Substring(0, PrimeiroNome.IndexOf(" "));

            mail.Subject = PrimeiroNome + ", seja bem-vindo!";
            mail.Body = "<html style=\"font-family: Verdana; font-size: 11pt;\"><body>Olá, " + PrimeiroNome + ".";
            mail.Body += "<br /><br />";

            string NomeUsuarioInclusao = usuario.UsuarioInclusao;
            Usuario uInclusao = Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.Login.Equals(usuario.UsuarioInclusao));
            if (uInclusao != null && !string.IsNullOrEmpty(uInclusao.Nome))
                NomeUsuarioInclusao = uInclusao.Nome;

            string sLink = "http://localhost:26717/";

            mail.Body += "Você foi cadastrado no sistema GiS - Gestão Inteligente da Segurança pelo " + NomeUsuarioInclusao + ".";
            mail.Body += "<br /><br />";
            mail.Body += "Clique <a href=\"" + sLink + "\">aqui</a> para acessar a sua conta ou cole o seguinte link no seu navegador.";
            mail.Body += "<br /><br />";
            mail.Body += sLink;
            mail.Body += "<br /><br />";
            mail.Body += "Obrigado por utilizar o GiS!<br />";
            mail.Body += "<strong>Gestão Inteligente da Segurança</strong>";
            mail.Body += "<br /><br />";
            mail.Body += "<span style=\"color: #ccc; font-style: italic;\">Mensagem enviada automaticamente, favor não responder este email.</span>";
            mail.Body += "</body></html>";

            mail.IsBodyHtml = true;
            mail.BodyEncoding = Encoding.UTF8;

            SmtpClient smtpClient = new SmtpClient(sSMTP, 587);

            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = ConfigurationManager.AppSettings["Web:Remetente"],
                Password = "sesmtajt"
            };

            smtpClient.EnableSsl = true;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };

            smtpClient.Send(mail);

        }

        #endregion

        #region Senhas

        [ComVisible(false)]
        public string CreateHashFromPassword(string pstrOriginalPassword)
        {
            if (string.IsNullOrEmpty(pstrOriginalPassword))
                return string.Empty;

            string str3 = ConvertToHashedString(pstrOriginalPassword).Substring(0, 5);
            byte[] bytes = Encoding.UTF8.GetBytes(pstrOriginalPassword + str3);
            HashAlgorithm lobjHash = new MD5CryptoServiceProvider();
            return Convert.ToBase64String(lobjHash.ComputeHash(bytes));
        }

        [ComVisible(false)]
        private string ConvertToHashedString(string pstrOriginal)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(pstrOriginal);
            HashAlgorithm lobjHash = new MD5CryptoServiceProvider();
            return Convert.ToBase64String(lobjHash.ComputeHash(bytes));
        }

        #endregion


    }
}
