using GISCore.Business.Abstract;
using GISModel.Entidades.Quest;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using GISCore.Infrastructure.Utils;
using GISModel.DTO.Shared;

namespace GISWeb.Controllers.Quest
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class TipoRespostaController : BaseController
    {

        #region Inject

        [Inject]
        public IBaseBusiness<TipoResposta> TipoRespostaBusiness { get; set; }

        [Inject]
        public IBaseBusiness<TipoRespostaItem> TipoRespostaItemBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BuscarTiposDeResposta()
        {

            List<TipoResposta> lista = new List<TipoResposta>();

            string sql = @"select tr.UniqueKey as UKTipoResposta, tr.Nome as TipoResposta, tri.Uniquekey as UKTipoRespostaItem, tri.nome as TipoRespostaItem
                           from tbTipoResposta tr
		                          left join tbTipoRespostaItem  tri on tr.UniqueKey = tri.UKTipoResposta and tri.DataExclusao ='9999-12-31 23:59:59.997' 
                           where tr.DataExclusao ='9999-12-31 23:59:59.997' order by tr.Nome, tri.ordem ";

            DataTable result = TipoRespostaBusiness.GetDataTable(sql);
            if (result.Rows.Count > 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    TipoResposta temp = lista.FirstOrDefault(a => a.UniqueKey.ToString().Equals(row["UKTipoResposta"].ToString()));
                    if (temp == null)
                    {
                        TipoResposta obj = new TipoResposta()
                        {
                            UniqueKey = Guid.Parse(row["UKTipoResposta"].ToString()),
                            Nome = row["TipoResposta"].ToString(),
                            TiposResposta = new List<TipoRespostaItem>()
                        };

                        if (!string.IsNullOrEmpty(row["UKTipoRespostaItem"].ToString()))
                        {
                            obj.TiposResposta.Add(new TipoRespostaItem()
                            {
                                UniqueKey = Guid.Parse(row["UKTipoRespostaItem"].ToString()),
                                Nome = row["TipoRespostaItem"].ToString()
                            });
                        }

                        lista.Add(obj);

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(row["UKTipoRespostaItem"].ToString()))
                        {
                            temp.TiposResposta.Add(new TipoRespostaItem()
                            {
                                UniqueKey = Guid.Parse(row["UKTipoRespostaItem"].ToString()),
                                Nome = row["TipoRespostaItem"].ToString()
                            });
                        }
                    }
                }
            }

            return PartialView("_BuscarTiposDeResposta", lista);
        }

        public ActionResult Novo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(TipoResposta entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(entidade.Nome))
                        throw new Exception("Informe o nome da resposta de múltipla escolha.");

                    entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                    TipoRespostaBusiness.Inserir(entidade);

                    Extensions.GravaCookie("MensagemSucesso", "O tipo de resposta '" + entidade.Nome + "' foi cadastrado com sucesso.", 10);

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "TipoResposta") } });
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
        public ActionResult Terminar(string id)
        {
            try
            {
                Guid UKDep = Guid.Parse(id);
                TipoResposta temp = TipoRespostaBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKDep));
                if (temp == null)
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o tipo de resposta, pois a mesmo não foi localizado na base de dados." } });

                temp.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Login;
                TipoRespostaBusiness.Terminar(temp);

                return Json(new { resultado = new RetornoJSON() { Sucesso = "O tipo de resposta '" + temp.Nome + "' foi excluído com sucesso." } });

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

    }
}