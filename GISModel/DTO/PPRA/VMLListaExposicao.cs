using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.PPRA
{
    public class VMLListaExposicao
    {
        public EExposicaoInsalubre EExposicaoInsalubre { get; set; }

        public EExposicaoCalor EExposicaoCalor { get; set; }

        public EExposicaoSeg EExposicaoSeg { get; set; }

        public EGravidade EGravidade { get; set; }

        public string Estabelecimento { get; set; }

        public string  Workarea { get; set; }

        public string FonteGeradora { get; set; }

        public string Risco { get; set; }

        public string Observacao { get; set; }

        public string UsuarioInclusao { get; set; }

        public DateTime DataInclusao { get; set; }
    }
}
