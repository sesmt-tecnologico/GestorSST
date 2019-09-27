using System;
using System.Runtime.Serialization;

namespace Gestor.Domain.Exceptions
{
    [Serializable]
    public class CpfJaCadastradoException : CoreException
    {
        public override string Key => "CpfJaCadastrado";
        public override string Message => "O CPF fornecido já está cadastrado na base de dados.";

        public CpfJaCadastradoException() : base()
        {
        }

        protected CpfJaCadastradoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}