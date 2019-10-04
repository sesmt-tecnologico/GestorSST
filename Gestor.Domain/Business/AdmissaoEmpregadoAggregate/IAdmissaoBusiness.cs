using Gestor.Domain.Exceptions;
using Gestor.Domain.ViewModels.AdmissaoEmpregadoAggregate;
using System;

namespace Gestor.Domain.Business.AdmissaoEmpregadoAggregate
{
    public interface IAdmissaoBusiness
    {
        /// <summary>
        /// Admite um empregado em uma empresa com possibilidade de informar a empresa tomadora do serviço no caso de uma terceirização.
        /// </summary>
        /// <param name="empregadoId"></param>
        /// <param name="admitirViewModel"></param>
        /// TODO: mapear demais exceções
        void Admitir(Guid empregadoId, AdmitirViewModel admitirViewModel);

        /// <summary>
        /// Demite um empregado com uma admissão válida.
        /// </summary>
        /// <param name="admissaoId"></param>
        /// <param name="demitirViewModel"></param>
        /// <exception cref="RecursoNaoEncontradoException"></exception>
        /// TODO: mapear demais exceções
        void Demitir(Guid admissaoId, DemitirViewModel demitirViewModel);

        /// <summary>
        /// Exclui uma admissão de empregado desde que esta não possua um histórico consolidado no sistema.
        /// </summary>
        /// <param name="admissaoId"></param>
        /// <exception cref="RecursoNaoEncontradoException"></exception>
        /// /// TODO: mapear demais exceções
        void Excluir(Guid admissaoId);
    }
}