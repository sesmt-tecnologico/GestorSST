using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{

    [Table("tbLink")]
    public class Link : EntidadeBase
    {

        public Guid UKObjeto { get; set; }

        public string Nome { get; set; }

        public string URL { get; set; }

    }


}
