using GISCore.Business.Abstract;
using GISModel.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class AtividadeAlocadaBusiness : BaseBusiness<AtividadeAlocada>, IAtividadeAlocadaBusiness
    {

        public override void Inserir(AtividadeAlocada pAtividadeAlocada)
        {
            base.Inserir(pAtividadeAlocada);
        }

        public override void Alterar(AtividadeAlocada pAtividadeAlocada)
        {
            List<AtividadeAlocada> lAtividadeAlocada = Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.idAlocacao.Equals(pAtividadeAlocada.idAlocacao) && p.idAtividadesDoEstabelecimento.Equals(pAtividadeAlocada.idAtividadesDoEstabelecimento)).ToList();
            if (lAtividadeAlocada.Count.Equals(1))
            {
                AtividadeAlocada oAtividadeAlocada = lAtividadeAlocada[0];

                oAtividadeAlocada.UsuarioExclusao = pAtividadeAlocada.UsuarioExclusao;
                oAtividadeAlocada.DataExclusao = pAtividadeAlocada.DataExclusao;

                base.Alterar(oAtividadeAlocada);
            }
        }

    }
}
