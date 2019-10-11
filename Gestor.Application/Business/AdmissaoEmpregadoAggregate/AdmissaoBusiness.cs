using Gestor.Domain.Business.AdmissaoEmpregadoAggregate;
using Gestor.Domain.Entities;
using Gestor.Domain.Entities.AdmissaoEmpregadoAggregate;
using Gestor.Domain.Exceptions;
using Gestor.Domain.Repositories;
using Gestor.Domain.Repositories.AdmissaoEmpregadoAggregate;
using Gestor.Domain.ViewModels.AdmissaoEmpregadoAggregate;
using System;

namespace Gestor.Application.Business.AdmissaoEmpregadoAggregate
{
    internal class AdmissaoBusiness : IAdmissaoBusiness
    {
        private readonly IAuthenticatedUser authenticatedUser;
        private readonly IAdmissaoRepository admissaoRepository;
        private readonly IEmpregadoRepository empregadoRepository;

        public AdmissaoBusiness(IAuthenticatedUser authenticatedUser,
            IAdmissaoRepository admissaoRepository,
            IEmpregadoRepository empregadoRepository)
        {
            this.authenticatedUser = authenticatedUser ?? throw new ArgumentNullException(nameof(authenticatedUser));
            this.admissaoRepository = admissaoRepository ?? throw new ArgumentNullException(nameof(admissaoRepository));
            this.empregadoRepository = empregadoRepository ?? throw new ArgumentNullException(nameof(empregadoRepository));
        }

        public void Admitir(Guid empregadoId, AdmitirViewModel admitirViewModel)
        {
            var empregado = empregadoRepository.ObterPeloId(empregadoId)
                ?? throw new RecursoNaoEncontradoException(nameof(Empregado));

            empregado.SetAdmitido();

            empregadoRepository.Alterar(empregado);

            var admissao = new Admissao(authenticatedUser.UserName, empregadoId, admitirViewModel.EmpresaId.Value, admitirViewModel.TomadoraId, admitirViewModel.DataAdmissao.Value);

            admissaoRepository.Inserir(admissao);

            admissaoRepository.UnitOfWork.SaveEntities();
        }

        public void Demitir(Guid admissaoId, DemitirViewModel demitirViewModel)
        {
            throw new NotImplementedException();
        }

        public void Excluir(Guid admissaoId)
        {
            throw new NotImplementedException();
        }
    }
}