using Gestor.Domain.Exceptions;
using System;

namespace Gestor.Domain.Entities
{
    public abstract class EntidadeBase
    {
        public Guid Id { get; set; }

        public string UsuarioInclusao { get; private set; }

        public DateTime DataInclusao { get; private set; }

        public string UsuarioExclusao { get; private set; }

        public DateTime? DataExclusao { get; private set; }

        protected EntidadeBase()
        {
        }

        protected EntidadeBase(string usuarioInclusao)
        {
            if (string.IsNullOrWhiteSpace(usuarioInclusao))
                throw new CampoNaoPodeSerNuloException(nameof(usuarioInclusao));

            Id = Guid.NewGuid();
            UsuarioInclusao = usuarioInclusao;
            DataInclusao = DateTime.Now;
        }

        protected abstract void ValidarExclusao();

        public void Excluir(string usuarioExclusao)
        {
            if (string.IsNullOrWhiteSpace(usuarioExclusao))
                throw new CampoNaoPodeSerNuloException(nameof(usuarioExclusao));

            ValidarExclusao();

            UsuarioExclusao = usuarioExclusao;
            DataExclusao = DateTime.Now;
        }
    }
}