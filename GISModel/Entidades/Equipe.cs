using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbEquipe")]
    public class Equipe: EntidadeBase
    {

        [Display(Name ="Nome da Equipe")]
        public string NomeDaEquipe { get; set; }

        [Display(Name = "Resumo da Atividade")]
        public string ResumoAtividade { get; set; }

        public Guid EmpresaID { get; set; }

        public virtual Empresa Empresa { get; set; }

       

    }
}
