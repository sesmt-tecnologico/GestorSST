using GISModel.Entidades;
using System;
using System.Collections.Generic;

namespace GISModel.DTO.Permissoes
{
    public class UsuarioPerfilViewModel
    {

        public Guid IDUsuario { get; set; }

        public string Login { get; set; }

        public string Nome { get; set; }

        public List<Perfil> Perfis { get; set; }

    }
}
