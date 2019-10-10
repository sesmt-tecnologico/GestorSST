using System;
using System.Runtime.Serialization;

namespace Gestor.Domain.Exceptions
{
    [Serializable]
    public class SituacaoInvalidaParaAprovacaoException : CoreException
    {
        public override string Key => "SituacaoInvalidaParaAprovacao";
        public override string Message => "A situação em que se encontra este registro não permite sua aprovação. Verifique os detalhes para mais informações.";
        public string Detalhes { get; }

        public SituacaoInvalidaParaAprovacaoException() : base()
        {
        }

        public SituacaoInvalidaParaAprovacaoException(string detalhes) : this()
        {
            Detalhes = detalhes;
        }

        protected SituacaoInvalidaParaAprovacaoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}