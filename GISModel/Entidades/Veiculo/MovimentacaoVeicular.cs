using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades.Veiculo
{

    [Table("tbMovimentacaoVeicular")]
    public class MovimentacaoVeicular: EntidadeBase
    {
        [Display(Name = "Registro")]
        public Guid RegistroVeicular { get; set; }

        [Display(Name = "Veículo")]
        public string Veiculo { get; set; }

        [Display(Name = "Frota")]
        public string frota { get; set; }

        [Display(Name = "KM Saída")]
        public string KMSaida { get; set; }

        [Display(Name = "KM Chegada")]
        [Range(1,99999)]
        public string KMChegada { get; set; }

        [Display(Name = "Intinerário Origem")]
        public string IntinerarioOrigem { get; set; }

        [Display(Name = "Intinerário Destino")]
        public string IntinerarioDestino { get; set; }





    }
}
