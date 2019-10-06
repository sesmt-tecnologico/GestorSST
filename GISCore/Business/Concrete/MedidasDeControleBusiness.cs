using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class MedidasDeControleBusiness:BaseBusiness<MedidasDeControleExistentes>, IMedidasDeControleBusiness
    {

        public override void Inserir(MedidasDeControleExistentes pMedidasDeControleExistentes)
        {
            base.Inserir(pMedidasDeControleExistentes);
        }

        public override void Alterar(MedidasDeControleExistentes pMedidasDeControleExistentes)
        {

            MedidasDeControleExistentes tempMedidasDeControleExistentes = Consulta.FirstOrDefault(p => p.ID.Equals(pMedidasDeControleExistentes.ID));
            if (tempMedidasDeControleExistentes == null)
            {
                throw new Exception("Não foi possível encontrar o Estabelecimento através do ID.");
            }
            else
            {
                tempMedidasDeControleExistentes.NomeDaImagem = tempMedidasDeControleExistentes.NomeDaImagem;
                tempMedidasDeControleExistentes.MedidasExistentes = tempMedidasDeControleExistentes.MedidasExistentes;
                tempMedidasDeControleExistentes.EClassificacaoDaMedida = tempMedidasDeControleExistentes.EClassificacaoDaMedida;
                tempMedidasDeControleExistentes.Imagem = tempMedidasDeControleExistentes.Imagem;

                base.Alterar(tempMedidasDeControleExistentes);
            }
        }

    }

}

