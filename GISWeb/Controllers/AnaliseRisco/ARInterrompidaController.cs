using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades.AnaliseDeRisco;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using GISModel.Entidades;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.Text;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGrid.Helpers.Mail.Model;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace GISWeb.Controllers.AnaliseRisco
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ARInterrompidaController : BaseController
    {
        #region
        [Inject]
        public IBaseBusiness<ARInterrompida> ARInterrompidaBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_AnaliseDeRiscoEmpregados> REL_AnaliseDeRiscoEmpregadosBusiness { get; set; }

        [Inject]
        public IBaseBusiness<PlanoDeAcao> PlanoDeAcaoBusiness { get; set; }


        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }
        #endregion

        // GET: ARInterrompida
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]        
        public ActionResult Cadastrar(ARInterrompida oARinterrompida)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    oARinterrompida.Status = "Aberta";
                    oARinterrompida.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    ARInterrompidaBusiness.Inserir(oARinterrompida);

                    //var data = DateTime.Now.Date;

                    var user = CustomAuthorizationProvider.UsuarioAutenticado.Login;

                    REL_AnaliseDeRiscoEmpregados rARE = REL_AnaliseDeRiscoEmpregadosBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Registro.Equals(oARinterrompida.Registro));

                    rARE.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Nome;

                    List<REL_AnaliseDeRiscoEmpregados> oARE = REL_AnaliseDeRiscoEmpregadosBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Registro.Equals(oARinterrompida.Registro)).ToList();

                    string oSql = "UPDATE REL_AnaliseDeRiscoEmpregados SET UsuarioExclusao = '" + CustomAuthorizationProvider.UsuarioAutenticado.Nome + "' where Registro = '"+ oARinterrompida.Registro + "' ";

                    SqlCommand updatecommand = new SqlCommand(oSql);


                    DataTable result = REL_AnaliseDeRiscoEmpregadosBusiness.GetDataTable(oSql);

                    



                    var oRegis = oARinterrompida.Registro;
                    var oItem = oARinterrompida.Item;
                    var ofato = oARinterrompida.Descricao;

                    PlanoDeAcao oPlanoDeAcao = null;

                    
                        if(oPlanoDeAcao == null)
                        {
                        PlanoDeAcao planoDeAcao = new PlanoDeAcao()
                        {
                            UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Nome,
                                Identificador = Guid.Parse(oRegis),
                                item = oItem,
                                fato = ofato,
                                status = "Aberto",

                            };
                            PlanoDeAcaoBusiness.Inserir(planoDeAcao);
                        }

                                       

                    Extensions.GravaCookie("MensagemSucesso", "O Evento '" + oARinterrompida.Descricao + "' gerou um Plano de Ação e a atividade deverá ser interrompida!", 10);

                    
                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "AnaliseDeRisco") } });

                }
                catch (Exception ex)
                {
                    if (ex.GetBaseException() == null)
                    {
                        return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
                    }
                    else
                    {
                        return Json(new { resultado = new RetornoJSON() { Erro = ex.GetBaseException().Message } });
                    }
                }

            }
            else
            {
                return Json(new { resultado = TratarRetornoValidacaoToJSON() });

            }
        }


        public ActionResult Grafics()
        {

            var TotalEventos = ARInterrompidaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao));
            var totalE = TotalEventos.Count();

            ViewBag.total = totalE;


            return View();
        }


        //public async Task SendEmail(string toEmail, string subject, string content)
        //{

        //    var apiKey = Environment.GetEnvironmentVariable("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");
        //    var client = new SendGridClient(apiKey);
        //    var from = new EmailAddress("encelavisosar@gmail.com", "Testando envio SendGrid");            
        //    var to = new EmailAddress(toEmail);
        //    var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
        //    var response = await client.SendEmailAsync(msg); 


        //}


        //private void EnviarEmailParaUsuarioRecemCriadoAD(string emailDestinatario)
        //{
        //    string sRemetente = ConfigurationManager.AppSettings["Web:Remetente"];
        //    string sSMTP = ConfigurationManager.AppSettings["Web:SMTP"];

        //    MailMessage mail = new MailMessage(sRemetente, emailDestinatario);

        //    string PrimeiroNome = GISHelpers.Utils.Severino.PrimeiraMaiusculaTodasPalavras(emailDestinatario);
        //    if (PrimeiroNome.Contains(" "))
        //        PrimeiroNome = PrimeiroNome.Substring(0, PrimeiroNome.IndexOf(" "));

        //    mail.Subject = PrimeiroNome + ", seja bem-vindo!";
        //    mail.Body = "<html style=\"font-family: Verdana; font-size: 11pt;\"><body>Olá, " + PrimeiroNome + ".";
        //    mail.Body += "<br /><br />";

        //    //string NomeUsuarioInclusao = usuario.UsuarioInclusao;
        //    //Usuario uInclusao = Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.Login.Equals(usuario.UsuarioInclusao));
        //    //if (uInclusao != null && !string.IsNullOrEmpty(uInclusao.Nome))
        //        //NomeUsuarioInclusao = "Antonio";

        //    string sLink = "http://localhost:26717/";

        //    mail.Body += "Você foi cadastrado no sistema GiS - Gestão Inteligente da Segurança pelo ";
        //    mail.Body += "<br /><br />";
        //    mail.Body += "Clique <a href=\"" + sLink + "\">aqui</a> para acessar a sua conta ou cole o seguinte link no seu navegador.";
        //    mail.Body += "<br /><br />";
        //    mail.Body += sLink;
        //    mail.Body += "<br /><br />";
        //    mail.Body += "Obrigado por utilizar o GiS!<br />";
        //    mail.Body += "<strong>Gestão Inteligente da Segurança</strong>";
        //    mail.Body += "<br /><br />";
        //    mail.Body += "<span style=\"color: #ccc; font-style: italic;\">Mensagem enviada automaticamente, favor não responder este email.</span>";
        //    mail.Body += "</body></html>";

        //    mail.IsBodyHtml = true;
        //    mail.BodyEncoding = Encoding.UTF8;

        //    SmtpClient smtpClient = new SmtpClient(sSMTP, 587);

        //    smtpClient.Credentials = new System.Net.NetworkCredential()
        //    {
        //        UserName = ConfigurationManager.AppSettings["Web:Remetente"],
        //        Password = "gabrielcaete@123"

        //    };

        //    smtpClient.EnableSsl = true;
        //    smtpClient.UseDefaultCredentials = false;
        //    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
        //            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
        //            System.Security.Cryptography.X509Certificates.X509Chain chain,
        //            System.Net.Security.SslPolicyErrors sslPolicyErrors)
        //    {
        //        return true;
        //    };

        //    smtpClient.Send(mail);

        //}





        //public bool SendMail(string email)
        //{
        //    // Configurações de correio
        //    MailMessage mailMsg = new MailMessage("encelavisosar@gmail.com", email); // o e-mail do remetente verificado que você registrou em minha conta

        //    // Configurações de mensagem
        //    mailMsg.Subject = "Alerta";    // qualquer texto ou dados padrão de nossa caixa de texto
        //    mailMsg.Body = "Aviso de Segurança";     // qualquer texto ou dados padrão de nossa caixa de texto

        //    // Configurações de SMTP
        //    SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
        //    NetworkCredential credentials = new NetworkCredential("encelavisosar@gmail.com", "gabrielcaete@123"); // o id de e-mail e a senha que registramos para fazer o login no sendgrid 
        //    smtpClient.Credentials = credentials;
        //    if (mailMsg != null)
        //    {
        //        smtpClient.Send(mailMsg);
        //        Response.Write("<script> alert ('Correio enviado!') </script>");
        //    }

        //    return true;
        //}





        //public bool SendMail(string email)
        //{
        //    try
        //    {
        //        // Estancia da Classe de Mensagem
        //        MailMessage _mailMessage = new MailMessage();
        //        // Remetente
        //        _mailMessage.From = new MailAddress("encelavisosar@gmail.com");

        //        // Destinatario seta no metodo abaixo

        //        //Contrói o MailMessage
        //        _mailMessage.CC.Add(email);
        //        _mailMessage.Subject = "Teste Aviso atividade interrompida";
        //        _mailMessage.IsBodyHtml = true;
        //        _mailMessage.Body = "<b>Uma Atividade foi interrompida e precisa de tratamento!</b><p>Teste Parágrafo</p>";

        //        //CONFIGURAÇÃO COM PORTA
        //        SmtpClient _smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32("587"));

        //        //CONFIGURAÇÃO SEM PORTA
        //        // SmtpClient _smtpClient = new SmtpClient(UtilRsource.ConfigSmtp);

        //        // Credencial para envio por SMTP Seguro (Quando o servidor exige autenticação)
        //        _smtpClient.UseDefaultCredentials = false;
        //        _smtpClient.Credentials = new NetworkCredential("encelavisosar@gmail.com", "encel@123");

        //        _smtpClient.EnableSsl = true;

        //        _smtpClient.Send(_mailMessage);

        //        return true;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


    }







}