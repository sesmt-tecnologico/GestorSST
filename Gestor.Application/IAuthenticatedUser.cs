using System.Collections.Generic;

namespace Gestor.Application
{
    public interface IAuthenticatedUser
    {
        string UserName { get; }

        string Email { get; }

        IEnumerable<IAuthenticatedUserRoleAssignment> RoleAssignments { get; } 
    }
}