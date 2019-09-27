using System;
using System.Runtime.Serialization;

namespace Gestor.Domain.Exceptions
{
    [Serializable]
    public class RecursoNaoEncontradoException : CoreException
    {
        public override string Key => "RecursoNaoEncontrado";
        public override string Message => "O recurso requisitado não foi encontrado na base de dados.";
        public string Recurso { get; }

        public RecursoNaoEncontradoException(string recurso) : base()
        {
            Recurso = recurso;
        }

        protected RecursoNaoEncontradoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}