using Gestor.Domain.Exceptions;
using System;

namespace Gestor.Domain.Entities
{
    public abstract class EntidadeBase
    {
        public Guid Id { get; set; }

        public string UsuarioInclusao { get; private set; }

        public DateTime DataInclusao { get; private set; }

        public string UsuarioTermino { get; private set; }

        public DateTime? DataTermino { get; private set; }

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

        public virtual void Terminar(string usuarioTermino)
        {
            if (string.IsNullOrWhiteSpace(usuarioTermino))
                throw new CampoNaoPodeSerNuloException(nameof(usuarioTermino));

            ValidarTerminoDaEntidade();

            UsuarioTermino = usuarioTermino;
            DataTermino = DateTime.Now;
        }

        protected abstract void ValidarTerminoDaEntidade();
    }
}