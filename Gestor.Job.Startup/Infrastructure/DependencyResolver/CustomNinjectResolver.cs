using GISCore.DI;
using Ninject;

namespace Gestor.Job.Startup.Infrastructure.DependencyResolver
{
    public class CustomNinjectResolver
    {
        public static IKernel Inject()
        {
            return new StandardKernel(new GISNinjectModule());
        }

    }
}
