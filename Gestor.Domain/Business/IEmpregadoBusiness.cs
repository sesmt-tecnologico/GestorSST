using Gestor.Domain.Exceptions;
using Gestor.Domain.ViewModels;
using System;

namespace Gestor.Domain.Business
{
    public interface IEmpregadoBusiness : IEntidadeComImagemService
    {
        /// <summary>
        /// Cadastra um empregado.
        /// </summary>
        /// <param name="cadastrarEmpregadoViewModel"></param>
        /// <exception cref="CpfInvalidoException"></exception>
        /// <exception cref="CpfJaCadastradoException"></exception>
        /// <exception cref="CampoNaoPodeSerNuloException"></exception>
        /// <exception cref="IdadeNaoPermitidaException"></exception>
        /// <returns>Objeto contendo o Id e o Cpf formatado do empregado cadastrado.</returns>
        CadastrarResult Cadastrar(CadastrarEmpregadoViewModel cadastrarEmpregadoViewModel);

        /// <summary>
        /// Atualiza as informações de um empregado.
        /// </summary>
        /// <param name="empregadoId"></param>
        /// <param name="atualizarEmpregadoViewModel"></param>
        /// <exception cref="RecursoNaoEncontradoException"></exception>
        /// <exception cref="CpfInvalidoException"></exception>
        /// <exception cref="CpfJaCadastradoException"></exception>
        /// <exception cref="CampoNaoPodeSerNuloException"></exception>
        /// <exception cref="IdadeNaoPermitidaException"></exception>
        void Atualizar(Guid empregadoId, AtualizarEmpregadoViewModel atualizarEmpregadoViewModel);

        /// <summary>
        /// Exclui um empregado desde que este não possua um histórico consolidado no sistema.
        /// </summary>
        /// <param name="empregadoId"></param>
        /// <exception cref="RecursoNaoEncontradoException"></exception>
        /// <exception cref="CampoNaoPodeSerNuloException"></exception>
        /// <exception cref="SituacaoInvalidaParaExclusaoException"></exception>
        void Excluir(Guid empregadoId);
    }
}