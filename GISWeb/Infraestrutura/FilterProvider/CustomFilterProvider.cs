using GISWeb.Infraestrutura.DependencyResolver;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GISWeb.Infraestrutura.FilterProvider
{
    public class CustomFilterProvider : FilterAttributeFilterProvider
    {
        public override IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filtrosRetorno = new List<Filter>();

            var filters = base.GetFilters(controllerContext, actionDescriptor);
            var dependencyResolver = (NinjectDependencyResolver)System.Web.Mvc.DependencyResolver.Current;

            if (dependencyResolver != null)
            {
                foreach (var filter in filters)
                {
                    dependencyResolver.Kernel.Inject(filter.Instance);
                    filtrosRetorno.Add(filter);
                }

                //Adicionando Filtros Globais e injetando dependência neles
                foreach (var filter in GlobalFilters.Filters)
                {
                    dependencyResolver.Kernel.Inject(filter.Instance);
                    filtrosRetorno.Add(filter);
                }
            }

            return filtrosRetorno;
        }

    }
}