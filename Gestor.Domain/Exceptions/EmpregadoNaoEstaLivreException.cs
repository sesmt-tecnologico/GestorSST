using System;
using System.Runtime.Serialization;

namespace Gestor.Domain.Exceptions
{
    [Serializable]
    public class EmpregadoNaoEstaLivreException : CoreException
    {
        public override string Key => "EmpregadoNaoEstaLivre";
        public override string Message => "O empregado não está com status Livre.";

        public EmpregadoNaoEstaLivreException() : base()
        {
        }

        protected EmpregadoNaoEstaLivreException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}