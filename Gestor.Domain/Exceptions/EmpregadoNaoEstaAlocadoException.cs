using System;
using System.Runtime.Serialization;

namespace Gestor.Domain.Exceptions
{
    [Serializable]
    public class EmpregadoNaoEstaAlocadoException : CoreException
    {
        public override string Key => "EmpregadoNaoEstaAlocado";
        public override string Message => "O empregado não está com status Alocado.";

        public EmpregadoNaoEstaAlocadoException() : base()
        {
        }

        protected EmpregadoNaoEstaAlocadoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}