using GISModel.Entidades.Estoques;
using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.EPI
{
    public class FichaPadraoViewModel
    {

        private string _Produto = "Capacete";
        public string Produto {
            get => Produto;

            set
            {
                _Produto = value;

            }
        }
       
    }
}
