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

        //public override void Inserir(WorkArea pWorkArea)
        //{
            
        //    base.Inserir(pWorkArea);
        //}

        public override void Alterar(WorkArea pWorkArea)
        {

                       
            WorkArea tempWorkArea = Consulta.FirstOrDefault(p => p.ID.Equals(pWorkArea.ID));
            if (tempWorkArea == null)
            {
                throw new Exception("Não foi possível encontrar a WorkArea através do ID.");
            }
            
                tempWorkArea.Nome = pWorkArea.Nome;
                tempWorkArea.Descricao = pWorkArea.Descricao;               
                base.Alterar(tempWorkArea);

                tempWorkArea.UsuarioExclusao = pWorkArea.UsuarioExclusao;
                base.Terminar(tempWorkArea);

                pWorkArea.ID = Guid.NewGuid();
                pWorkArea.UniqueKey = tempWorkArea.UniqueKey;
                pWorkArea.Nome = tempWorkArea.Nome;
                pWorkArea.Descricao = tempWorkArea.Descricao;
                pWorkArea.UsuarioExclusao = string.Empty;
                base.Inserir(pWorkArea);

           

        }

    }


}

