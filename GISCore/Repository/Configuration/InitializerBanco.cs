using System.Data.Entity;

namespace GISCore.Repository.Configuration
{
    public class InitializerBanco : DropCreateDatabaseIfModelChanges<SESTECContext>
    {
        protected override void Seed(SESTECContext context)
        {
            base.Seed(context);
        }
    }
}
