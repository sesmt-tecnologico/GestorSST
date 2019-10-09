using Gestor.Domain.Business;
using Gestor.Domain.Entities;
using Gestor.Domain.Enums;
using Gestor.Domain.Exceptions;
using Gestor.Domain.Repositories;
using Gestor.Domain.Repositories.AdmissaoEmpregadoAggregate;
using Gestor.Domain.ValueObjects;
using Gestor.Domain.ViewModels;
using System;

namespace Gestor.Application.Business
{
    internal class EmpregadoBusiness : IEmpregadoBusiness
    {
        private readonly IAuthenticatedUser authenticatedUser;
        private readonly IEmpregadoRepository empregadoRepository;
        private readonly IAdmissaoRepository admissaoRepository;
        private readonly IImageRepository imageRepository;

        public EmpregadoBusiness(IAuthenticatedUser authenticatedUser,
            IEmpregadoRepository empregadoRepository,
            IAdmissaoRepository admissaoRepository,
            IImageRepository imageRepository)
        {
            this.authenticatedUser = authenticatedUser ?? throw new ArgumentNullException(nameof(authenticatedUser));
            this.empregadoRepository = empregadoRepository ?? throw new ArgumentNullException(nameof(empregadoRepository));
            this.admissaoRepository = admissaoRepository ?? throw new ArgumentNullException(nameof(admissaoRepository));
            this.imageRepository = imageRepository ?? throw new ArgumentNullException(nameof(imageRepository));
        }

        public CadastrarResult Cadastrar(CadastrarEmpregadoViewModel cadastrarEmpregadoViewModel)
        {
            var cpf = new Cpf(cadastrarEmpregadoViewModel.Cpf);
            var empregado = empregadoRepository.ObterPeloCpf(cpf);
            if (empregado != null)
                throw new CpfJaCadastradoException();

            empregado = new Empregado(authenticatedUser.UserName, cpf, cadastrarEmpregadoViewModel.Nome, cadastrarEmpregadoViewModel.DataNascimento.Value, cadastrarEmpregadoViewModel.Email, cadastrarEmpregadoViewModel.Telefone, cadastrarEmpregadoViewModel.Matricula);

            empregadoRepository.Inserir(empregado);

            empregadoRepository.UnitOfWork.SaveEntities();

            return new CadastrarResult(empregado.Id, cpf.NumeroComMascara);
        }

        public void Atualizar(Guid empregadoId, AtualizarEmpregadoViewModel atualizarEmpregadoViewModel)
        {
            //TODO: falta auditoria deste método, já que até então salvamos apenas os usuários de criação e término da entidade.
            //posteriormente incluir a Furiza.AuditTrails no projeto para armazenar o historico de todas operações.

            var empregado = empregadoRepository.ObterPeloId(empregadoId)
                ?? throw new RecursoNaoEncontradoException(nameof(Empregado));

            var cpf = new Cpf(atualizarEmpregadoViewModel.Cpf);
            if (cpf != empregado.Cpf && empregadoRepository.ObterPeloCpf(cpf) != null)
                throw new CpfJaCadastradoException();

            empregado.SetCpf(cpf);
            empregado.SetNome(atualizarEmpregadoViewModel.Nome);
            empregado.SetDataNascimento(atualizarEmpregadoViewModel.DataNascimento.Value);
            empregado.SetEmail(atualizarEmpregadoViewModel.Email);

            empregado.Telefone = atualizarEmpregadoViewModel.Telefone;
            empregado.Matricula = atualizarEmpregadoViewModel.Matricula;

            empregadoRepository.Alterar(empregado);

            empregadoRepository.UnitOfWork.SaveEntities();
        }

        public void Excluir(Guid empregadoId)
        {
            var empregado = empregadoRepository.ObterPeloId(empregadoId)
                ?? throw new RecursoNaoEncontradoException(nameof(Empregado));

            if (empregado.Status == StatusEmpregado.Livre && admissaoRepository.PossuiAdmissaoNaoTerminada(empregadoId))
                throw new SituacaoInvalidaParaExclusaoException("Empregado possui histórico de admissões.");

            empregado.Terminar(authenticatedUser.UserName);

            empregadoRepository.Alterar(empregado);

            empregadoRepository.UnitOfWork.SaveEntities();
        }

        #region [+] Imagem Service
        public void IncluirFoto(Guid entidadeId, string imagemBase64)
        {
            if (empregadoRepository.ObterPeloId(entidadeId) == null)
                throw new RecursoNaoEncontradoException(nameof(Empregado));

            imageRepository.Upload<Empregado>(entidadeId, imagemBase64);
        }

        public byte[] ObterFoto(Guid entidadeId)
        {
            if (empregadoRepository.ObterPeloId(entidadeId) == null)
                throw new RecursoNaoEncontradoException(nameof(Empregado));

            return imageRepository.Get<Empregado>(entidadeId);
        }

        public bool PossuiFoto(Guid entidadeId)
        {
            if (empregadoRepository.ObterPeloId(entidadeId) == null)
                throw new RecursoNaoEncontradoException(nameof(Empregado));

            return imageRepository.Has<Empregado>(entidadeId);
        }
        #endregion
    }
}