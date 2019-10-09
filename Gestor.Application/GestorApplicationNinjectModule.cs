using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace Gestor.Application
{
    public class GestorApplicationNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind(c => c.FromThisAssembly().IncludingNonPublicTypes().SelectAllClasses().BindDefaultInterfaces());
        }
    }
}