using Gestor.Job.Startup.Infrastructure;
using Gestor.Job.Startup.Infrastructure.DependencyResolver;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Gestor.Job.Startup
{
    class Program
    {
        static void Main(string[] args)
        {
            string iArq = ConfigurationManager.AppSettings["PathFileStartup"];
            if (File.Exists(iArq))
            {
                var newFile = new FileInfo(iArq);
                Orb OrbEffect = new Orb(CustomNinjectResolver.Inject());
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    foreach (ExcelWorksheet ws in xlPackage.Workbook.Worksheets)
                    {


                        if (ws.Name.Equals("AtividadeEmergencial"))
                        {
                            LoadAtividadeEquipe(ws, OrbEffect);
                        }


                        //if (ws.Name.Equals("Atividade"))
                        //{
                        //    LoadAtividade(ws, OrbEffect);
                        //}


                        //if (ws.Name.Equals("tbEmpresa"))
                        //{
                        //    LoadEmpresa(ws, OrbEffect);
                        //}
                        //else if (ws.Name.Equals("tbNivelHierarquico"))
                        //{
                        //    LoadNivelHierarquico(ws, OrbEffect);
                        //}
                        //else if (ws.Name.Equals("tbDepartamento"))
                        //{
                        //    LoadDepartamento(ws, OrbEffect);
                        //}
                        //else if (ws.Name.Equals("tbPerfil"))
                        //{
                        //    LoadPerfil(ws, OrbEffect);
                        //}
                        //else if (ws.Name.Equals("tbUsuario"))
                        //{
                        //    LoadUsuario(ws, OrbEffect);
                        //}
                        //else if (ws.Name.Equals("tbUsuarioPerfil"))
                        //{
                        //    LoadUsuarioPerfil(ws, OrbEffect);
                        //}
                    }
                }
            }
        }


        internal static void LoadAtividadeEquipe(ExcelWorksheet ws, Orb OrbEffect)
        {

            var start = ws.Dimension.Start;
            var end = ws.Dimension.End;
            for (int row = start.Row + 1; row <= end.Row; row++)
            {
                string ID = ws.Cells[row, 1].Text;
                string UKEmpresa = ws.Cells[row, 2].Text;
                string UKEquipe = ws.Cells[row, 3].Text;
                string UKAtividade = ws.Cells[row, 4].Text;
                string Uniquekey = ws.Cells[row, 5].Text;
                string UsuarioInclusao = ws.Cells[row, 6].Text;
                string DataInclusao = ws.Cells[row, 7].Text;
                string UsuarioExclusao = ws.Cells[row, 8].Text;
                string DataExclusao = ws.Cells[row, 9].Text;


                OrbEffect.CadastrarAtividadeEquipe(new GISModel.Entidades.REL_AtividadeEquipe()
                {

                    //ID = Guid.Parse(ID),
                    UKEmpresa = Guid.Parse(UKEmpresa.Trim()),
                    UKEquipe = Guid.Parse(UKEquipe.Trim()),
                    UKAtividade = Guid.Parse(UKAtividade.Trim()),
                    UsuarioInclusao = UsuarioInclusao


                });

            }

           }





        //    internal static void LoadAtividade(ExcelWorksheet ws, Orb OrbEffect)
        //{

        //    var start = ws.Dimension.Start;
        //    var end = ws.Dimension.End;
        //    for (int row = start.Row + 1; row <= end.Row; row++)
        //    {
        //        string ID = ws.Cells[row, 1].Text;
        //        string Descricao = ws.Cells[row, 2].Text;
        //        string Uniquekey = ws.Cells[row, 3].Text;
        //        string UsuarioExclusao = ws.Cells[row, 4].Text;
        //        string UsuarioInclusao = ws.Cells[row, 5].Text;
        //        string DataInclusao = ws.Cells[row, 6].Text;
        //        string DataExclusao = ws.Cells[row, 7].Text;
        //        string Funcao_ID = ws.Cells[row, 8].Text;
        //        string Codigo = ws.Cells[row, 9].Text;


        //        OrbEffect.CadastrarAtividade(new GISModel.Entidades.Atividade()
        //        {

        //            //ID = Guid.Parse(ID),
        //            Descricao = Descricao,
        //            //UniqueKey = Guid.Parse(Uniquekey),
        //            UsuarioExclusao = UsuarioExclusao,
        //            UsuarioInclusao = UsuarioInclusao,
        //            //DataInclusao = DataInclusao,
        //            //DataExclusao = DataExclusao,
        //            //Funcao_ID = ws.Cells[row, 8].Text;
        //            Codigo = ws.Cells[row, 9].Text

        //        });

        //    }

        //}



        //internal static void LoadEmpresa(ExcelWorksheet ws, Orb OrbEffect)
        //{

        //    var start = ws.Dimension.Start;
        //    var end = ws.Dimension.End;
        //    for (int row = start.Row + 1; row <= end.Row; row++)
        //    {
        //        string UK = ws.Cells[row, 1].Text;
        //        string CNPJ = ws.Cells[row, 2].Text;
        //        string RazaoSocial = ws.Cells[row, 3].Text;
        //        string NomeFantasia = ws.Cells[row, 4].Text;
        //        string URLSite = ws.Cells[row, 5].Text;
        //        string URLAD = ws.Cells[row, 6].Text;
        //        string URLWS = ws.Cells[row, 7].Text;
        //        string UsuarioInclusao = ws.Cells[row, 8].Text;

        //        OrbEffect.CadastrarEmpresa(new GISModel.Entidades.Empresa() {
        //            UniqueKey = Guid.Parse(UK),                    
        //            CNPJ = CNPJ,
        //            RazaoSocial = RazaoSocial,
        //            NomeFantasia = NomeFantasia,
        //            URL_Site = URLSite,
        //            URL_AD = URLAD,
        //            URL_WS = URLWS,
        //            UsuarioInclusao = UsuarioInclusao
        //        });

        //    }

        //}

        //internal static void LoadNivelHierarquico(ExcelWorksheet ws, Orb OrbEffect)
        //{

        //    var start = ws.Dimension.Start;
        //    var end = ws.Dimension.End;
        //    for (int row = start.Row + 1; row <= end.Row; row++)
        //    {
        //        string UK = ws.Cells[row, 1].Text;
        //        string Nome = ws.Cells[row, 2].Text;
        //        string UsuarioInclusao = ws.Cells[row, 3].Text;

        //        OrbEffect.CadastrarNivelHierarquico(new GISModel.Entidades.NivelHierarquico()
        //        {
        //            UniqueKey = Guid.Parse(UK),
        //            Nome = Nome,
        //            UsuarioInclusao = UsuarioInclusao
        //        });

        //    }

        //}

        //internal static void LoadDepartamento(ExcelWorksheet ws, Orb OrbEffect)
        //{
        //    var start = ws.Dimension.Start;
        //    var end = ws.Dimension.End;
        //    for (int row = start.Row + 1; row <= end.Row; row++)
        //    {
        //        string UK = ws.Cells[row, 1].Text;
        //        string Codigo = ws.Cells[row, 2].Text;
        //        string Sigla = ws.Cells[row, 3].Text;
        //        string Descricao = ws.Cells[row, 4].Text;
        //        string Status = ws.Cells[row, 5].Text;
        //        string UKEmpresa = ws.Cells[row, 6].Text;
        //        string UKDepartamentoVinculado = ws.Cells[row, 7].Text;
        //        string UKNivelHierarquico = ws.Cells[row, 8].Text;
        //        string UsuarioInclusao = ws.Cells[row, 9].Text;

        //        GISModel.Entidades.Departamento dep = new GISModel.Entidades.Departamento()
        //        {
        //            UniqueKey = Guid.Parse(UK),
        //            Codigo = Codigo,
        //            Sigla = Sigla,
        //            Descricao = Descricao,
        //            Status = (GISModel.Enums.Situacao)Enum.Parse(typeof(GISModel.Enums.Situacao), Status, true),
        //            UKEmpresa = Guid.Parse(UKEmpresa),
        //            UKNivelHierarquico = Guid.Parse(UKNivelHierarquico),
        //            UsuarioInclusao = UsuarioInclusao
        //        };

        //        if (!string.IsNullOrEmpty(UKDepartamentoVinculado))
        //        {
        //            dep.UKDepartamentoVinculado = Guid.Parse(UKDepartamentoVinculado);
        //        }

        //        OrbEffect.CadastrarDepartamento(dep);
        //    }
        //}

        //internal static void LoadPerfil(ExcelWorksheet ws, Orb OrbEffect)
        //{
        //    var start = ws.Dimension.Start;
        //    var end = ws.Dimension.End;
        //    for (int row = start.Row + 1; row <= end.Row; row++)
        //    {
        //        string UK = ws.Cells[row, 1].Text;
        //        string Nome = ws.Cells[row, 2].Text;
        //        string Descricao = ws.Cells[row, 3].Text;
        //        string Action = ws.Cells[row, 4].Text;
        //        string Controller = ws.Cells[row, 5].Text;
        //        string UsuarioInclusao = ws.Cells[row, 6].Text;

        //        OrbEffect.CadastrarPerfil(new GISModel.Entidades.Perfil()
        //        {
        //            UniqueKey = Guid.Parse(UK),
        //            Nome = Nome,
        //            Descricao = Descricao,
        //            ActionDefault = Action,
        //            ControllerDefault = Controller,
        //            UsuarioInclusao = UsuarioInclusao
        //        });
        //    }
        //}

        //internal static void LoadUsuario(ExcelWorksheet ws, Orb OrbEffect)
        //{

        //    var start = ws.Dimension.Start;
        //    var end = ws.Dimension.End;
        //    for (int row = start.Row + 1; row <= end.Row; row++)
        //    {
        //        string UK = ws.Cells[row, 1].Text;
        //        string CPF = ws.Cells[row, 2].Text;
        //        string Nome = ws.Cells[row, 3].Text;
        //        string Login = ws.Cells[row, 4].Text;
        //        string Senha = ws.Cells[row, 5].Text;
        //        string Email = ws.Cells[row, 6].Text;
        //        string UKEmpresa = ws.Cells[row, 7].Text;
        //        string UKDepartamento = ws.Cells[row, 8].Text;
        //        string TipoAcesso = ws.Cells[row, 9].Text;
        //        string UsuarioInclusao = ws.Cells[row, 10].Text;

        //        OrbEffect.CadastrarUsuario(new GISModel.Entidades.Usuario()
        //        {
        //            UniqueKey = Guid.Parse(UK),
        //            CPF = CPF,
        //            Nome = Nome,
        //            Login = Login,
        //            Senha = Senha,
        //            Email = Email,
        //            UKEmpresa = Guid.Parse(UKEmpresa),
        //            UKDepartamento = Guid.Parse(UKDepartamento),
        //            TipoDeAcesso = (GISModel.Enums.TipoDeAcesso)Enum.Parse(typeof(GISModel.Enums.TipoDeAcesso), TipoAcesso, true),
        //            UsuarioInclusao = UsuarioInclusao
        //        });

        //    }

        //}

        //internal static void LoadUsuarioPerfil(ExcelWorksheet ws, Orb OrbEffect)
        //{

        //    var start = ws.Dimension.Start;
        //    var end = ws.Dimension.End;
        //    for (int row = start.Row + 1; row <= end.Row; row++)
        //    {
        //        string UK = ws.Cells[row, 1].Text;
        //        string UKUsuario = ws.Cells[row, 2].Text;
        //        string UKPerfil = ws.Cells[row, 3].Text;
        //        string UKConfig = ws.Cells[row, 4].Text;
        //        string UsuarioInclusao = ws.Cells[row, 5].Text;

        //        OrbEffect.CadastrarUsuarioPerfil(new GISModel.Entidades.UsuarioPerfil()
        //        {
        //            UniqueKey = Guid.Parse(UK),
        //            UKUsuario = Guid.Parse(UKUsuario),
        //            UKConfig = Guid.Parse(UKConfig),
        //            UKPerfil = Guid.Parse(UKPerfil),
        //            UsuarioInclusao = UsuarioInclusao
        //        });

        //    }

        //}


        #region Senhas

        [ComVisible(false)]
        private static string CreateHashFromPassword(string pstrOriginalPassword)
        {
            if (string.IsNullOrEmpty(pstrOriginalPassword))
                return string.Empty;

            string str3 = ConvertToHashedString(pstrOriginalPassword).Substring(0, 5);
            byte[] bytes = Encoding.UTF8.GetBytes(pstrOriginalPassword + str3);
            HashAlgorithm lobjHash = new MD5CryptoServiceProvider();
            return Convert.ToBase64String(lobjHash.ComputeHash(bytes));
        }

        [ComVisible(false)]
        private static string ConvertToHashedString(string pstrOriginal)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(pstrOriginal);
            HashAlgorithm lobjHash = new MD5CryptoServiceProvider();
            return Convert.ToBase64String(lobjHash.ComputeHash(bytes));
        }

        #endregion

    }
}
