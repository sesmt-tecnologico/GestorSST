using System;
using System.Runtime.Serialization;

namespace Gestor.Domain.Exceptions
{
    [Serializable]
    public class CampoNaoPodeSerNuloException : CoreException
    {
        public override string Key => "CampoNaoPodeSerNulo";
        public override string Message => "O campo do objeto não pode estar em branco.";
        public string Campo { get; }

        public CampoNaoPodeSerNuloException(string campo) : base()
        {
            Campo = campo;
        }

        protected CampoNaoPodeSerNuloException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}