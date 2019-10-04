using Gestor.Domain.Exceptions;
using Gestor.Domain.ViewModels.AdmissaoEmpregadoAggregate;
using System;

namespace Gestor.Domain.Business.AdmissaoEmpregadoAggregate
{
    public interface IAlocacaoBusiness
    {
        /// <summary>
        /// Aloca um empregado admitido previamente com uma série de informações.
        /// </summary>
        /// <param name="admissaoId"></param>
        /// <param name="alocarViewModel"></param>
        /// TODO: mapear demais exceções
        void Alocar(Guid admissaoId, AlocarViewModel alocarViewModel);

        /// <summary>
        /// Desaloca um empregado com uma alocação válida.
        /// </summary>
        /// <param name="alocacaoId"></param>
        /// <param name="desalocarViewModel"></param>
        /// <exception cref="RecursoNaoEncontradoException"></exception>
        /// TODO: mapear demais exceções
        void Desalocar(Guid alocacaoId, DesalocarViewModel desalocarViewModel);

        /// <summary>
        /// Exclui uma alocação de empregado desde que esta não possua um histórico consolidado no sistema.
        /// </summary>
        /// <param name="alocacaoId"></param>
        /// <exception cref="RecursoNaoEncontradoException"></exception>
        /// /// TODO: mapear demais exceções
        void Excluir(Guid alocacaoId);
    }
}