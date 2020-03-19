using GISModel.DTO.Conta;
using GISModel.Entidades;

namespace GISCore.Business.Abstract
{
    public interface IUsuarioBusiness : IBaseBusiness<Usuario>
    {

        AutenticacaoModel ValidarCredenciais(AutenticacaoModel autenticacaoModel);

        void DefinirSenha(NovaSenhaViewModel novaSenhaViewModel);

        void SolicitarAcesso(string email);

        byte[] RecuperarAvatar(string login);

        void SalvarAvatar(string login, string imageStringBase64, string extensaoArquivo);

        string CreateHashFromPassword(string pstrOriginalPassword);

    }
}
