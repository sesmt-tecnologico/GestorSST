using Gestor.Application;

namespace GISWeb.Infraestrutura.Provider.Concrete
{
    internal class AuthenticatedUserRoleAssignment : IAuthenticatedUserRoleAssignment
    {
        public string Role { get; }

        public string Configuration { get; }

        public AuthenticatedUserRoleAssignment(string role, string configuration)
        {
            Role = role;
            Configuration = configuration;
        }
    }
}