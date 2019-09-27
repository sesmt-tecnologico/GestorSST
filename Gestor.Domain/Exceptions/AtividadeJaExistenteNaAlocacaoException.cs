using System;
using System.Runtime.Serialization;

namespace Gestor.Domain.Exceptions
{
    [Serializable]
    public class AtividadeJaExistenteNaAlocacaoException : CoreException
    {
        public override string Key => "AtividadeJaExistenteNaAlocacao";
        public override string Message => "A atividade informada já existe na alocação.";

        public AtividadeJaExistenteNaAlocacaoException() : base()
        {
        }

        protected AtividadeJaExistenteNaAlocacaoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}