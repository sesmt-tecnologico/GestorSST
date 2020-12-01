using GISCore.Business.Abstract;
using GISModel.Entidades;
using Ninject;

namespace Gestor.Job.Startup.Infrastructure
{
    internal class Orb
    {
       // private IAtividadeBusiness _AtividadeBusiness { get; set; }

       private IBaseBusiness<REL_AtividadeEquipe> _AtividadeEquipeBusiness { get; set; }

        //private IEmpresaBusiness _EmpresaBusiness { get; set; }

        //private INivelHierarquicoBusiness _NivelHierarquicoBusiness { get; set; }

        //private IDepartamentoBusiness _DepartamentoBusiness { get; set; }

        //private IUsuarioBusiness _UsuarioBusiness { get; set; }

        //private IPerfilBusiness _PerfilBusiness { get; set; }

        //private IUsuarioPerfilBusiness _UsuarioPerfilBusiness { get; set; }

        public Orb(IKernel kernel)
        {

           // _AtividadeBusiness = kernel.Get<IAtividadeBusiness>();
           _AtividadeEquipeBusiness = kernel.Get<IBaseBusiness<REL_AtividadeEquipe>>();
            //_EmpresaBusiness = kernel.Get<IEmpresaBusiness>();
            //_NivelHierarquicoBusiness = kernel.Get<INivelHierarquicoBusiness>();
            //_DepartamentoBusiness = kernel.Get<IDepartamentoBusiness>();
            //_UsuarioBusiness = kernel.Get<IUsuarioBusiness>();
            //_PerfilBusiness = kernel.Get<IPerfilBusiness>();
            //_UsuarioPerfilBusiness = kernel.Get<IUsuarioPerfilBusiness>();
        }




        internal void CadastrarAtividadeEquipe(REL_AtividadeEquipe entidade)
        {
            try
            {
                _AtividadeEquipeBusiness.Inserir(entidade);
            }
            catch { }
        }

        //internal void CadastrarAtividade(Atividade entidade)
        //{
        //    try
        //    {
        //        _AtividadeBusiness.Inserir(entidade);
        //    }
        //    catch { }
        //}

        //internal void CadastrarEmpresa(Empresa entidade)
        //{
        //    try
        //    {
        //        _EmpresaBusiness.Inserir(entidade);
        //    }
        //    catch { }
        //}


        //internal void CadastrarNivelHierarquico(NivelHierarquico entidade)
        //{
        //    try
        //    {
        //        _NivelHierarquicoBusiness.Inserir(entidade);
        //    }
        //    catch { }

        //}

        //internal void CadastrarDepartamento(Departamento entidade)
        //{
        //    try
        //    {
        //        _DepartamentoBusiness.Inserir(entidade);
        //    }
        //    catch { }            
        //}

        //internal void CadastrarUsuario(Usuario entidade)
        //{
        //    try
        //    {
        //        _UsuarioBusiness.Inserir(entidade);
        //    }
        //    catch { }            
        //}

        //internal void CadastrarPerfil(Perfil entidade)
        //{
        //    try
        //    {
        //        _PerfilBusiness.Inserir(entidade);
        //    }
        //    catch { }            
        //}

        //internal void CadastrarUsuarioPerfil(UsuarioPerfil entidade)
        //{
        //    try
        //    {
        //        _UsuarioPerfilBusiness.Inserir(entidade);
        //    }
        //    catch { }            
        //}

    }
}
