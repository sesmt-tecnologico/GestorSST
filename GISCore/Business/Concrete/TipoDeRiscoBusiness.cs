using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class TipoDeRiscoBusiness: BaseBusiness<TipoDeRisco>, ITipoDeRiscoBusiness
    {

        public override void Inserir(TipoDeRisco pTipoDeRisco)
        {     
            base.Inserir(pTipoDeRisco);
        }

        public override void Alterar(TipoDeRisco oTipoDeRisco)
        {
            TipoDeRisco tempTipoDeRisco = Consulta.FirstOrDefault(p => p.ID.Equals(oTipoDeRisco.ID));

            if (tempTipoDeRisco == null)
            {
                throw new Exception("não foi possível encontrar este Tipo de Risco!");
            }
            
            tempTipoDeRisco.idPerigoPotencial = oTipoDeRisco.idPerigoPotencial;
            tempTipoDeRisco.PossiveisDanos = oTipoDeRisco.PossiveisDanos;

            base.Alterar(tempTipoDeRisco);
        }

        public override void Excluir(TipoDeRisco pTipoDeRisco)
        {
            base.Alterar(pTipoDeRisco);
        }

    }
}
