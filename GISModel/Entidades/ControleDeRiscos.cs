using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GISModel.Entidades
{
    public class ControleDeRiscos: EntidadeBase
    {

        [Display(Name = "Classifique o Controle")]
        public EClassificacaoDaMedia EClassificacaoDaMedia { get; set; }

        [Display(Name = "Eficacia")]
        public EControle EControle { get; set; }

        [Display(Name = "Descrição do Controle")]
        public string Descricao { get; set; }

    }
}
