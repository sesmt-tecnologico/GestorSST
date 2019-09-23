using GISModel.Entidades;
using System.Collections.Generic;

namespace GISModel.DTO.Permissoes
{
    public class UsuarioPerfilViewModel
    {

        public string IDUsuario { get; set; }

        public string Login { get; set; }

        public string Nome { get; set; }

        public List<Perfil> Perfis { get; set; }

    }
}
