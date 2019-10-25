using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.WorkArea
{
    public class REL_WorkAreaAtividadeViewModel
    {
        [Display(Name ="Estabelecimento")]
        public Guid IDEstabelecimento { get; set; }

        [Display(Name ="Atividade")]
        public Guid IDAtividade { get; set; }

        [Display(Name ="WorkArea")]
        public Guid IDWorkArea { get; set; }

    }
}
