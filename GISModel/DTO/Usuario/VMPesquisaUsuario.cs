using System;
using System.ComponentModel.DataAnnotations;

namespace GISModel.DTO.Usuario
{
    public class VMPesquisaUsuario
    {


        public string UniqueKey { get; set; }


        [Display(Name = "CPF")]
        public string CPF { get; set; }

        
        public string Nome { get; set; }


        [Display(Name = "E-mail")]
        public string Email { get; set; }


        [Display(Name = "Tipo de Acesso")]
        public string TipoAcesso { get; set; }


        [Display(Name = "Período de Criação")]
        public string DataCriacao { get; set; }


        [Display(Name = "Empresa")]
        public string UKEmpresa { get; set; }


        [Display(Name = "Departamento")]
        public string UKDepartamento { get; set; }


    }
}
