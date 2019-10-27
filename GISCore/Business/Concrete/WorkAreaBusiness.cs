using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class WorkAreaBusiness: BaseBusiness<WorkArea>, IWorkAreaBusiness
    {

        public override void Inserir(WorkArea pWorkArea)
        {
            
            base.Inserir(pWorkArea);
        }

        public override void Alterar(WorkArea pWorkArea)
        {
            WorkArea tempWorkArea = Consulta.FirstOrDefault(p => p.UniqueKey.Equals(pWorkArea.UniqueKey));
            if (tempWorkArea == null)
            {
                throw new Exception("Não foi possível encontrar a WorkArea através do ID.");
            }
            else
            {
                tempWorkArea.Nome = pWorkArea.Nome;
                tempWorkArea.Descricao = pWorkArea.Descricao;
               
                base.Alterar(tempWorkArea);
            }

        }

    }


}

