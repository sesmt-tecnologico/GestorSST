using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{

    [Table("tbLink")]
    public class Link : EntidadeBase
    {

        public Guid UKObjeto { get; set; }


        [Required(ErrorMessage = "Informe o nome do link")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe a URL do link")]
        public string URL { get; set; }

    }


}
