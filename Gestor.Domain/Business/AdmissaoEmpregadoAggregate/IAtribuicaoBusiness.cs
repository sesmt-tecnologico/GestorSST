using Gestor.Domain.Exceptions;
using Gestor.Domain.ViewModels.AdmissaoEmpregadoAggregate;
using System;

namespace Gestor.Domain.Business.AdmissaoEmpregadoAggregate
{
    public interface IAtribuicaoBusiness
    {
        /// <summary>
        /// Atribui uma atividade a uma alocação válida de um empregado.
        /// </summary>
        /// <param name="alocacaoId"></param>
        /// <param name="atribuirViewModel"></param>
        /// <exception cref="RecursoNaoEncontradoException"></exception>
        void Atribuir(Guid alocacaoId, AtribuirViewModel atribuirViewModel);

        /// <summary>
        /// Exclui uma atribuição de atividade de uma alocação válida de um empregado desde que esta não possua um histórico consolidado no sistema.
        /// </summary>
        /// <param name="atividadeId"></param>
        /// <exception cref="RecursoNaoEncontradoException"></exception>
        /// <exception cref="CampoNaoPodeSerNuloException"></exception>
        /// <exception cref="SituacaoInvalidaParaExclusaoException"></exception>
        void Excluir(Guid atividadeId);
    }
}