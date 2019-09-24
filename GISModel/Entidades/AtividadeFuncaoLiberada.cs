using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbAtividadeFuncaoLiberada")]
   public class AtividadeFuncaoLiberada:EntidadeBase
    {

        public Guid IDFuncao { get; set; }

        public Guid IDAtividade { get; set; }

        public Guid IDAlocacao { get; set; }

        public virtual Funcao Funcao { get; set; }

        public virtual Atividade Atividade { get; set; }

        public virtual Alocacao Alocacao { get; set; }

    }
}
