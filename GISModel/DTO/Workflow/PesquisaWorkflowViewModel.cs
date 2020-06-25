using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.Workflow
{
    public class PesquisaWorkflowViewModel
    {
        public Guid IDWorkflow { get; set; }

        public string UsuarioInclusao { get; set; }

        public DateTime  Data { get; set; }

        public int status { get; set; }

        public string Comentarios { get; set; }

        public string Documento { get; set; }

    }
}
