using GISModel.Entidades.PCMSO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbPerigo")]
    public class Perigo : EntidadeBase
    {

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Informe a descrição do perigo")]
        public string Descricao { get; set; }

        public bool Template { get; set; }


        public List<Risco> Riscos { get; set; }

        public List<Exames> Exames { get; set; }

    }
}