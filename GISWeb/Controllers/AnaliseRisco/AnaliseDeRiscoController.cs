﻿using GISCore.Business.Abstract;
using GISModel.DTO.Admissao;
using GISModel.DTO.AnaliseDeRisco;
using GISModel.DTO.AtividadeAlocadaFuncao;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISModel.Entidades.AnaliseDeRisco;
using GISModel.Entidades.Quest;
using GISModel.Enums;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GISWeb.Controllers.AnaliseRisco
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class AnaliseDeRiscoController : BaseController
    {
        #region Inject

        [Inject]
        public IBaseBusiness<Validacoes> ValidacoesBusiness { get; set; }


        [Inject]
        public IBaseBusiness<Questionario> QuestionarioBusiness { get; set; }

        [Inject]
        public IBaseBusiness<FrasesSeguranca> FrasesSegurancaBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Equipe> EquipeBusiness { get; set; }


        [Inject]
        public IBaseBusiness<Alocacao> AlocacaoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_AtividadeEquipe> REL_AtividadeEquipeBusiness { get; set; }


        [Inject]
        public IBaseBusiness<Admissao> AdmissaoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Arquivo> ArquivoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Atividade> AtividadeBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Usuario> UsuarioBusiness { get; set; }


        [Inject]
        public IBaseBusiness<FonteGeradoraDeRisco> FonteGeradoraDeRiscoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Pergunta> PerguntaBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Resposta> RespostaBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_AnaliseDeRiscoEmpregados> REL_AnaliseRiscoEmpregadoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<RespostaItem> RespostaItemBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Empresa> EmpresaBusiness { get; set; }
        [Inject]
        public IBaseBusiness<Empregado> EmpregadoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_AnaliseDeRiscoEmpregados> REL_AnaliseDeRiscoEmpregadosBusiness { get; set; }
        [Inject]
        public IBaseBusiness<ARInterrompida> ARInterrompidaBusiness { get; set; }


        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion
        // GET: AnaliseDeRisco
        public ActionResult Index()
        {
            try
            {


                var oInter = ARInterrompidaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                && a.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login) && a.Status == "Aberto").ToList();

                ViewBag.inter = oInter.ToList();


            var relacao = from rel in REL_AnaliseDeRiscoEmpregadosBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                          && a.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login)).ToList()
                          join e in EmpregadoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                          on rel.UKEmpregado equals e.UniqueKey
                          where rel.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login)
                          select new Empregado()
                          {
                              UniqueKey = rel.UniqueKey,
                              Nome = e.Nome,
                              CPF = e.CPF,
                              UsuarioInclusao = rel.UsuarioInclusao,
                              DataInclusao = rel.DataInclusao
                          };
            var data = DateTime.Now.Date;

            List<Empregado> emp = new List<Empregado>();

            foreach (var item in relacao)
            {
                if (item.DataInclusao.Date == data)
                {
                    emp.Add(item);
                }

            }

            ViewBag.Relacao = emp;


            Empregado oEmpregado = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.CPF.ToUpper().Trim().Replace(".", "").Replace("-", "").Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login.ToUpper().Trim())
                            || a.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login));

            ViewBag.Empregado = oEmpregado.Nome;
            ViewBag.cpf = oEmpregado.CPF;



            Admissao oAdmissao = AdmissaoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                                    && a.UKEmpregado.Equals(oEmpregado.UniqueKey));

            Alocacao oAloc = AlocacaoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                               && a.UKAdmissao.Equals(oAdmissao.UniqueKey));



            var Ativi = (from a in AtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                         join re in REL_AtividadeEquipeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                         on a.UniqueKey equals re.UKAtividade
                         where re.UKEquipe.Equals(oAloc.UKEquipe)
                         select a
                                 );

            ViewBag.Atividade = Ativi;

            var oRegis = REL_AnaliseDeRiscoEmpregadosBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)
               && a.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login)).OrderByDescending(b => b.DataInclusao);

                REL_AnaliseDeRiscoEmpregados iRegis = oRegis.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao)
              && a.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login));


                //     REL_AnaliseDeRiscoEmpregados AR = REL_AnaliseDeRiscoEmpregadosBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                //&& a.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login));


                if (iRegis != null)
                {
                    ViewBag.Supervisor = iRegis.UsuarioInclusao;

                    ViewBag.UKSupervisor = iRegis.UKEmpregado;
                    ViewBag.Registro = iRegis.Registro;

                    string NumRegist = iRegis.Registro;

                    string NR = NumRegist.Substring(0, 8);

                    ViewBag.Nregist = NR;


                }




           REL_AnaliseDeRiscoEmpregados oEmp = REL_AnaliseDeRiscoEmpregadosBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao)
           && a.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login));


            Questionario oQuestionario = QuestionarioBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) &&
                                        a.UKEmpresa.Equals(oAdmissao.UKEmpresa) && a.TipoQuestionario == ETipoQuestionario.Conclusao_Analise_de_Risco_Equipe);

            var oResposta = (from r in RespostaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                             join q in QuestionarioBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                             on r.UKQuestionario equals q.UniqueKey
                             where q.UniqueKey.Equals(oQuestionario.UniqueKey)
                            && r.DataInclusao.Date == data
                             select r);


            ViewBag.Total = oResposta.Count();

            ViewBag.Fechamento = oResposta.OrderByDescending(a => a.DataExclusao);

            Resposta oResp = RespostaBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login));


            if (iRegis != null)
            {
                var oRegistro = iRegis.Registro;



                var APR = from ri in RespostaItemBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                          join r in RespostaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                          on ri.UKResposta equals r.UniqueKey
                          join p in PerguntaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                          on ri.UKPergunta equals p.UniqueKey
                          join re in REL_AnaliseDeRiscoEmpregadosBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                          && a.Registro.Equals(oRegistro)).ToList().Take(1)
                          on Convert.ToString(ri.UKFonteGeradora) equals re.Registro
                          where ri.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login)
                          select new VMAnaliseDeRiscoEmpregados()
                          {

                              Pergunta = p.Descricao,
                              Resposta = ri.Resposta,
                              Data = ri.DataInclusao,
                              Status = r.Status


                          };

                List<VMAnaliseDeRiscoEmpregados> ListAPR = new List<VMAnaliseDeRiscoEmpregados>();

                foreach (var item in APR)
                {
                    if (item.Data.Date == data)
                    {
                        ListAPR.Add(item);
                    }
                }


                var Fech = from r in RespostaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao))
                           where r.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login)
                           && r.Status.Contains("Fechado")
                           select new VMAnaliseDeRiscoEmpregados()
                           {
                               Status = r.Status,
                               Data = r.DataInclusao

                           };

                List<VMAnaliseDeRiscoEmpregados> ListFech = new List<VMAnaliseDeRiscoEmpregados>();

                foreach (var itemfech in Fech)
                {
                    if (itemfech.Data.Date == data)
                    {
                        ListFech.Add(itemfech);
                    }
                }

                var fechado = ListFech.Count();

                ViewBag.Fechado = fechado;


                ViewBag.APR = ListAPR;
                ViewBag.APRcount = ListAPR.Count;


            }


            var ARisc = from a in AtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                        join ri in RespostaItemBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                        on a.UniqueKey equals ri.UKFonteGeradora
                        join r in RespostaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                        on ri.UKResposta equals r.UniqueKey
                        where ri.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login)
                        select new VMAnaliseDeRiscoEmpregados()
                        {
                            UKAtividade = a.UniqueKey,
                            Atividade = a.Descricao,
                            Data = ri.DataInclusao,
                            Resposta = ri.Resposta,
                            UKResposta = ri.UKResposta,
                            Fonte = ri.UKFonteGeradora,
                            Status = r.Status

                        };

            List<VMAnaliseDeRiscoEmpregados> ListARisc = new List<VMAnaliseDeRiscoEmpregados>();

            foreach (var item2 in ARisc)
            {
                if (item2.Data.Date == data)
                {
                    ListARisc.Add(item2);
                }
            }


            ViewBag.TotalRisc = ListARisc.Count();

            ViewBag.ARISC = ListARisc.OrderByDescending(a => a.Data);


            var oFechamento = from r in RespostaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                              join ri in RespostaItemBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                              && a.Resposta.Contains("Sim") || a.Resposta.Contains("Não")).ToList()
                              on r.UniqueKey equals ri.UKResposta
                              join a in AtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                              on ri.UKFonteGeradora equals a.UniqueKey
                              where ri.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login)
                              select new VMAnaliseDeRiscoEmpregados()
                              {
                                  UKAtividade = a.UniqueKey,
                                  UKResposta = r.UniqueKey,
                                  Data = ri.DataInclusao,
                                  Resposta = ri.Resposta,
                                  Fonte = ri.UKFonteGeradora,
                                  Atividade = a.Descricao,
                                  Status= r.Status

                              };


            List<VMAnaliseDeRiscoEmpregados> ListResposta = new List<VMAnaliseDeRiscoEmpregados>();

            foreach (var item3 in oFechamento)
            {
                if (item3.Data.Date == data)
                {
                    ListResposta.Add(item3);
                }
            }

            ViewBag.ListResposta = ListResposta.OrderByDescending(a => a.Atividade); ;
            ViewBag.TotalRespostas = ListResposta.Count();

            Random random = new Random();

            int intNumeroAleatorio;




            var frases = FrasesSegurancaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) &&
                        a.Status == GISModel.Enums.Situacao.Ativo).ToArray();

            var total = frases.Length;


            intNumeroAleatorio = random.Next(0, total);

            List<FrasesSeguranca> listfrases = new List<FrasesSeguranca>();

            if (frases != null)
            {
                listfrases.Add(frases[intNumeroAleatorio]);
            }

            ViewBag.Listafrase = listfrases;

            if (iRegis != null)
            {
                var _Registro = iRegis.Registro;

                var validacao = ValidacoesBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                  && a.Registro.Equals(_Registro)).ToList();


                    List<Validacoes> ListValidacao = new List<Validacoes>();

                    foreach (var item4 in validacao)
                    {
                        if (item4.DataExclusao.Date == data)
                        {
                            ListValidacao.Add(item4);
                        }
                    }
                    

                    ViewBag.valido = validacao.ToList();
            }

                return View();

            }
            catch (Exception)
            {

                throw;
            }


          
        }

        public ActionResult ListarAR(string ukAtividade)
        {

            var data = DateTime.Now.Date;


            var oRegis = REL_AnaliseDeRiscoEmpregadosBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)
              && a.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login)).OrderByDescending(b => b.DataInclusao);


            REL_AnaliseDeRiscoEmpregados iRegis = oRegis.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao)
             && a.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login));

            if (iRegis != null)
            {
                var _Registro = iRegis.Registro;

                var validacao = ValidacoesBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                  && a.Registro.Equals(_Registro)).ToList();


                List<Validacoes> ListValidacao = new List<Validacoes>();

                foreach (var item4 in validacao)
                {
                    if (item4.DataExclusao.Date == data)
                    {
                        ListValidacao.Add(item4);
                    }
                }


                ViewBag.valido = validacao.ToList();
            }
        


            var relacao = from rel in REL_AnaliseDeRiscoEmpregadosBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                         && a.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login)).ToList()
                          join e in EmpregadoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                          on rel.UKEmpregado equals e.UniqueKey
                          where rel.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login)
                          select new Empregado()
                          {
                              UniqueKey = rel.UniqueKey,
                              Nome = e.Nome,
                              CPF = e.CPF,
                              UsuarioInclusao = rel.UsuarioInclusao,
                              DataInclusao = rel.DataInclusao
                          };
            

            List<Empregado> emp = new List<Empregado>();

            foreach (var item in relacao)
            {
                if (item.DataInclusao.Date == data)
                {
                    emp.Add(item);
                }

            }

            ViewBag.Relacao = emp;


            Guid ativ = Guid.Parse(ukAtividade);

            var ARisc = from ri in RespostaItemBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                        join r in RespostaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                        on ri.UKResposta equals r.UniqueKey
                        join p in PerguntaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                        on ri.UKPergunta equals p.UniqueKey
                        join a in AtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                        on ri.UKFonteGeradora equals a.UniqueKey
                        where ri.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login)
                        && a.UniqueKey.Equals(ativ) && ri.DataInclusao.Date == data
                        select new VMAnaliseDeRiscoEmpregados()
                        {

                            Pergunta = p.Descricao,
                            Resposta = ri.Resposta,
                            Data = ri.DataInclusao


                        };

            List<VMAnaliseDeRiscoEmpregados> ListARisc = new List<VMAnaliseDeRiscoEmpregados>();

            foreach (var item2 in ARisc)
            {
                if (item2.Data.Date == data)
                {
                    ListARisc.Add(item2);
                }
            }

            ViewBag.ARISC = ListARisc.OrderByDescending(a => a.Data);

            Atividade oAtividade = AtividadeBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(ativ));

            RespostaItem oResp = RespostaItemBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKFonteGeradora.Equals(ativ));


            ViewBag.Atividade = oAtividade.Descricao;
            ViewBag.AtiviHora = oResp.DataInclusao.ToString("HH:mm");
            ViewBag.Resp = oResp.UsuarioInclusao;


            return PartialView("_ListarAR");
        }

        public ActionResult Registros()
        {
            Empregado oEmpregado = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.CPF.ToUpper().Trim().Replace(".", "").Replace("-", "").Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login.ToUpper().Trim())
                           || a.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login));



            ViewBag.listRegistro = RespostaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                               && a.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login.ToUpper().Trim())).ToList().Take(100).OrderByDescending(a => a.Registro);




            ViewBag.listRegistroTotal = RespostaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList().Take(100).OrderByDescending(a => a.Registro);





            return View();

        }


        public ActionResult ListaRegistroAR(string id)
        {
            //string data = Convert.ToString(entidade.DataInclusao);
            //string data1 = data.Substring(0, data.IndexOf(" - "));
            //string data2 = data.Substring(data.IndexOf(" - ") + 3);

            Guid Regis = Guid.Parse(id);

            var nomes = from r in REL_AnaliseDeRiscoEmpregadosBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                        join e in EmpregadoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                        on r.UKEmpregado equals e.UniqueKey
                        where r.Registro.Equals(id)
                        select new VMAnaliseDeRiscoEmpregados()
                        {

                            NomeEmpregado = e.Nome,
                            CPF = e.CPF,
                            Usuario = r.UsuarioInclusao
                        };


            List<VMAnaliseDeRiscoEmpregados> emp = new List<VMAnaliseDeRiscoEmpregados>();

            foreach (var item in nomes)
            {
                if (item != null)
                {
                    emp.Add(item);
                }

            }

            ViewBag.nomes = emp;

            Guid Ids = Guid.Parse(id);

            var APR = from ri in RespostaItemBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                      join p in PerguntaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                      on ri.UKPergunta equals p.UniqueKey
                      join r in RespostaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                      on ri.UKResposta equals r.UniqueKey
                      where r.Registro.Equals(id) && r.UKObjeto.Equals(Ids)
                      select new VMRelatorioAnaliseDeRisco()
                      {
                          Pergunta = p.Descricao,
                          Resposta = ri.Resposta,
                          Data = ri.DataInclusao


                      };

            List<VMRelatorioAnaliseDeRisco> rel = new List<VMRelatorioAnaliseDeRisco>();

            foreach (var resp in APR)
            {
                if (resp != null)
                {
                    rel.Add(resp);

                }
            }

            ViewBag.RelatorioAPR = rel;



            var AR = from ri in RespostaItemBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                     join p in PerguntaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                     on ri.UKPergunta equals p.UniqueKey
                     join r in RespostaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                     on ri.UKResposta equals r.UniqueKey
                     join a in AtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                     on ri.UKFonteGeradora equals a.UniqueKey
                     //join arq in ArquivoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                     //on r.Registro equals arq.NumRegistro
                     where ri.Registro.Equals(id)
                     select new VMRelatorioAnaliseDeRisco()
                     {
                         Atividade = a.Descricao,
                         Pergunta = p.Descricao,
                         Resposta = ri.Resposta,
                         Objeto = a.UniqueKey != Guid.Empty ? a.UniqueKey : Guid.Parse("null"),
                         NumRegistro = ri.Registro,
                         Data = ri.DataInclusao



                     };

            List<VMRelatorioAnaliseDeRisco> relAR = new List<VMRelatorioAnaliseDeRisco>();

            foreach (var resp in AR)
            {
                if (resp != null)
                {
                    relAR.Add(resp);

                }
            }

            ViewBag.RelatorioAR = relAR.OrderByDescending(a => a.Data);




            List<Arquivo> arquivos = ArquivoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.NumRegistro.Equals(id)).ToList();


            ViewBag.Arquivos = arquivos;



            return View();
        }


        public ActionResult VincularNome()
        {

            var reg = Guid.NewGuid();

            ViewBag.Registro = reg;


            ViewBag.Empregados = EmpregadoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

            return PartialView("_VincularNome");
        }

        [HttpPost]
        [RestritoAAjax]
        public ActionResult VincularNomes(string UKNome, string UKRegistro)
        {

            try
            {
                //Guid UK_Registro = Guid.Parse(UKRegistro);               

                if (string.IsNullOrEmpty(UKNome))
                    throw new Exception("Nenhum Nome para vincular.");
                if (string.IsNullOrEmpty(UKRegistro))
                    throw new Exception("Nenhum Registro Encontrado.");


                if (UKNome.Contains(","))
                {
                    foreach (string nom in UKNome.Split(','))
                    {
                        if (!string.IsNullOrEmpty(nom.Trim()))
                        {
                            Empregado pTemp = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Nome.Equals(nom.Trim()));
                            if (pTemp != null)
                            {
                                if (REL_AnaliseDeRiscoEmpregadosBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKEmpregado.Equals(pTemp.UniqueKey) && a.Registro.Equals(UKRegistro)).Count() == 0)
                                {
                                    REL_AnaliseDeRiscoEmpregadosBusiness.Inserir(new REL_AnaliseDeRiscoEmpregados()
                                    {
                                        Registro = UKRegistro,
                                        UKEmpregado = pTemp.UniqueKey,
                                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                                    });
                                }
                            }
                        }
                    }
                }
                else
                {
                    Empregado pTemp = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                                && a.Nome.Equals(UKNome.Trim()));

                    if (pTemp != null)
                    {
                        if (REL_AnaliseDeRiscoEmpregadosBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKEmpregado.Equals(pTemp.UniqueKey) && a.Registro.Equals(UKRegistro)).Count() == 0)
                        {
                            REL_AnaliseDeRiscoEmpregadosBusiness.Inserir(new REL_AnaliseDeRiscoEmpregados()
                            {
                                Registro = UKRegistro,
                                UKEmpregado = pTemp.UniqueKey,
                                UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                            });
                        }
                    }
                }

                return Json(new { resultado = new RetornoJSON() { Sucesso = "Nome(s) registrado com sucesso." } });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }


        }

        public ActionResult VincularAtividade()
        {

            Empregado oEmpregado = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.CPF.ToUpper().Trim().Replace(".", "").Replace("-", "").Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login.ToUpper().Trim())
                           || a.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login));


            var oAdmissao = AdmissaoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKEmpregado.Equals(oEmpregado.UniqueKey));

            var oAlocacao = AlocacaoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKAdmissao.Equals(oAdmissao.UniqueKey));


            var Atividade = (from a in AtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao))
                             join re in REL_AtividadeEquipeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao))
                             on a.UniqueKey equals re.UKAtividade
                             where re.UKEquipe.Equals(oAlocacao.UKEquipe)
                             select a
                             );

            ViewBag.Atividade = Atividade;

            //ViewBag.Atividade = AtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();

            return PartialView("_VincularAtividade");
        }


        //[HttpPost]
        //[RestritoAAjax]
        //public ActionResult VincularAtividades(string UKAtividade)
        //{

        //    try
        //    {
        //        //Guid UK_Registro = Guid.Parse(UKRegistro);               

        //        if (string.IsNullOrEmpty(UKAtividade))
        //            throw new Exception("Nenhuma Atividade encontrada.");



        //        if (UKAtividade.Contains(","))
        //        {
        //            foreach (string nom in UKAtividade.Split(','))
        //            {
        //                if (!string.IsNullOrEmpty(nom.Trim()))
        //                {
        //                    Empregado pTemp = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Nome.Equals(nom.Trim()));
        //                    if (pTemp != null)
        //                    {
        //                        if (REL_AnaliseDeRiscoEmpregadosBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKEmpregado.Equals(pTemp.UniqueKey) && a.Registro.Equals(UKRegistro)).Count() == 0)
        //                        {
        //                            REL_AnaliseDeRiscoEmpregadosBusiness.Inserir(new REL_AnaliseDeRiscoEmpregados()
        //                            {

        //                                UKEmpregado = pTemp.UniqueKey,
        //                                UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
        //                            });
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            Empregado pTemp = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao)
        //                        && a.Nome.Equals(UKAtividade.Trim()));

        //            if (pTemp != null)
        //            {
        //                if (REL_AnaliseDeRiscoEmpregadosBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKEmpregado.Equals(pTemp.UniqueKey) && a.Registro.Equals(UKRegistro)).Count() == 0)
        //                {
        //                    REL_AnaliseDeRiscoEmpregadosBusiness.Inserir(new REL_AnaliseDeRiscoEmpregados()
        //                    {

        //                        UKEmpregado = pTemp.UniqueKey,
        //                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
        //                    });
        //                }
        //            }
        //        }

        //        return Json(new { resultado = new RetornoJSON() { Sucesso = "Atividade registrada com sucesso." } });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
        //    }


        //}



        //UKFonteGeradora esta recebendo o registro pois a Atividade é somente na AR
        public ActionResult BuscarQuestionarioAPR(string UKEmpregado, string UKFonteGeradora)
        {
            try
            {
                ViewBag.UKEmpregado = UKEmpregado;
                ViewBag.UKFonteGeradora = UKFonteGeradora;

                Questionario oQuest = null;

                string sql = @"select q.UniqueKey, q.Nome, q.Tempo, q.Periodo, q.UKEmpresa, 
	                                  p.UniqueKey as UKPergunta, p.Descricao as Pergunta, p.TipoResposta, p.Ordem, 
	                                  tr.UniqueKey as UKTipoResposta, tr.Nome as TipoResposta, 
	                                  tri.Uniquekey as UKTipoRespostaItem, tri.nome as TipoRespostaItem
                               from tbAdmissao a, tbQuestionario q
		                               left join tbPergunta  p on q.UniqueKey = p.UKQuestionario and p.DataExclusao ='9999-12-31 23:59:59.997' 
		                               left join tbTipoResposta  tr on tr.UniqueKey = p.UKTipoResposta and tr.DataExclusao ='9999-12-31 23:59:59.997' 
		                               left join tbTipoRespostaItem tri on tr.UniqueKey = tri.UKTipoResposta and tri.DataExclusao ='9999-12-31 23:59:59.997' 
                               where a.UKEmpregado = '" + UKEmpregado + @"' and a.DataExclusao = '9999-12-31 23:59:59.997' and
	                                 a.UKEmpresa = q.UKEmpresa and q.DataExclusao = '9999-12-31 23:59:59.997' and q.TipoQuestionario = 4 and q.Status = 1
                               order by p.Ordem, tri.Ordem";

                DataTable result = QuestionarioBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {

                    oQuest = new Questionario();
                    oQuest.UniqueKey = Guid.Parse(result.Rows[0]["UniqueKey"].ToString());
                    oQuest.Nome = result.Rows[0]["Nome"].ToString();
                    oQuest.Periodo = (EPeriodo)Enum.Parse(typeof(EPeriodo), result.Rows[0]["Periodo"].ToString(), true);
                    oQuest.Tempo = int.Parse(result.Rows[0]["Tempo"].ToString());
                    oQuest.Perguntas = new List<Pergunta>();
                    oQuest.UKEmpresa = Guid.Parse(result.Rows[0]["UKEmpresa"].ToString());

                    Guid UKEmp = Guid.Parse(UKEmpregado);
                    //Guid UKFonte = Guid.Parse(UKFonteGeradora);


                    string sql2 = @"select MAX(DataInclusao) as UltimoQuestRespondido
                                    from tbResposta
                                    where UKEmpregado = '" + UKEmpregado + @"' and 
                                          UKQuestionario = '" + result.Rows[0]["UniqueKey"].ToString() + @"' and 
                                          UKObjeto = '" + UKFonteGeradora + "'";

                    DataTable result2 = QuestionarioBusiness.GetDataTable(sql2);
                    if (result2.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(result2.Rows[0]["UltimoQuestRespondido"].ToString()))
                        {
                            DateTime UltimaResposta = (DateTime)result2.Rows[0]["UltimoQuestRespondido"];

                            DateTime DataAtualMenosTempoQuestionario = DateTime.Now.Date;

                            var data = DateTime.Now.Date;

                            if (UltimaResposta.Date.CompareTo(data) >= 0)
                            {
                                return PartialView("_BuscarAPR");
                            }

                            //if (oQuest.Periodo == EPeriodo.Dia)
                            //{
                            //    DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddDays(-oQuest.Tempo);
                            //}
                            //else if (oQuest.Periodo == EPeriodo.Mes)
                            //{
                            //    DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddMonths(-oQuest.Tempo);
                            //}
                            //else if (oQuest.Periodo == EPeriodo.Ano)
                            //{
                            //    DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddYears(-oQuest.Tempo);
                            //}

                            //if (UltimaResposta.CompareTo(DataAtualMenosTempoQuestionario) >= 0)
                            //{
                            //    return PartialView("_BuscarAPR");
                            //}
                        }
                    }

                    foreach (DataRow row in result.Rows)
                    {


                        if (!string.IsNullOrEmpty(row["UKPergunta"].ToString()))
                        {
                            if (!string.IsNullOrEmpty(row["UKPergunta"].ToString()))
                            {
                                Pergunta oPergunta = oQuest.Perguntas.FirstOrDefault(a => a.UniqueKey.ToString().Equals(row["UKPergunta"].ToString()));
                                if (oPergunta == null)
                                {
                                    oPergunta = new Pergunta()
                                    {
                                        UniqueKey = Guid.Parse(row["UKPergunta"].ToString()),
                                        Descricao = row["Pergunta"].ToString(),
                                        Ordem = int.Parse(row["Ordem"].ToString()),
                                        TipoResposta = (ETipoResposta)Enum.Parse(typeof(ETipoResposta), row["TipoResposta"].ToString(), true)
                                    };

                                    if (!string.IsNullOrEmpty(row["UKTipoResposta"].ToString()))
                                    {
                                        TipoResposta oTipoResposta = new TipoResposta()
                                        {
                                            UniqueKey = Guid.Parse(row["UKTipoResposta"].ToString()),
                                            Nome = row["TipoResposta"].ToString(),
                                            TiposResposta = new List<TipoRespostaItem>()
                                        };

                                        if (!string.IsNullOrEmpty(row["UKTipoRespostaItem"].ToString()))
                                        {
                                            TipoRespostaItem oTipoRespostaItem = new TipoRespostaItem()
                                            {
                                                UniqueKey = Guid.Parse(row["UKTipoRespostaItem"].ToString()),
                                                Nome = row["TipoRespostaItem"].ToString()
                                            };

                                            oTipoResposta.TiposResposta.Add(oTipoRespostaItem);
                                        }

                                        oPergunta._TipoResposta = oTipoResposta;
                                    }

                                    oQuest.Perguntas.Add(oPergunta);
                                }
                                else
                                {

                                    if (!string.IsNullOrEmpty(row["UKTipoRespostaItem"].ToString()))
                                    {
                                        TipoRespostaItem oTipoRespostaItem = new TipoRespostaItem()
                                        {
                                            UniqueKey = Guid.Parse(row["UKTipoRespostaItem"].ToString()),
                                            Nome = row["TipoRespostaItem"].ToString()
                                        };

                                        oPergunta._TipoResposta.TiposResposta.Add(oTipoRespostaItem);
                                    }
                                }
                            }
                        }

                    }
                }



                return PartialView("_BuscarAPR", oQuest);
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




        public ActionResult BuscarQuestionarioAPR_MD(string UKEmpregado, string UKFonteGeradora)
        {
            try
            {
                ViewBag.UKEmpregado = UKEmpregado;
                ViewBag.UKFonteGeradora = UKFonteGeradora;

                Questionario oQuest = null;

                string sql = @"select q.UniqueKey, q.Nome, q.Tempo, q.Periodo, q.UKEmpresa, 
	                                  p.UniqueKey as UKPergunta, p.Descricao as Pergunta, p.TipoResposta, p.Ordem, 
	                                  tr.UniqueKey as UKTipoResposta, tr.Nome as TipoResposta, 
	                                  tri.Uniquekey as UKTipoRespostaItem, tri.nome as TipoRespostaItem
                               from tbAdmissao a, tbQuestionario q
		                               left join tbPergunta  p on q.UniqueKey = p.UKQuestionario and p.DataExclusao ='9999-12-31 23:59:59.997' 
		                               left join tbTipoResposta  tr on tr.UniqueKey = p.UKTipoResposta and tr.DataExclusao ='9999-12-31 23:59:59.997' 
		                               left join tbTipoRespostaItem tri on tr.UniqueKey = tri.UKTipoResposta and tri.DataExclusao ='9999-12-31 23:59:59.997' 
                               where a.UKEmpregado = '" + UKEmpregado + @"' and a.DataExclusao = '9999-12-31 23:59:59.997' and
	                                 a.UKEmpresa = q.UKEmpresa and q.DataExclusao = '9999-12-31 23:59:59.997' and q.TipoQuestionario = 4 and q.Status = 1
                               order by p.Ordem, tri.Ordem";

                DataTable result = QuestionarioBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {

                    oQuest = new Questionario();
                    oQuest.UniqueKey = Guid.Parse(result.Rows[0]["UniqueKey"].ToString());
                    oQuest.Nome = result.Rows[0]["Nome"].ToString();
                    oQuest.Periodo = (EPeriodo)Enum.Parse(typeof(EPeriodo), result.Rows[0]["Periodo"].ToString(), true);
                    oQuest.Tempo = int.Parse(result.Rows[0]["Tempo"].ToString());
                    oQuest.Perguntas = new List<Pergunta>();
                    oQuest.UKEmpresa = Guid.Parse(result.Rows[0]["UKEmpresa"].ToString());

                    Guid UKEmp = Guid.Parse(UKEmpregado);
                    //Guid UKFonte = Guid.Parse(UKFonteGeradora);


                    string sql2 = @"select MAX(DataInclusao) as UltimoQuestRespondido
                                    from tbResposta
                                    where UKEmpregado = '" + UKEmpregado + @"' and 
                                          UKQuestionario = '" + result.Rows[0]["UniqueKey"].ToString() + @"' and 
                                          UKObjeto = '" + UKFonteGeradora + "'";

                    DataTable result2 = QuestionarioBusiness.GetDataTable(sql2);
                    if (result2.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(result2.Rows[0]["UltimoQuestRespondido"].ToString()))
                        {
                            DateTime UltimaResposta = (DateTime)result2.Rows[0]["UltimoQuestRespondido"];

                            DateTime DataAtualMenosTempoQuestionario = DateTime.Now.Date;

                            var data = DateTime.Now.Date;

                            if (UltimaResposta.Date.CompareTo(data) >= 0)
                            {
                                return PartialView("_BuscarAPRMD");
                            }

                            //if (oQuest.Periodo == EPeriodo.Dia)
                            //{
                            //    DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddDays(-oQuest.Tempo);
                            //}
                            //else if (oQuest.Periodo == EPeriodo.Mes)
                            //{
                            //    DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddMonths(-oQuest.Tempo);
                            //}
                            //else if (oQuest.Periodo == EPeriodo.Ano)
                            //{
                            //    DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddYears(-oQuest.Tempo);
                            //}

                            //if (UltimaResposta.CompareTo(DataAtualMenosTempoQuestionario) >= 0)
                            //{
                            //    return PartialView("_BuscarAPR");
                            //}
                        }
                    }

                    foreach (DataRow row in result.Rows)
                    {


                        if (!string.IsNullOrEmpty(row["UKPergunta"].ToString()))
                        {
                            if (!string.IsNullOrEmpty(row["UKPergunta"].ToString()))
                            {
                                Pergunta oPergunta = oQuest.Perguntas.FirstOrDefault(a => a.UniqueKey.ToString().Equals(row["UKPergunta"].ToString()));
                                if (oPergunta == null)
                                {
                                    oPergunta = new Pergunta()
                                    {
                                        UniqueKey = Guid.Parse(row["UKPergunta"].ToString()),
                                        Descricao = row["Pergunta"].ToString(),
                                        Ordem = int.Parse(row["Ordem"].ToString()),
                                        TipoResposta = (ETipoResposta)Enum.Parse(typeof(ETipoResposta), row["TipoResposta"].ToString(), true)
                                    };

                                    if (!string.IsNullOrEmpty(row["UKTipoResposta"].ToString()))
                                    {
                                        TipoResposta oTipoResposta = new TipoResposta()
                                        {
                                            UniqueKey = Guid.Parse(row["UKTipoResposta"].ToString()),
                                            Nome = row["TipoResposta"].ToString(),
                                            TiposResposta = new List<TipoRespostaItem>()
                                        };

                                        if (!string.IsNullOrEmpty(row["UKTipoRespostaItem"].ToString()))
                                        {
                                            TipoRespostaItem oTipoRespostaItem = new TipoRespostaItem()
                                            {
                                                UniqueKey = Guid.Parse(row["UKTipoRespostaItem"].ToString()),
                                                Nome = row["TipoRespostaItem"].ToString()
                                            };

                                            oTipoResposta.TiposResposta.Add(oTipoRespostaItem);
                                        }

                                        oPergunta._TipoResposta = oTipoResposta;
                                    }

                                    oQuest.Perguntas.Add(oPergunta);
                                }
                                else
                                {

                                    if (!string.IsNullOrEmpty(row["UKTipoRespostaItem"].ToString()))
                                    {
                                        TipoRespostaItem oTipoRespostaItem = new TipoRespostaItem()
                                        {
                                            UniqueKey = Guid.Parse(row["UKTipoRespostaItem"].ToString()),
                                            Nome = row["TipoRespostaItem"].ToString()
                                        };

                                        oPergunta._TipoResposta.TiposResposta.Add(oTipoRespostaItem);
                                    }
                                }
                            }
                        }

                    }
                }

                return PartialView("_BuscarAPRMD", oQuest);
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

        public ActionResult BuscarQuestionarioPorSupervisor(string UKEmpregado, string UKFonteGeradora, string oRegistro)
        {
            try
            {

                Atividade oAtividade = AtividadeBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                                            && a.Descricao.Equals(UKFonteGeradora));

                

                



                Guid oUKEmpregado = Guid.Parse(UKEmpregado);


                var oEquipe = (from al in AlocacaoBusiness.Consulta.Where(a=>string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                               join adm in AdmissaoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                               on al.UKAdmissao equals adm.UniqueKey
                               join e in EquipeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                               on al.UKEquipe equals e.UniqueKey
                               where adm.UKEmpregado.Equals(oUKEmpregado)
                               select e
                               );


                List<string> oTipoQuestionario = new List<string>();

                string TQuest = "";

                foreach (var item in oEquipe)
                {
                    if (item != null && item.NomeDaEquipe.Contains("EQUIPE DE CORTE/RELIGA"))
                    {
                        TQuest = "6";
                        
                    }
                    if (item != null && item.NomeDaEquipe.Contains("EQUIPE DE LIGAÇÃO/SERVIÇOS"))
                    {
                        TQuest = "7";
                        
                    }
                    if (item != null && item.NomeDaEquipe.Contains("EQUIPE DE CONSTRUÇÃO/ MANUTENÇÃO EM RDA"))
                    {
                        TQuest = "9";

                    }

                }
                                               

                ViewBag.equipe = oTipoQuestionario;                


                ViewBag.UKEmpregado = UKEmpregado;
                ViewBag.UKFonteGeradora = oAtividade.UniqueKey;
                var fonte = oAtividade.UniqueKey;

                ViewBag.Registro = oRegistro;

                Questionario oQuest = null;

                //FonteGeradora aqui é a atividade

                string sql = @"select q.UniqueKey, q.Nome, q.Tempo, q.Periodo, q.UKEmpresa, 
	                                  p.UniqueKey as UKPergunta, p.Descricao as Pergunta, p.TipoResposta, p.Ordem, 
	                                  tr.UniqueKey as UKTipoResposta, tr.Nome as TipoResposta, 
	                                  tri.Uniquekey as UKTipoRespostaItem, tri.nome as TipoRespostaItem
                               from tbAdmissao a, tbQuestionario q
		                               left join tbPergunta  p on q.UniqueKey = p.UKQuestionario and p.DataExclusao ='9999-12-31 23:59:59.997' 
		                               left join tbTipoResposta  tr on tr.UniqueKey = p.UKTipoResposta and tr.DataExclusao ='9999-12-31 23:59:59.997' 
		                               left join tbTipoRespostaItem tri on tr.UniqueKey = tri.UKTipoResposta and tri.DataExclusao ='9999-12-31 23:59:59.997' 
                               where a.UKEmpregado = '" + UKEmpregado + @"' and a.DataExclusao = '9999-12-31 23:59:59.997' and
	                                 a.UKEmpresa = q.UKEmpresa and q.DataExclusao = '9999-12-31 23:59:59.997' and q.TipoQuestionario = '" + TQuest + @"' and q.Status = 1
                               order by p.Ordem, tri.Ordem";

                DataTable result = QuestionarioBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {

                    oQuest = new Questionario();
                    oQuest.UniqueKey = Guid.Parse(result.Rows[0]["UniqueKey"].ToString());
                    oQuest.Nome = result.Rows[0]["Nome"].ToString();
                    oQuest.Periodo = (EPeriodo)Enum.Parse(typeof(EPeriodo), result.Rows[0]["Periodo"].ToString(), true);
                    oQuest.Tempo = int.Parse(result.Rows[0]["Tempo"].ToString());
                    oQuest.Perguntas = new List<Pergunta>();
                    oQuest.UKEmpresa = Guid.Parse(result.Rows[0]["UKEmpresa"].ToString());

                    Guid UKEmp = Guid.Parse(UKEmpregado);
                    Guid UKFonte = oAtividade.UniqueKey;


                    string sql2 = @"select MAX(DataInclusao) as UltimoQuestRespondido
                                    from tbResposta
                                    where UKEmpregado = '" + UKEmpregado + @"' and 
                                          UKQuestionario = '" + result.Rows[0]["UniqueKey"].ToString() + @"' and 
                                          UKObjeto = '" + fonte + "'";

                    DataTable result2 = QuestionarioBusiness.GetDataTable(sql2);
                    if (result2.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(result2.Rows[0]["UltimoQuestRespondido"].ToString()))
                        {
                            DateTime UltimaResposta = (DateTime)result2.Rows[0]["UltimoQuestRespondido"];

                            DateTime DataAtualMenosTempoQuestionario = DateTime.Now;

                            var data = DateTime.Now.Date;

                            //if (UltimaResposta.Date.CompareTo(data) >= 0)
                            //{
                            //    return PartialView("_BuscarAPR");
                            //}




                            //if (oQuest.Periodo == EPeriodo.Dia)
                            //{
                            //    DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddDays(-oQuest.Tempo);
                            //}
                            //else if (oQuest.Periodo == EPeriodo.Mes)
                            //{
                            //    DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddMonths(-oQuest.Tempo);
                            //}
                            //else if (oQuest.Periodo == EPeriodo.Ano)
                            //{
                            //    DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddYears(-oQuest.Tempo);
                            //}

                            //if (UltimaResposta.CompareTo(DataAtualMenosTempoQuestionario) >= 0)
                            //{
                            //    return PartialView("_BuscarQuestionario");
                            //}
                        }
                    }

                    foreach (DataRow row in result.Rows)
                    {


                        if (!string.IsNullOrEmpty(row["UKPergunta"].ToString()))
                        {
                            if (!string.IsNullOrEmpty(row["UKPergunta"].ToString()))
                            {
                                Pergunta oPergunta = oQuest.Perguntas.FirstOrDefault(a => a.UniqueKey.ToString().Equals(row["UKPergunta"].ToString()));
                                if (oPergunta == null)
                                {
                                    oPergunta = new Pergunta()
                                    {
                                        UniqueKey = Guid.Parse(row["UKPergunta"].ToString()),
                                        Descricao = row["Pergunta"].ToString(),
                                        Ordem = int.Parse(row["Ordem"].ToString()),
                                        TipoResposta = (ETipoResposta)Enum.Parse(typeof(ETipoResposta), row["TipoResposta"].ToString(), true)
                                    };

                                    if (!string.IsNullOrEmpty(row["UKTipoResposta"].ToString()))
                                    {
                                        TipoResposta oTipoResposta = new TipoResposta()
                                        {
                                            UniqueKey = Guid.Parse(row["UKTipoResposta"].ToString()),
                                            Nome = row["TipoResposta"].ToString(),
                                            TiposResposta = new List<TipoRespostaItem>()
                                        };

                                        if (!string.IsNullOrEmpty(row["UKTipoRespostaItem"].ToString()))
                                        {
                                            TipoRespostaItem oTipoRespostaItem = new TipoRespostaItem()
                                            {
                                                UniqueKey = Guid.Parse(row["UKTipoRespostaItem"].ToString()),
                                                Nome = row["TipoRespostaItem"].ToString()
                                            };

                                            oTipoResposta.TiposResposta.Add(oTipoRespostaItem);
                                        }

                                        oPergunta._TipoResposta = oTipoResposta;
                                    }

                                    oQuest.Perguntas.Add(oPergunta);
                                }
                                else
                                {

                                    if (!string.IsNullOrEmpty(row["UKTipoRespostaItem"].ToString()))
                                    {
                                        TipoRespostaItem oTipoRespostaItem = new TipoRespostaItem()
                                        {
                                            UniqueKey = Guid.Parse(row["UKTipoRespostaItem"].ToString()),
                                            Nome = row["TipoRespostaItem"].ToString()
                                        };

                                        oPergunta._TipoResposta.TiposResposta.Add(oTipoRespostaItem);
                                    }
                                }
                            }
                        }

                    }
                }

                return PartialView("_BuscarQuestionario", oQuest);

                
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

        public ActionResult BuscarQuestionarioPorSupervisorMD(string UKEmpregado, string UKFonteGeradora, string oRegistro)
        {
            try
            {

                Atividade oAtividade = AtividadeBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao)
                                            && a.Descricao.Equals(UKFonteGeradora));







                Guid oUKEmpregado = Guid.Parse(UKEmpregado);


                var oEquipe = (from al in AlocacaoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                               join adm in AdmissaoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                               on al.UKAdmissao equals adm.UniqueKey
                               join e in EquipeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                               on al.UKEquipe equals e.UniqueKey
                               where adm.UKEmpregado.Equals(oUKEmpregado)
                               select e
                               );


                List<string> oTipoQuestionario = new List<string>();

                string TQuest = "";

                foreach (var item in oEquipe)
                {
                    if (item != null && item.NomeDaEquipe.Contains("EQUIPE DE CORTE/RELIGA"))
                    {
                        TQuest = "6";

                    }
                    if (item != null && item.NomeDaEquipe.Contains("EQUIPE DE LIGAÇÃO/SERVIÇOS"))
                    {
                        TQuest = "7";

                    }
                    if (item != null && item.NomeDaEquipe.Contains("EQUIPE DE CONSTRUÇÃO/ MANUTENÇÃO EM RDA"))
                    {
                        TQuest = "9";

                    }

                }


                ViewBag.equipe = oTipoQuestionario;


                ViewBag.UKEmpregado = UKEmpregado;
                ViewBag.UKFonteGeradora = oAtividade.UniqueKey;
                var fonte = oAtividade.UniqueKey;

                ViewBag.Registro = oRegistro;

                Questionario oQuest = null;

                //FonteGeradora aqui é a atividade

                string sql = @"select q.UniqueKey, q.Nome, q.Tempo, q.Periodo, q.UKEmpresa, 
	                                  p.UniqueKey as UKPergunta, p.Descricao as Pergunta, p.TipoResposta, p.Ordem, 
	                                  tr.UniqueKey as UKTipoResposta, tr.Nome as TipoResposta, 
	                                  tri.Uniquekey as UKTipoRespostaItem, tri.nome as TipoRespostaItem
                               from tbAdmissao a, tbQuestionario q
		                               left join tbPergunta  p on q.UniqueKey = p.UKQuestionario and p.DataExclusao ='9999-12-31 23:59:59.997' 
		                               left join tbTipoResposta  tr on tr.UniqueKey = p.UKTipoResposta and tr.DataExclusao ='9999-12-31 23:59:59.997' 
		                               left join tbTipoRespostaItem tri on tr.UniqueKey = tri.UKTipoResposta and tri.DataExclusao ='9999-12-31 23:59:59.997' 
                               where a.UKEmpregado = '" + UKEmpregado + @"' and a.DataExclusao = '9999-12-31 23:59:59.997' and
	                                 a.UKEmpresa = q.UKEmpresa and q.DataExclusao = '9999-12-31 23:59:59.997' and q.TipoQuestionario = '" + TQuest + @"' and q.Status = 1
                               order by p.Ordem, tri.Ordem";

                DataTable result = QuestionarioBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {

                    oQuest = new Questionario();
                    oQuest.UniqueKey = Guid.Parse(result.Rows[0]["UniqueKey"].ToString());
                    oQuest.Nome = result.Rows[0]["Nome"].ToString();
                    oQuest.Periodo = (EPeriodo)Enum.Parse(typeof(EPeriodo), result.Rows[0]["Periodo"].ToString(), true);
                    oQuest.Tempo = int.Parse(result.Rows[0]["Tempo"].ToString());
                    oQuest.Perguntas = new List<Pergunta>();
                    oQuest.UKEmpresa = Guid.Parse(result.Rows[0]["UKEmpresa"].ToString());

                    Guid UKEmp = Guid.Parse(UKEmpregado);
                    Guid UKFonte = oAtividade.UniqueKey;


                    string sql2 = @"select MAX(DataInclusao) as UltimoQuestRespondido
                                    from tbResposta
                                    where UKEmpregado = '" + UKEmpregado + @"' and 
                                          UKQuestionario = '" + result.Rows[0]["UniqueKey"].ToString() + @"' and 
                                          UKObjeto = '" + fonte + "'";

                    DataTable result2 = QuestionarioBusiness.GetDataTable(sql2);
                    if (result2.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(result2.Rows[0]["UltimoQuestRespondido"].ToString()))
                        {
                            DateTime UltimaResposta = (DateTime)result2.Rows[0]["UltimoQuestRespondido"];

                            DateTime DataAtualMenosTempoQuestionario = DateTime.Now;

                            var data = DateTime.Now.Date;

                            //if (UltimaResposta.Date.CompareTo(data) >= 0)
                            //{
                            //    return PartialView("_BuscarAPR");
                            //}




                            //if (oQuest.Periodo == EPeriodo.Dia)
                            //{
                            //    DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddDays(-oQuest.Tempo);
                            //}
                            //else if (oQuest.Periodo == EPeriodo.Mes)
                            //{
                            //    DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddMonths(-oQuest.Tempo);
                            //}
                            //else if (oQuest.Periodo == EPeriodo.Ano)
                            //{
                            //    DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddYears(-oQuest.Tempo);
                            //}

                            //if (UltimaResposta.CompareTo(DataAtualMenosTempoQuestionario) >= 0)
                            //{
                            //    return PartialView("_BuscarQuestionario");
                            //}
                        }
                    }

                    foreach (DataRow row in result.Rows)
                    {


                        if (!string.IsNullOrEmpty(row["UKPergunta"].ToString()))
                        {
                            if (!string.IsNullOrEmpty(row["UKPergunta"].ToString()))
                            {
                                Pergunta oPergunta = oQuest.Perguntas.FirstOrDefault(a => a.UniqueKey.ToString().Equals(row["UKPergunta"].ToString()));
                                if (oPergunta == null)
                                {
                                    oPergunta = new Pergunta()
                                    {
                                        UniqueKey = Guid.Parse(row["UKPergunta"].ToString()),
                                        Descricao = row["Pergunta"].ToString(),
                                        Ordem = int.Parse(row["Ordem"].ToString()),
                                        TipoResposta = (ETipoResposta)Enum.Parse(typeof(ETipoResposta), row["TipoResposta"].ToString(), true)
                                    };

                                    if (!string.IsNullOrEmpty(row["UKTipoResposta"].ToString()))
                                    {
                                        TipoResposta oTipoResposta = new TipoResposta()
                                        {
                                            UniqueKey = Guid.Parse(row["UKTipoResposta"].ToString()),
                                            Nome = row["TipoResposta"].ToString(),
                                            TiposResposta = new List<TipoRespostaItem>()
                                        };

                                        if (!string.IsNullOrEmpty(row["UKTipoRespostaItem"].ToString()))
                                        {
                                            TipoRespostaItem oTipoRespostaItem = new TipoRespostaItem()
                                            {
                                                UniqueKey = Guid.Parse(row["UKTipoRespostaItem"].ToString()),
                                                Nome = row["TipoRespostaItem"].ToString()
                                            };

                                            oTipoResposta.TiposResposta.Add(oTipoRespostaItem);
                                        }

                                        oPergunta._TipoResposta = oTipoResposta;
                                    }

                                    oQuest.Perguntas.Add(oPergunta);
                                }
                                else
                                {

                                    if (!string.IsNullOrEmpty(row["UKTipoRespostaItem"].ToString()))
                                    {
                                        TipoRespostaItem oTipoRespostaItem = new TipoRespostaItem()
                                        {
                                            UniqueKey = Guid.Parse(row["UKTipoRespostaItem"].ToString()),
                                            Nome = row["TipoRespostaItem"].ToString()
                                        };

                                        oPergunta._TipoResposta.TiposResposta.Add(oTipoRespostaItem);
                                    }
                                }
                            }
                        }

                    }
                }


                return PartialView("_BuscarQuestionarioMD", oQuest);


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

        public ActionResult BuscarQuestionarioConclusao(string UKAtividade, string UKFonteGeradora)
        {
            try
            {
                ViewBag.UKatividade = UKAtividade;

                Empregado oEmpregado = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.CPF.ToUpper().Trim().Replace(".", "").Replace("-", "").Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login.ToUpper().Trim())
                            || a.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login));

                ViewBag.UKFonteGeradora = UKFonteGeradora;

                ViewBag.UKEmpregado = oEmpregado.UniqueKey;

                Questionario oQuest = null;

                string sql = @"select q.UniqueKey, q.Nome, q.Tempo, q.Periodo, q.UKEmpresa, 
	                                  p.UniqueKey as UKPergunta, p.Descricao as Pergunta, p.TipoResposta, p.Ordem, 
	                                  tr.UniqueKey as UKTipoResposta, tr.Nome as TipoResposta, 
	                                  tri.Uniquekey as UKTipoRespostaItem, tri.nome as TipoRespostaItem
                               from tbAdmissao a, tbQuestionario q
		                               left join tbPergunta  p on q.UniqueKey = p.UKQuestionario and p.DataExclusao ='9999-12-31 23:59:59.997' 
		                               left join tbTipoResposta  tr on tr.UniqueKey = p.UKTipoResposta and tr.DataExclusao ='9999-12-31 23:59:59.997' 
		                               left join tbTipoRespostaItem tri on tr.UniqueKey = tri.UKTipoResposta and tri.DataExclusao ='9999-12-31 23:59:59.997' 
                               where a.UKEmpregado = '" + oEmpregado.UniqueKey + @"' and a.DataExclusao = '9999-12-31 23:59:59.997' and
	                                 a.UKEmpresa = q.UKEmpresa and q.DataExclusao = '9999-12-31 23:59:59.997' and q.TipoQuestionario = 5 and q.Status = 1
                               order by p.Ordem, tri.Ordem";

                DataTable result = QuestionarioBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {

                    oQuest = new Questionario();
                    oQuest.UniqueKey = Guid.Parse(result.Rows[0]["UniqueKey"].ToString());
                    oQuest.Nome = result.Rows[0]["Nome"].ToString();
                    oQuest.Periodo = (EPeriodo)Enum.Parse(typeof(EPeriodo), result.Rows[0]["Periodo"].ToString(), true);
                    oQuest.Tempo = int.Parse(result.Rows[0]["Tempo"].ToString());
                    oQuest.Perguntas = new List<Pergunta>();
                    oQuest.UKEmpresa = Guid.Parse(result.Rows[0]["UKEmpresa"].ToString());

                    //Guid UKEmp = Guid.Parse(UKEmpregado);
                    //Guid UKFonte = Guid.Parse(UKFonteGeradora);


                    string sql2 = @"select MAX(DataInclusao) as UltimoQuestRespondido
                                    from tbResposta
                                    where UKEmpregado = '" + oEmpregado.UniqueKey + @"' and 
                                          UKQuestionario = '" + result.Rows[0]["UniqueKey"].ToString() + @"' and 
                                          UKObjeto = '" + UKAtividade + @"' and Registro = '" + UKFonteGeradora + "'";
                                            

                    DataTable result2 = QuestionarioBusiness.GetDataTable(sql2);
                    if (result2.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(result2.Rows[0]["UltimoQuestRespondido"].ToString()))
                        {
                            DateTime UltimaResposta = (DateTime)result2.Rows[0]["UltimoQuestRespondido"];

                            DateTime DataAtualMenosTempoQuestionario = DateTime.Now.Date;

                            var data = DateTime.Now.Date;

                            if (UltimaResposta.Date.CompareTo(data) >= 0)
                            {
                                return PartialView("_QuestionarioQC");
                            }

                            //if (oQuest.Periodo == EPeriodo.Dia)
                            //{
                            //    DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddDays(-oQuest.Tempo);
                            //}
                            //else if (oQuest.Periodo == EPeriodo.Mes)
                            //{
                            //    DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddMonths(-oQuest.Tempo);
                            //}
                            //else if (oQuest.Periodo == EPeriodo.Ano)
                            //{
                            //    DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddYears(-oQuest.Tempo);
                            //}

                            //if (UltimaResposta.CompareTo(DataAtualMenosTempoQuestionario) >= 0)
                            //{
                            //    return PartialView("_BuscarAPR");
                            //}
                        }
                    }

                    foreach (DataRow row in result.Rows)
                    {


                        if (!string.IsNullOrEmpty(row["UKPergunta"].ToString()))
                        {
                            if (!string.IsNullOrEmpty(row["UKPergunta"].ToString()))
                            {
                                Pergunta oPergunta = oQuest.Perguntas.FirstOrDefault(a => a.UniqueKey.ToString().Equals(row["UKPergunta"].ToString()));
                                if (oPergunta == null)
                                {
                                    oPergunta = new Pergunta()
                                    {
                                        UniqueKey = Guid.Parse(row["UKPergunta"].ToString()),
                                        Descricao = row["Pergunta"].ToString(),
                                        Ordem = int.Parse(row["Ordem"].ToString()),
                                        TipoResposta = (ETipoResposta)Enum.Parse(typeof(ETipoResposta), row["TipoResposta"].ToString(), true)
                                    };

                                    if (!string.IsNullOrEmpty(row["UKTipoResposta"].ToString()))
                                    {
                                        TipoResposta oTipoResposta = new TipoResposta()
                                        {
                                            UniqueKey = Guid.Parse(row["UKTipoResposta"].ToString()),
                                            Nome = row["TipoResposta"].ToString(),
                                            TiposResposta = new List<TipoRespostaItem>()
                                        };

                                        if (!string.IsNullOrEmpty(row["UKTipoRespostaItem"].ToString()))
                                        {
                                            TipoRespostaItem oTipoRespostaItem = new TipoRespostaItem()
                                            {
                                                UniqueKey = Guid.Parse(row["UKTipoRespostaItem"].ToString()),
                                                Nome = row["TipoRespostaItem"].ToString()
                                            };

                                            oTipoResposta.TiposResposta.Add(oTipoRespostaItem);
                                        }

                                        oPergunta._TipoResposta = oTipoResposta;
                                    }

                                    oQuest.Perguntas.Add(oPergunta);
                                }
                                else
                                {

                                    if (!string.IsNullOrEmpty(row["UKTipoRespostaItem"].ToString()))
                                    {
                                        TipoRespostaItem oTipoRespostaItem = new TipoRespostaItem()
                                        {
                                            UniqueKey = Guid.Parse(row["UKTipoRespostaItem"].ToString()),
                                            Nome = row["TipoRespostaItem"].ToString()
                                        };

                                        oPergunta._TipoResposta.TiposResposta.Add(oTipoRespostaItem);
                                    }
                                }
                            }
                        }

                    }
                }

                return PartialView("_QuestionarioQC", oQuest);
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


        public ActionResult BuscarQuestionarioConclusaoMD(string UKAtividade, string UKFonteGeradora)
        {
            try
            {
                ViewBag.UKatividade = UKAtividade;

                Empregado oEmpregado = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.CPF.ToUpper().Trim().Replace(".", "").Replace("-", "").Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login.ToUpper().Trim())
                            || a.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login));

                ViewBag.UKFonteGeradora = UKFonteGeradora;

                ViewBag.UKEmpregado = oEmpregado.UniqueKey;

                Questionario oQuest = null;

                string sql = @"select q.UniqueKey, q.Nome, q.Tempo, q.Periodo, q.UKEmpresa, 
	                                  p.UniqueKey as UKPergunta, p.Descricao as Pergunta, p.TipoResposta, p.Ordem, 
	                                  tr.UniqueKey as UKTipoResposta, tr.Nome as TipoResposta, 
	                                  tri.Uniquekey as UKTipoRespostaItem, tri.nome as TipoRespostaItem
                               from tbAdmissao a, tbQuestionario q
		                               left join tbPergunta  p on q.UniqueKey = p.UKQuestionario and p.DataExclusao ='9999-12-31 23:59:59.997' 
		                               left join tbTipoResposta  tr on tr.UniqueKey = p.UKTipoResposta and tr.DataExclusao ='9999-12-31 23:59:59.997' 
		                               left join tbTipoRespostaItem tri on tr.UniqueKey = tri.UKTipoResposta and tri.DataExclusao ='9999-12-31 23:59:59.997' 
                               where a.UKEmpregado = '" + oEmpregado.UniqueKey + @"' and a.DataExclusao = '9999-12-31 23:59:59.997' and
	                                 a.UKEmpresa = q.UKEmpresa and q.DataExclusao = '9999-12-31 23:59:59.997' and q.TipoQuestionario = 5 and q.Status = 1
                               order by p.Ordem, tri.Ordem";

                DataTable result = QuestionarioBusiness.GetDataTable(sql);
                if (result.Rows.Count > 0)
                {

                    oQuest = new Questionario();
                    oQuest.UniqueKey = Guid.Parse(result.Rows[0]["UniqueKey"].ToString());
                    oQuest.Nome = result.Rows[0]["Nome"].ToString();
                    oQuest.Periodo = (EPeriodo)Enum.Parse(typeof(EPeriodo), result.Rows[0]["Periodo"].ToString(), true);
                    oQuest.Tempo = int.Parse(result.Rows[0]["Tempo"].ToString());
                    oQuest.Perguntas = new List<Pergunta>();
                    oQuest.UKEmpresa = Guid.Parse(result.Rows[0]["UKEmpresa"].ToString());

                    //Guid UKEmp = Guid.Parse(UKEmpregado);
                    //Guid UKFonte = Guid.Parse(UKFonteGeradora);


                    string sql2 = @"select MAX(DataInclusao) as UltimoQuestRespondido
                                    from tbResposta
                                    where UKEmpregado = '" + oEmpregado.UniqueKey + @"' and 
                                          UKQuestionario = '" + result.Rows[0]["UniqueKey"].ToString() + @"' and 
                                          UKObjeto = '" + UKAtividade + @"' and Registro = '" + UKFonteGeradora + "'";


                    DataTable result2 = QuestionarioBusiness.GetDataTable(sql2);
                    if (result2.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(result2.Rows[0]["UltimoQuestRespondido"].ToString()))
                        {
                            DateTime UltimaResposta = (DateTime)result2.Rows[0]["UltimoQuestRespondido"];

                            DateTime DataAtualMenosTempoQuestionario = DateTime.Now.Date;

                            var data = DateTime.Now.Date;

                            if (UltimaResposta.Date.CompareTo(data) >= 0)
                            {
                                return PartialView("_QuestionarioQCMD ");
                            }

                            //if (oQuest.Periodo == EPeriodo.Dia)
                            //{
                            //    DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddDays(-oQuest.Tempo);
                            //}
                            //else if (oQuest.Periodo == EPeriodo.Mes)
                            //{
                            //    DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddMonths(-oQuest.Tempo);
                            //}
                            //else if (oQuest.Periodo == EPeriodo.Ano)
                            //{
                            //    DataAtualMenosTempoQuestionario = DataAtualMenosTempoQuestionario.AddYears(-oQuest.Tempo);
                            //}

                            //if (UltimaResposta.CompareTo(DataAtualMenosTempoQuestionario) >= 0)
                            //{
                            //    return PartialView("_BuscarAPR");
                            //}
                        }
                    }

                    foreach (DataRow row in result.Rows)
                    {


                        if (!string.IsNullOrEmpty(row["UKPergunta"].ToString()))
                        {
                            if (!string.IsNullOrEmpty(row["UKPergunta"].ToString()))
                            {
                                Pergunta oPergunta = oQuest.Perguntas.FirstOrDefault(a => a.UniqueKey.ToString().Equals(row["UKPergunta"].ToString()));
                                if (oPergunta == null)
                                {
                                    oPergunta = new Pergunta()
                                    {
                                        UniqueKey = Guid.Parse(row["UKPergunta"].ToString()),
                                        Descricao = row["Pergunta"].ToString(),
                                        Ordem = int.Parse(row["Ordem"].ToString()),
                                        TipoResposta = (ETipoResposta)Enum.Parse(typeof(ETipoResposta), row["TipoResposta"].ToString(), true)
                                    };

                                    if (!string.IsNullOrEmpty(row["UKTipoResposta"].ToString()))
                                    {
                                        TipoResposta oTipoResposta = new TipoResposta()
                                        {
                                            UniqueKey = Guid.Parse(row["UKTipoResposta"].ToString()),
                                            Nome = row["TipoResposta"].ToString(),
                                            TiposResposta = new List<TipoRespostaItem>()
                                        };

                                        if (!string.IsNullOrEmpty(row["UKTipoRespostaItem"].ToString()))
                                        {
                                            TipoRespostaItem oTipoRespostaItem = new TipoRespostaItem()
                                            {
                                                UniqueKey = Guid.Parse(row["UKTipoRespostaItem"].ToString()),
                                                Nome = row["TipoRespostaItem"].ToString()
                                            };

                                            oTipoResposta.TiposResposta.Add(oTipoRespostaItem);
                                        }

                                        oPergunta._TipoResposta = oTipoResposta;
                                    }

                                    oQuest.Perguntas.Add(oPergunta);
                                }
                                else
                                {

                                    if (!string.IsNullOrEmpty(row["UKTipoRespostaItem"].ToString()))
                                    {
                                        TipoRespostaItem oTipoRespostaItem = new TipoRespostaItem()
                                        {
                                            UniqueKey = Guid.Parse(row["UKTipoRespostaItem"].ToString()),
                                            Nome = row["TipoRespostaItem"].ToString()
                                        };

                                        oPergunta._TipoResposta.TiposResposta.Add(oTipoRespostaItem);
                                    }
                                }
                            }
                        }

                    }
                }

                return PartialView("_QuestionarioQCMD ", oQuest);
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

        public ActionResult Geolocalizacao() {




            return View();

        }




        [RestritoAAjax]
        public ActionResult BuscarNomeForAutoComplete(string key)
        {
            try
            {
                var emp = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.CPF.ToUpper().Trim().Replace(".", "").Replace("-", "").Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login.ToUpper().Trim())
                            || a.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login));

                var adm = AdmissaoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKEmpregado.Equals(emp.UniqueKey));


                List<string> riscosAsString = new List<string>();

                var empList = from a in AdmissaoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                                join e in EmpregadoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Nome.ToUpper().Contains(key.ToUpper())).ToList()
                                on a.UKEmpregado equals e.UniqueKey                                
                                where a.UKEmpresa.Equals(adm.UKEmpresa)
                                select new PerfilEmpregadoViewModel()
                                {
                                    NomeEmpregado = e.Nome
                                };

                List<string> ListEmp = new List<string>();

                foreach (var item in empList)
                {
                    if(item != null)
                    {
                        ListEmp.Add(item.NomeEmpregado);
                    }

                }

                List <Empregado> lista = EmpregadoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Nome.ToUpper().Contains(key.ToUpper())).ToList();

                foreach (Empregado com in lista)
                    riscosAsString.Add(com.Nome);

                return Json(new { Result = ListEmp });
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }

        [RestritoAAjax]
        public ActionResult ConfirmarnomeForAutoComplete(string key)
        {
            try
            {
               


                Empregado item = EmpregadoBusiness.Consulta.FirstOrDefault(a => a.Nome.ToUpper().Equals(key.ToUpper()));

                if (item == null)
                    throw new Exception();

                return Json(new { Result = true });
            }
            catch
            {
                return Json(new { Result = false });
            }
        }
   




        [RestritoAAjax]
        public ActionResult BuscarAtividadeForAutoComplete(string key)
        {
            try
            {
                Empregado oEmpregado = EmpregadoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.CPF.ToUpper().Trim().Replace(".", "").Replace("-", "").Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login.ToUpper().Trim())
                          || a.UsuarioInclusao.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Login));


                var oAdmissao = AdmissaoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKEmpregado.Equals(oEmpregado.UniqueKey));

                var oAlocacao = AlocacaoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKAdmissao.Equals(oAdmissao.UniqueKey));

                

              var oAtividade = from a in AtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                                       join re in REL_AtividadeEquipeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                                       on a.UniqueKey equals re.UKAtividade                                       
                                       where re.UKEquipe.Equals(oAlocacao.UKEquipe)
                                       select new AtividadeAlocadaFuncaoViewModel()
                                       {
                                           Descricao = a.Descricao,

                                       };
                                 

                List<string> riscosAsString = new List<string>();

                List<string> ListAtividade = new List<string>();

                foreach (var item in oAtividade)
                {
                    if (item != null)
                    {
                        ListAtividade.Add(item.Descricao);
                    }

                }

                List<Atividade> lista = AtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Descricao.ToUpper().Contains(key.ToUpper())).ToList();

                foreach (Atividade com in lista)

                    riscosAsString.Add(com.Descricao);

                return Json(new { Result = ListAtividade });
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }

        [RestritoAAjax]
        public ActionResult ConfirmarAtividadeForAutoComplete(string key)
        {
            try
            {



                Atividade item = AtividadeBusiness.Consulta.FirstOrDefault(a => a.Descricao.ToUpper().Equals(key.ToUpper()));

                if (item == null)
                    throw new Exception();

                return Json(new { Result = true });
            }
            catch
            {
                return Json(new { Result = false });
            }
        }

    }
}
        

