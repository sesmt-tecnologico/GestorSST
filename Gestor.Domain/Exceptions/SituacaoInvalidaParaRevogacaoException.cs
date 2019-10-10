using System;
using System.Runtime.Serialization;

namespace Gestor.Domain.Exceptions
{
    [Serializable]
    public class SituacaoInvalidaParaRevogacaoException : CoreException
    {
        public override string Key => "SituacaoInvalidaParaRevogacao";
        public override string Message => "A situação em que se encontra este registro não permite sua revogação. Verifique os detalhes para mais informações.";
        public string Detalhes { get; }

        public SituacaoInvalidaParaRevogacaoException() : base()
        {
        }

        public SituacaoInvalidaParaRevogacaoException(string detalhes) : this()
        {
            Detalhes = detalhes;
        }

        protected SituacaoInvalidaParaRevogacaoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}