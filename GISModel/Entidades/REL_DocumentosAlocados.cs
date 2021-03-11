using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("REL_DocumentoAlocacao")]
    public class REL_DocumentosAlocados : EntidadeBase
    {
        public Guid UKAlocacao { get; set; }

        public Guid UKDocumento { get; set; }

        public bool status { get; set; }

        public int Posicao { get; set; }


        [Display(Name = "Data do Documento")]
        [DisplayFormat(ApplyFormatInEditMode = true,
         DataFormatString = "{0:dd-MM-yyyy}")]        
        public DateTime DataDocumento { get; set; }


    }
}
