using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.AtividadeGeradoraRisco
{
    public class VMAtividadeGeradoraRisco
    {
        public Guid UniqueKey { get; set; }

        [Display(Name = "Atividade")]
        public string UKAtividade { get; set; }

        [Display(Name = "Descrição da Atividade")]
        public string Descricao { get; set; }


        
    }
}
