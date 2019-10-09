using Gestor.Application;
using GISWeb.Infraestrutura.Provider.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GISWeb.Infraestrutura.Provider.Concrete
{
    internal class AuthenticatedUserResolver : IAuthenticatedUser
    {
        private readonly ICustomAuthorizationProvider customAuthorizationProvider;

        public string UserName => customAuthorizationProvider.UsuarioAutenticado?.Login;

        public string Email => customAuthorizationProvider.UsuarioAutenticado?.Email;

        public IEnumerable<IAuthenticatedUserRoleAssignment> RoleAssignments => customAuthorizationProvider?.UsuarioAutenticado?.Permissoes?.Select(p => new AuthenticatedUserRoleAssignment(p.Perfil, p.Config));

        public AuthenticatedUserResolver(ICustomAuthorizationProvider customAuthorizationProvider)
        {
            this.customAuthorizationProvider = customAuthorizationProvider ?? throw new ArgumentNullException(nameof(customAuthorizationProvider));
        }
    }
}