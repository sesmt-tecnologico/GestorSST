using GISModel.Entidades;
using System.Collections.Generic;

namespace GISCore.Business.Abstract
{
    public interface IDepartamentoBusiness : IBaseBusiness<Departamento>
    {

        List<NivelHierarquico> BuscarNiveis();

    }
}
