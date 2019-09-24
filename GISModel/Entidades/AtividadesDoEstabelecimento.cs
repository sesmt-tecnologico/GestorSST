using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbAtividadesDoEstabelecimento")]
    public class AtividadesDoEstabelecimento: EntidadeBase
    {
        

        [Display(Name ="Ativar")]
        public string Ativo { get; set; }


        [Display(Name ="Descrição desta Atividade")]
        public string DescricaoDestaAtividade { get; set; }


        [Display(Name ="Nome da Imagem ")]
        public string NomeDaImagem { get; set; }

        [Display(Name ="Imagem")]
        public string Imagem { get; set; }        

        [Display(Name = "Ambientes do Estabelecimento")]
        public Guid IDEstabelecimentoImagens { get; set; }

        [Display(Name = "Estabelecimento")]
        public Guid IDEstabelecimento { get; set; }

        [Display(Name = "Possiveis Danos")]
        public Guid IDPossiveisDanos { get; set; }

        [Display(Name = "Evento Perigoso")]
        public Guid IDEventoPerigoso { get; set; }

        [Display(Name = "Alocacao")]
        public Guid IDAlocacao { get; set; }

        public virtual EstabelecimentoAmbiente EstabelecimentoImagens { get; set; }

        public virtual Estabelecimento Estabelecimento { get; set; }

        public virtual PossiveisDanos PossiveisDanos { get; set; }

        public virtual EventoPerigoso EventoPerigoso { get; set; }

    }
}
