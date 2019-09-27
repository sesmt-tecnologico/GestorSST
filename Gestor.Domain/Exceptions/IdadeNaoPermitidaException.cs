using System;
using System.Runtime.Serialization;

namespace Gestor.Domain.Exceptions
{
    [Serializable]
    public class IdadeNaoPermitidaException : CoreException
    {
        public override string Key => "IdadeNaoPermitida";
        public override string Message => "A idade informada não é permitida para cadastro na base de dados.";

        public IdadeNaoPermitidaException() : base()
        {
        }

        protected IdadeNaoPermitidaException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}