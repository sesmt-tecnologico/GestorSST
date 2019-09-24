using System;

namespace GISModel.DTO.Permissoes
{
    public class VMPermissao
    {

        public Guid UKUsuario { get; set; }

        public string Usuario { get; set; }


        public Guid UKPerfil { get; set; }

        public string Perfil { get; set; }


        public Guid UKConfig { get; set; }

        public string Config { get; set; }

    }
}
