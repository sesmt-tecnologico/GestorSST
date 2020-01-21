using GISModel.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("REL_ContratoFornecedor")]
    public class REL_ContratoFornecedor : EntidadeBase
    {

        [Display(Name = "Contrato")]
        public Guid UKContrato { get; set; }

        [Display(Name = "Fornecedor")]
        public Guid UKFornecedor { get; set; }


        public virtual Contrato Contrato { get; set; }

        public virtual Empresa Fornecedor { get; set; }

        [Display(Name = "Vinculo Contrato x Fornecedor")]
        public ETipoContratoFornecedor TipoContratoFornecedor { get; set; }

    }
}
