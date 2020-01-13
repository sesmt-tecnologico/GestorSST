using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbDocumentosPessoal")]
    public class DocumentosPessoal: EntidadeBase
    {

        [Display(Name ="Nome do Documento")]
        public string NomeDocumento { get; set; }

        [Display(Name ="Descrição")]
        public string DescriçãoDocumento { get; set; }

        [Display(Name ="Validade em meses")]
        [Required(ErrorMessage ="Insira o total de meses - de 0 a 12",AllowEmptyStrings =false)]
        public int Validade { get; set; }

        [Display(GroupName ="Atulização da validade")]
        public string ApartirDe { get; set; }

        [Display(GroupName = "Fim desta atualização")]
        public string FimDE { get; set; }



    }
}
