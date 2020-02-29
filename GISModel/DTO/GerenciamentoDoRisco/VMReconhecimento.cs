using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.GerenciamentoDoRisco
{
    public class VMReconhecimento
    {

        public string WorkArea { get; set; }

        public string FonteGeradora { get; set; }

        public List<Perigo> Perigos { get; set; }



    }
}
