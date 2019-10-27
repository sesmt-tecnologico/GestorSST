using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.WorkArea
{
   public class PesquisaWorkAreaViewModel
    {
        [Display(Name = "UniqueKey")]
        public Guid UniqueKey { get; set; }

        [Display(Name = "IDEstabelecimento")]
        public Guid IDEstabelecimento { get; set; }

        [Display(Name ="IDWorkarea")]
        public Guid IDWorkarea { get; set; }

        [Display(Name = "Nome da WorkArea")]
        public string Nome { get; set; }

        [Display(Name = "Descrição WorkArea")]
        public string Descricao { get; set; }        

        [Display(Name = "Nome")]
        public string NomeCompleto { get; set; }

        [Display(Name = "Atividades")]
        public string Atividade { get; set; }
    }
}
