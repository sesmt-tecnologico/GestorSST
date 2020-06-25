using GISModel.Enums;
using System;
using System.Collections.Generic;

namespace GISModel.DTO.PPRA
{
    public class VMListaMedicoes
    {


        public string WorkArea { get; set; }

        public string FonteGeradora { get; set; }

        public  ETipoMedicoes TipoMedicao { get; set; }

        public string ValorMedicoes { get; set; }

        public string MaxExposicao { get; set; }

        public string Observacao { get; set; }

        public string UsuarioInclusao { get; set; }

        public DateTime DataInclusao { get; set; }



        // public List<Entidades.FonteGeradoraDeRisco> FontesGeradoras { get; set; }

    }
}
