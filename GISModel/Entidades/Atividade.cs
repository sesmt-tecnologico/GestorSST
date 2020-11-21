using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbAtividade")]
    public class Atividade: EntidadeBase
    {
        [Display(Name = "Código da Atividade")]
        public string Codigo { get; set; }

        [Display(Name ="Descrição da Atividade")]
        public string Descricao { get; set; }

        public List<DocumentosPessoal> DocumentosPessoal { get; set; }

        public List<Perigo> Perigos { get; set; }

    }
}
