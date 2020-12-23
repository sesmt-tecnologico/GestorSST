using GISCore.Business.Abstract;
using GISModel.Entidades;
using GISModel.Entidades.AnaliseDeRisco;
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

namespace GISWeb.Controllers
{

    [Autorizador]
    [DadosUsuario]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class InboxController : BaseController
    {
        #region
        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        [Inject]
        public IUsuarioPerfilBusiness UsuarioPerfilBusiness { get; set; }

        [Inject]
        public IPerfilBusiness PerfilBusiness { get; set; }

        [Inject]
        public IBaseBusiness<Workflow> WorkflowBusiness { get; set; }

        [Inject]
        public IBaseBusiness<ARInterrompida> ARInterrompidaBusiness { get; set; }

        #endregion

        // GET: Inbox
        public ActionResult BuscarTotalDocsInbox()
        {

            int iTotal = 0;
            int iPessoal = 0;
            var TotalPend = 0;

            if (memoryCacheDefault.Contains(CustomAuthorizationProvider.UsuarioAutenticado.Login + "InboxTotal"))
            {
                List<int> lista = (List<int>)memoryCacheDefault[CustomAuthorizationProvider.UsuarioAutenticado.Login + "InboxTotal"];
                iTotal = lista[0];
                iPessoal = lista[1];

            }
            else
            {
                List<string> perfis = (from usuarioperfil in UsuarioPerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                       join perfil in PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usuarioperfil.UKPerfil equals perfil.UniqueKey
                                       where usuarioperfil.UKUsuario.Equals(CustomAuthorizationProvider.UsuarioAutenticado.UniqueKey)
                                       select "'" + perfil.Nome + "'").ToList();




                //string sql01 = @"select 'Posicao' as posicao,count(*) as total from REL_DocumentoAlocacao d
                //                where d.Posicao = 1 ";

                string sql01 = @"select 'Status' as posicao,count(*) as total from tbARInterrompida d
                                where d.Status = 1 ";

                DataTable dtInbox = WorkflowBusiness.GetDataTable(sql01);
                if (dtInbox.Rows.Count > 0)
                {
                    foreach (DataRow row in dtInbox.Rows)
                    {
                        if (row[0].ToString().Equals("Status"))
                        {
                            iPessoal = int.Parse(row[1].ToString());
                        }

                    }

                    iTotal = iPessoal;
                }


                


                List<int> lista = new List<int>();
                lista.Add(iTotal);
                lista.Add(iPessoal);


                memoryCacheDefault.Add(CustomAuthorizationProvider.UsuarioAutenticado.Login + "InboxTotal", lista, DateTime.Now.AddHours(2));
            }

                return Json(new { resultado = new { Total = iTotal, Pessoal = iPessoal } });
            }
        }
    }
