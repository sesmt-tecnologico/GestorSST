using System;
using System.Runtime.Serialization;

namespace Gestor.Domain.Exceptions
{
    [Serializable]
    public class EmpregadoNaoEstaAdmitidoException : CoreException
    {
        public override string Key => "EmpregadoNaoEstaAdmitido";
        public override string Message => "O empregado não está com status Admitido.";

        public EmpregadoNaoEstaAdmitidoException() : base()
        {
        }

        protected EmpregadoNaoEstaAdmitidoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}