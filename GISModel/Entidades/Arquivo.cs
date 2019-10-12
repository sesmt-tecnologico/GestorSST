using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{

    [Table("tbArquivo")]
    public class Arquivo : EntidadeBase
    {

        public Guid UKObjeto { get; set; }

        public string NomeLocal { get; set; }

        public string Extensao { get; set; }

        public string NomeRemoto { get; set; }

        [NotMapped]
        public byte[] Conteudo { get; set; }

    }
}
