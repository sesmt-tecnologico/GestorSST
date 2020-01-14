using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class AtividadeBusiness : BaseBusiness<Atividade>, IAtividadeBusiness
    {

        public override void Inserir(Atividade pAtividadeDeRisco)
        {
            if (Consulta.Any(u => u.ID.Equals(pAtividadeDeRisco.ID)))
                throw new InvalidOperationException("Não é possível inserir a Atividade, pois já existe uma Atividade com este ID.");

            base.Inserir(pAtividadeDeRisco);
        }

        public override void Alterar(Atividade pAtividade)
        {
            Atividade tempAtividade = Consulta.FirstOrDefault(p => p.UniqueKey.Equals(pAtividade.UniqueKey));

            if (tempAtividade == null)
            {
                throw new Exception("Não foi possível encontrar esta Atividade");
            }

            tempAtividade.DataExclusao = DateTime.Now;
            tempAtividade.UsuarioExclusao = pAtividade.UsuarioExclusao;
            base.Alterar(tempAtividade);

            pAtividade.ID = Guid.Empty;
            pAtividade.UniqueKey = tempAtividade.UniqueKey;
            pAtividade.UsuarioExclusao = string.Empty;
            pAtividade.UsuarioInclusao = tempAtividade.UsuarioExclusao;
            
                        
            base.Inserir(pAtividade);
            

        }

        public override void Excluir(Atividade entidade)
        {
            
            base.Excluir(entidade);
        }

    }
}
