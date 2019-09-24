using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace GISCore.Repository.Configuration
{
    public class InitializerBanco : DropCreateDatabaseIfModelChanges<SESTECContext>
    {
        protected override void Seed(SESTECContext context)
        {
            //criar alguns dados no banco

            new List<Empregado>
            {
                new Empregado
                {
                    ID = new Guid(),
                    CPF ="24547551812",
                    Nome ="Antonio Heriques Pereira",
                    DataNascimento = DateTime.Now
                },
                new Empregado
                {
                    ID = new Guid(),
                    CPF ="24547551812",
                    Nome ="Gabriel Henriques Pereira",
                    DataNascimento = DateTime.Now,
                }
            }.ForEach(p => context.Empregado.Add(p));

            base.Seed(context);
        }
    }
}
