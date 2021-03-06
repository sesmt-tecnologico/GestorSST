﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbUsuarioPerfil")]
    public class UsuarioPerfil : EntidadeBase
    {

        [Required(ErrorMessage = "Selecione um usuário")]
        public Guid UKUsuario { get; set; }

        public virtual Usuario Usuario { get; set; }



        [Required(ErrorMessage = "Selecione um perfil")]
        public Guid UKPerfil { get; set; }

        public virtual Perfil Perfil { get; set; }



        [Required(ErrorMessage = "Selecione um órgão")]
        public Guid UKConfig { get; set; }

    }
}
