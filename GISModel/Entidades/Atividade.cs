using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbAtividade")]
    public class Atividade: EntidadeBase
    {

        [Display(Name ="Descrição da Atividade")]
        public string Descricao { get; set; }

        public List<DocumentosPessoal> DocumentosPessoal { get; set; }

    }
}
