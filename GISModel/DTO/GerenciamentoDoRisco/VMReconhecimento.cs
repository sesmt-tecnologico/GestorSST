using GISModel.Entidades;
using System.Collections.Generic;

namespace GISModel.DTO.GerenciamentoDoRisco
{
    public class VMReconhecimento
    {

        public string UKWorkArea { get; set; }

        public string  UKAtividade { get; set; }       

        public string AtivDescricao { get; set; }

        public string WorkArea { get; set; }

       

        public List<Entidades.FonteGeradoraDeRisco> FontesGeradoras { get; set; }

        public List<Perigo> Perigo { get; set; }

    }
}
