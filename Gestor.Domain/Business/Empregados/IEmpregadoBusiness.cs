using Gestor.Domain.Exceptions;
using Gestor.Domain.ViewModels;
using Gestor.Domain.ViewModels.Empregados;
using System;

namespace Gestor.Domain.Business.Empregados
{
    public interface IEmpregadoBusiness : IEntidadeComImagemService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cadastrarEmpregadoViewModel"></param>
        /// <exception cref="CpfInvalidoException"></exception>
        /// <exception cref="CpfJaCadastradoException"></exception>
        /// <exception cref="CampoNaoPodeSerNuloException"></exception>
        /// <exception cref="IdadeNaoPermitidaException"></exception>
        /// <returns>Objeto contendo o Id e o Cpf formatado do empregado cadastrado.</returns>
        CadastrarResult Cadastrar(CadastrarEmpregadoViewModel cadastrarEmpregadoViewModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="empregadoId"></param>
        /// <param name="atualizarEmpregadoViewModel"></param>
        /// <exception cref="RecursoNaoEncontradoException"></exception>
        /// <exception cref="CpfInvalidoException"></exception>
        /// <exception cref="CpfJaCadastradoException"></exception>
        /// <exception cref="CampoNaoPodeSerNuloException"></exception>
        /// <exception cref="IdadeNaoPermitidaException"></exception>
        void Atualizar(Guid empregadoId, AtualizarEmpregadoViewModel atualizarEmpregadoViewModel);
    }
}