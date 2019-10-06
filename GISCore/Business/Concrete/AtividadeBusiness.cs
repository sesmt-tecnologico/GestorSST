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

        public override void Alterar(Atividade pAtividadeDeRisco)
        {
            Atividade tempAtividadeDeRisco = Consulta.FirstOrDefault(p => p.ID.Equals(pAtividadeDeRisco.ID));

            if (tempAtividadeDeRisco == null)
            {
                throw new Exception("Não foi possível encontrar esta Atividade");
            }

            tempAtividadeDeRisco.Descricao = pAtividadeDeRisco.Descricao;
            

            base.Alterar(tempAtividadeDeRisco);

        }

    }
}
