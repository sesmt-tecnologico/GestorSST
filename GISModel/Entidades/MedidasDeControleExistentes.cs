using GISModel.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GISModel.Entidades
{
    public class MedidasDeControleExistentes: EntidadeBase
    {

        [Display(Name = "Medida de Controle Existente")]
        public string MedidasExistentes { get; set; }

        [Display(Name ="Classificação do Controle")]
        public EClassificacaoDaMedia EClassificacaoDaMedida { get; set; }

        [Display(Name = "Nome da Imagem")]
        public string NomeDaImagem { get; set; }

        [Display(Name = "Imagem")]
        public string Imagem { get; set; }

        [Display(Name = "Controle do Agente Ambiental")]
        public EControle EControle { get; set; }

        [Display(Name ="Tipo de Risco")]
        public Guid IDTipoDeRisco { get; set; }

        public virtual TipoDeRisco TipoDeRisco { get; set; }

    }
}
