using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GISModel.Entidades;

namespace GISModel.DTO.Ged
{
    public class GedViewModel
    {
        public Guid UKEmpregado { get; set; }

        public Empregado Empregado { get; set; }
    }
}
