using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbAtividadeFuncaoLiberada")]
   public class AtividadeFuncaoLiberada:EntidadeBase
    {

        public string IDFuncao { get; set; }

        public string IDAtividade { get; set; }

        public string IDAlocacao { get; set; }

        public virtual Funcao Funcao { get; set; }

        public virtual Atividade Atividade { get; set; }

        public virtual Alocacao Alocacao { get; set; }

    }
}
