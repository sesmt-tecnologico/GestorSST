using System;

namespace Gestor.Domain.ViewModels
{
    public class CadastrarResult
    {
        public Guid Id { get; }
        public string Codigo { get; }

        public CadastrarResult(Guid id)
        {
            Id = id;
        }

        public CadastrarResult(Guid id, string codigo) : this(id)
        {
            Codigo = codigo;
        }
    }
}