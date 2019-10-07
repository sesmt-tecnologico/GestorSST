using System.Configuration;

namespace Gestor.Infrastructure.EntityFramework
{
    internal class GestorEntityFrameworkConfiguration
    {
        public static string Database 
        { 
            get 
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["Infrastructure:Database"]))
                    return ConfigurationManager.AppSettings["Infrastructure:Database"];
                else
                    return "SESTECConection";
            }
        }
    }
}