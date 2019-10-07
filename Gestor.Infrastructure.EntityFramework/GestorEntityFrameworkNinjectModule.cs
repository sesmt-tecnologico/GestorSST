using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace Gestor.Infrastructure.EntityFramework
{
    public class GestorEntityFrameworkNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind(c => c.FromThisAssembly().IncludingNonPublicTypes().SelectAllClasses().BindDefaultInterfaces());
        }
    }
}