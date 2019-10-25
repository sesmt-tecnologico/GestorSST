using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    public class REL_WorkAreaAtividade: EntidadeBase
    {

        public Guid IDEstabelecimento { get; set; }

        public Guid IDWorkArea { get; set; }

        public Guid IDAtividade { get; set; }

        public virtual Estabelecimento Estabelecimento { get; set; }

        public virtual WorkArea WorkArea { get; set; }

        public virtual Atividade Atividade { get; set; }


    }
}
