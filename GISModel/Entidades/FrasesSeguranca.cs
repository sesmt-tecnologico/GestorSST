using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("tbFraseSeguranca")]
    public class FrasesSeguranca: EntidadeBase
    {
        [Display(Name ="Descrição da Frase")]
        public string Descricao { get; set; }

        [Display(Name = "Categoria")]
        public  ECategoriaFrases Categoria { get; set; }
        
        public Situacao Status { get; set; }
    }
}
