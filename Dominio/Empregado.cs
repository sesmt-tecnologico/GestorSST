using GISModel.DTO;
using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    [Table("tbEmpregado")]
    public class Empregado : EntidadeBase
    {
                
        public string CPF { get; set; }
        
        public string Nome { get; set; }
        
        public string DataNascimento { get; set; }
        
        public EGenero Genero { get; set; }
        
        public string Email { get; set; }

        public string Status { get; set; }
    }
}
