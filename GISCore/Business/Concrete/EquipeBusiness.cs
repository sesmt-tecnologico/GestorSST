using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class EquipeBusiness : BaseBusiness<Equipe>, IEquipeBusiness
    {

        public override void Alterar(Equipe pEquipe)
        {

            Equipe tempEquipe = Consulta.FirstOrDefault(p => p.ID.Equals(pEquipe.ID));
            if (tempEquipe == null)
            {
                throw new Exception("Não foi possível encontrar esta Equipe");
            }
            else
            {
                tempEquipe.NomeDaEquipe = pEquipe.NomeDaEquipe;
                tempEquipe.ResumoAtividade = pEquipe.ResumoAtividade;

                base.Alterar(tempEquipe);
            }

        }

    }
}
