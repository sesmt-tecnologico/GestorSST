using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;
using System.Data;
using System.Collections.Generic;

namespace GISWeb.Controllers
{
    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class PossiveisDanosController : BaseController
    {

        #region Inject

        [Inject]
        public ITipoDeRiscoBusiness TipoDeRiscoBusiness { get; set; }

        [Inject]
        public IEventoPerigosoBusiness EventoPerigosoBusiness { get; set; }


        [Inject]
        public IPossiveisDanosBusiness PossiveisDanosBusiness { get; set; }

        [Inject]
        public IEventoPerigosoBusiness EventoPerigosBusiness { get; set; }

        [Inject]
        public IRiscoBusiness RiscoBusiness { get; set; }

        [Inject]
        public IBaseBusiness<REL_RiscoDanosASaude> REL_RiscoDanosBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion


        public ActionResult Index()
        {
            ViewBag.PossiveisDanos = PossiveisDanosBusiness.Consulta.Where(d => string.IsNullOrEmpty(d.UsuarioExclusao)).OrderBy(p => p.DescricaoDanos).ToList();

            return View();
        }

        public ActionResult Novo()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(PossiveisDanos oPossiveisDanoso)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (PossiveisDanosBusiness.Consulta.Any(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.DescricaoDanos.ToUpper().Trim().Equals(oPossiveisDanoso.DescricaoDanos.ToUpper().Trim())))
                        throw new Exception("Já existe um possível dano com esta descrição. Favor informar outro nome para o possível dano.");

                    oPossiveisDanoso.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    PossiveisDanosBusiness.Inserir(oPossiveisDanoso);

                    Extensions.GravaCookie("MensagemSucesso", "O Possivel dano '" + oPossiveisDanoso.DescricaoDanos + "' foi cadastrado com sucesso!", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "PossiveisDanos") } });
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

        public ActionResult Edicao(string id)
        {
            Guid UK_PossiveisDanos = Guid.Parse(id);
            return View(PossiveisDanosBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UK_PossiveisDanos)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(PossiveisDanos entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PossiveisDanos obj = PossiveisDanosBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(entidade.UniqueKey));
                    if (obj == null)
                        throw new Exception("Não foi possível encontrar o possível dano a ser editado na base de dados.");



                    if (PossiveisDanosBusiness.Consulta.Any(a => string.IsNullOrEmpty(a.UsuarioExclusao) && 
                                                                 !a.UniqueKey.Equals(entidade.UniqueKey) &&
                                                                 a.DescricaoDanos.ToUpper().Trim().Equals(entidade.DescricaoDanos.ToUpper().Trim())))
                        throw new Exception("Já existe um possível dano com esta descrição. Favor informar outro nome para o possível dano.");



                    obj.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    PossiveisDanosBusiness.Terminar(obj);

                    entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    PossiveisDanosBusiness.Inserir(entidade);

                    Extensions.GravaCookie("MensagemSucesso", "O Possivel Dano '" + entidade.DescricaoDanos + "' foi atualizado com sucesso.", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "PossiveisDanos") } });
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

        [HttpPost]
        public ActionResult Terminar(string IDPossiveisDanos)
        {
            Guid uk = Guid.Parse(IDPossiveisDanos);
            try
            {
                PossiveisDanos oPossiveisDanos = PossiveisDanosBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(uk));
                if (oPossiveisDanos == null)
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o Possivel Dano, pois o mesmo não foi localizado." } });

                oPossiveisDanos.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                PossiveisDanosBusiness.Terminar(oPossiveisDanos);

                Extensions.GravaCookie("MensagemSucesso", "O Possivel Dano '" + oPossiveisDanos.DescricaoDanos + "' foi excluído com sucesso.", 10);

                return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "PossiveisDanos") } });
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






        public ActionResult ListaRiscos()
        {

            string sql = @"select r.UniqueKey as UK_Risco, r.Nome as Nome_risco, d.UniqueKey as UK_Danos, d.DescricaoDanos, rd.UKRiscos as rel01,

                                  rd.UKDanosSaude as rel02 
                           from [dbo].[tbRisco] r
                                left join [REL_RiscoDanosASaude]  rd on rd.UKRiscos = r.UniqueKey and r.DataExclusao = CAST('9999-12-31 23:59:59.997'as datetime2)
                                left join [tbPossiveisDanos]  d on d.UniqueKey = rd.UKDanosSaude and d.DataExclusao = CAST('9999-12-31 23:59:59.997'as datetime2)
                           order by r.Nome";



            DataTable result = RiscoBusiness.GetDataTable(sql);

            List<Risco> lista = new List<Risco>();


            if (result.Rows.Count > 0)
            {
                Risco obj = null;
                PossiveisDanos oDanos = null;

                foreach (DataRow row in result.Rows)
                {
                    if (obj == null)
                    {
                        obj = new Risco()
                        {
                            UniqueKey = Guid.Parse(row["UK_Risco"].ToString()),
                            Nome = row["Nome_risco"].ToString(),
                            Danos = new List<PossiveisDanos>()
                        };


                        if (!string.IsNullOrEmpty(row["rel02"].ToString()))
                        {
                            oDanos = new PossiveisDanos()
                            {
                                UniqueKey = Guid.Parse(row["rel02"].ToString()),
                                DescricaoDanos = row["DescricaoDanos"].ToString(),

                            };
                            obj.Danos.Add(oDanos);
                        }


                    }
                    //if UniqueKey for igual a UKPerigo
                    else if (obj.UniqueKey.Equals(Guid.Parse(row["UK_Risco"].ToString())))
                    {
                        //if UKRisco nao for nulo
                        if (!string.IsNullOrEmpty(row["rel01"].ToString()))
                        {
                            if (oDanos != null)
                            {
                                oDanos = new PossiveisDanos()
                                {
                                    UniqueKey = Guid.Parse(row["rel02"].ToString()),
                                    DescricaoDanos = row["DescricaoDanos"].ToString(),
                                };

                                obj.Danos.Add(oDanos);

                            }


                        }


                    }


                    else
                    {
                        lista.Add(obj);

                        obj = new Risco()
                        {
                            UniqueKey = Guid.Parse(row["UK_Risco"].ToString()),
                            Nome = row["Nome_risco"].ToString(),
                            Danos = new List<PossiveisDanos>()
                        };


                        if (!string.IsNullOrEmpty(row["rel02"].ToString()))
                        {
                            oDanos = new PossiveisDanos()
                            {
                                UniqueKey = Guid.Parse(row["rel02"].ToString()),
                                DescricaoDanos = row["DescricaoDanos"].ToString(),

                            };

                            obj.Danos.Add(oDanos);
                        }
                    }
                }

                if (obj != null)
                    lista.Add(obj);

            }

            return View("_ListaRiscos", lista);

        }

        [HttpPost]
        [RestritoAAjax]
        public ActionResult VincularDanos(string UKRisco)
        {

            ViewBag.UKRisco = UKRisco;

            return PartialView("_VincularPossiveisDanos");
        }


        [HttpPost]
        [RestritoAAjax]
        public ActionResult VincularRiscoDano(string UKRisco, string UKDano)
        {

            try
            {
                Guid UK_Risco = Guid.Parse(UKRisco);

                if (string.IsNullOrEmpty(UKRisco))
                    throw new Exception("Não foi possível localizar o Risco.");

                if (string.IsNullOrEmpty(UKDano))
                    throw new Exception("Nenhum Dano para vincular.");


                if (UKDano.Contains(","))
                {
                    foreach (string ativ in UKDano.Split(','))
                    {
                        if (!string.IsNullOrEmpty(ativ.Trim()))
                        {
                            PossiveisDanos pTemp = PossiveisDanosBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.DescricaoDanos.Equals(ativ.Trim()));
                            if (pTemp != null)
                            {
                                if (REL_RiscoDanosBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKDanosSaude.Equals(pTemp.UniqueKey) && a.UKRiscos.Equals(UK_Risco)).Count() == 0)
                                {
                                    REL_RiscoDanosBusiness.Inserir(new REL_RiscoDanosASaude()
                                    {
                                        UKRiscos = UK_Risco,
                                        UKDanosSaude = pTemp.UniqueKey,
                                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                                    });
                                }
                            }
                        }
                    }
                }
                else
                {
                    PossiveisDanos pTemp = PossiveisDanosBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.DescricaoDanos.Equals(UKDano.Trim()));

                    if (pTemp != null)
                    {
                        if (REL_RiscoDanosBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UKDanosSaude.Equals(pTemp.UniqueKey) && a.UKRiscos.Equals(UK_Risco)).Count() == 0)
                        {
                            REL_RiscoDanosBusiness.Inserir(new REL_RiscoDanosASaude()
                            {
                                UKRiscos = UK_Risco,
                                UKDanosSaude = pTemp.UniqueKey,
                                UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login
                            });
                        }
                    }
                }

                return Json(new { resultado = new RetornoJSON() { Sucesso = "Possíveis danos a saúde vinculado ao Risco com sucesso." } });
            }
            catch (Exception ex)
            {
                return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
            }


        }


        [RestritoAAjax]
        public ActionResult BuscarDanosForAutoComplete(string key)
        {
            try
            {
                List<string> danosAsString = new List<string>();
                List<PossiveisDanos> lista = PossiveisDanosBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.DescricaoDanos.ToUpper().Contains(key.ToUpper())).ToList();

                foreach (PossiveisDanos com in lista)
                    danosAsString.Add(com.DescricaoDanos);

                return Json(new { Result = danosAsString });
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }

        [RestritoAAjax]
        public ActionResult ConfirmarDanosForAutoComplete(string key)
        {
            try
            {
                PossiveisDanos item = PossiveisDanosBusiness.Consulta.FirstOrDefault(a => a.DescricaoDanos.ToUpper().Equals(key.ToUpper()));

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





