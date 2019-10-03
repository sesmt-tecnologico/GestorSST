using Gestor.Domain.Exceptions;
using Gestor.Domain.ViewModels.Empregados;
using System;

namespace Gestor.Domain.Business.Empregados
{
    public interface IAlocacaoBusiness
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="admissaoId"></param>
        /// <param name="alocarEmpregadoViewModel"></param>
        /// <exception cref="RecursoNaoEncontradoException"></exception>
        /// TODO: mapear demais exceções
        void Alocar(Guid admissaoId, AlocarEmpregadoViewModel alocarEmpregadoViewModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alocacaoId"></param>
        /// <param name="AdicionarAtividadeNaAlocacaoDeEmpregadoViewModel"></param>
        /// <exception cref="RecursoNaoEncontradoException"></exception>
        /// TODO: mapear demais exceções
        void AdicionarAtividade(Guid alocacaoId, AdicionarAtividadeNaAlocacaoDeEmpregadoViewModel adicionarAtividadeNaAlocacaoDeEmpregadoViewModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alocacaoId"></param>
        /// <param name="removerAtividadeDaAlocacaoDeEmpregadoViewModel"></param>
        /// <exception cref="RecursoNaoEncontradoException"></exception>
        /// TODO: mapear demais exceções
        void RemoverAtividade(Guid alocacaoId, RemoverAtividadeDaAlocacaoDeEmpregadoViewModel removerAtividadeDaAlocacaoDeEmpregadoViewModel);
    }
}