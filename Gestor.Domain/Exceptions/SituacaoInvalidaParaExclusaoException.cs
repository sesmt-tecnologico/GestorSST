using System;
using System.Runtime.Serialization;

namespace Gestor.Domain.Exceptions
{
    [Serializable]
    public class SituacaoInvalidaParaExclusaoException : CoreException
    {
        public override string Key => "SituacaoInvalidaParaExclusao";
        public override string Message => "A situação em que se encontra este registro não permite sua exclusão. Verifique os detalhes para mais informações.";
        public string Detalhes { get; }

        public SituacaoInvalidaParaExclusaoException() : base()
        {
        }

        public SituacaoInvalidaParaExclusaoException(string detalhes) : this()
        {
            Detalhes = detalhes;
        }

        protected SituacaoInvalidaParaExclusaoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}