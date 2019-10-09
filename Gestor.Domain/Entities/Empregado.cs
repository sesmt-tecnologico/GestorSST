using Gestor.Domain.Enums;
using Gestor.Domain.Exceptions;
using Gestor.Domain.ValueObjects;
using System;

namespace Gestor.Domain.Entities
{
    public class Empregado : EntidadeBase
    {
        public string Cpf { get; private set; }
        public string Nome { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public string Email { get; private set; }
        public string Telefone { get; set; }
        public string Matricula { get; set; }
        public StatusEmpregado Status { get; private set; }

        private Empregado()
        {
        }

        public Empregado(string usuarioInclusao, Cpf cpf, string nome, DateTime dataNascimento, string email, string telefone, string matricula) : base(usuarioInclusao)
        {
            SetCpf(cpf);
            SetNome(nome);
            SetDataNascimento(dataNascimento);
            SetEmail(email);

            Telefone = telefone;
            Matricula = matricula;

            Status = StatusEmpregado.Livre;
        }

        public void SetCpf(Cpf cpf) => Cpf = cpf;

        public void SetNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new CampoNaoPodeSerNuloException(nameof(nome));

            Nome = nome.Trim().ToUpper();
        }

        public void SetDataNascimento(DateTime dataNascimento)
        {
            var idade = DateTime.Today.Year - dataNascimento.Year;
            if (dataNascimento > DateTime.Today.AddYears(-idade))
                idade--;

            if (idade < 16 || idade > 80)
                throw new IdadeNaoPermitidaException();

            DataNascimento = dataNascimento.Date;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new CampoNaoPodeSerNuloException(nameof(email));

            Email = email.Trim().ToLower();
        }

        protected override void ValidarTerminoDaEntidade()
        {
            if (Status != StatusEmpregado.Livre)
                throw new SituacaoInvalidaParaExclusaoException("Status do empregado diferente de Livre.");
        }        
    }
}