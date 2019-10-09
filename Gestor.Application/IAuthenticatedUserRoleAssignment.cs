namespace Gestor.Application
{
    public interface IAuthenticatedUserRoleAssignment
    {
        string Role { get; }

        string Configuration { get; }
    }
}