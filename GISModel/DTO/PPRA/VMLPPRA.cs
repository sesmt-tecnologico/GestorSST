using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.PPRA
{
    class VMLPPRA
    {
        public string UKWorkArea { get; set; }

        public string WorkArea { get; set; }



        public List<Entidades.FonteGeradoraDeRisco> FontesGeradoras { get; set; }

    }
}
