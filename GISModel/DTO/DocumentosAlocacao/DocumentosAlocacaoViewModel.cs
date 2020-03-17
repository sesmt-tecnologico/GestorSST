using System;
using System.ComponentModel.DataAnnotations;

namespace GISModel.DTO.DocumentosAlocacao

{
    public class DocumentosAlocacaoViewModel:EntidadeBase
        

    {

        public Guid UniqueKey { get; set; }


        [Display(Name = "Alocação")]
        public Guid UKAlocacao { get; set; }


        [Display(Name ="Atividade")]
        public Guid UKATividade { get; set; }

        [Display(Name = "UniqueKey REL")]
        public Guid UniqueKeyREL { get; set; }

        [Display(Name ="Documento")]
        public Guid UKDocumento { get; set; }

        [Display(Name = "Documento")]
        public string  Nomedocumento { get; set; }

        [Display(Name = "Documento")]
        public DateTime Data { get; set; }


    }
}
