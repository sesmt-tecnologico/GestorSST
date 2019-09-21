using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class PerfilBusiness : BaseBusiness<Perfil>, IPerfilBusiness
    {

        public override void Inserir(Perfil Perfil)
        {
            Perfil.Nome = Perfil.Nome.Trim();

            if (Consulta.Any(u => u.Nome.Equals(Perfil.Nome)))
                throw new InvalidOperationException("Não é possível inserir o perfil, pois já existe um perfil cadastro com este nome.");

            Perfil.UniqueKey = Guid.NewGuid().ToString();

            base.Inserir(Perfil);
        }

    }
}
