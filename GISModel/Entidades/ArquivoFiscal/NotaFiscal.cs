using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades.ArquivoFiscal
{

    [Table("tbNotaFiscal")]
    public class NotaFiscal: EntidadeBase
    {
        [Display(Name ="Serviço")]
        public bool Servico { get; set; }

        [Display(Name = "Material")]
        public bool Material { get; set; }

        [Display(Name = "Material e Serviço")]
        public bool ServicoMaterial { get; set; }

        [Display(Name = "Entrada")]
        public bool Entrada { get; set; }

        [Display(Name = "Saída")]
        public bool Saida { get; set; }

        [Display(Name = "Vencimento da Nota")]
        public DateTime Vencimento { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Fornecedor")]
        public string Fornecedor { get; set; }

        [Display(Name = "Número da Nota")]
        public int Numero { get; set; }

        [Display(Name = "Valor da Nota")]
        public float Valor { get; set; }


    }
}
