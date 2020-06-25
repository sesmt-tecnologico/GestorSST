using GISModel.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbTipoDeRisco")]
    public class TipoDeRisco: EntidadeBase
    {

        [Display(Name ="Descrição do evento Perigoso")]
        public Guid idPerigoPotencial { get; set; }        
        
        [Display(Name ="Possíveis Danos a Saúde")]
        public Guid idPossiveisDanos { get; set; }

        public Guid idEventoPerigoso { get; set; }

        [Display(Name = "Atividade do Estabelecimento")]
        public Guid idAtividadesDoEstabelecimento { get; set; }

        [Display(Name ="Atividade da Função")]
        public Guid idAtividade { get; set; }

        [Display(Name ="Classifique o Risco")]
        public EClasseDoRisco EClasseDoRisco { get; set; }

        [Display(Name = "Fonte Geradora")]
        public string FonteGeradora { get; set; }

        [Display(Name = "Tragetória")]
        public string Tragetoria { get; set; }

        [Display(Name ="Vincular")]
        public bool Vinculado { get; set; }
       
        public virtual EventoPerigoso EventoPerigoso { get; set; }

        public virtual PossiveisDanos PossiveisDanos { get; set; }

        public virtual ListaDePerigo ListaDePerigo { get; set; }

        public virtual AtividadesDoEstabelecimento AtividadesDoEstabelecimento {get; set;}

        public virtual Atividade Atividade { get; set; }

    }
}   
