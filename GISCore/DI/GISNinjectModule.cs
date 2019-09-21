using Ninject.Modules;
using Ninject.Extensions.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.DI
{
    public class GISNinjectModule : NinjectModule
    {
        public override void Load()
        {
            AddBindingsGenerics();
        }

        private void AddBindingsGenerics()
        {
            Kernel.Bind(c => c.FromThisAssembly()
                .SelectAllClasses()
                .BindDefaultInterfaces());
            
        }

    }
}
