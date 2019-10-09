using System;
using System.Runtime.Serialization;

namespace Gestor.Domain.Exceptions
{
    [Serializable]
    public class SituacaoInvalidaParaFinalizacaoException : CoreException
    {
        public override string Key => "SituacaoInvalidaParaFinalizacao";
        public override string Message => "A situação em que se encontra este registro não permite sua finalização. Verifique os detalhes para mais informações.";
        public string Detalhes { get; }

        public SituacaoInvalidaParaFinalizacaoException() : base()
        {
        }

        public SituacaoInvalidaParaFinalizacaoException(string detalhes) : this()
        {
            Detalhes = detalhes;
        }

        protected SituacaoInvalidaParaFinalizacaoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}