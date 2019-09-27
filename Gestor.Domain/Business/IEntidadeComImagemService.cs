using Gestor.Domain.Exceptions;
using System;

namespace Gestor.Domain.Business
{
    public interface IEntidadeComImagemService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entidadeId"></param>
        /// <param name="imagemBase64"></param>
        /// <exception cref="RecursoNaoEncontradoException"></exception>
        void IncluirFoto(Guid entidadeId, string imagemBase64);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entidadeId"></param>
        /// <returns>Foto de uma entidade em formato de array de bytes.</returns>
        /// <exception cref="RecursoNaoEncontradoException"></exception>
        byte[] ObterFoto(Guid entidadeId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entidadeId"></param>
        /// <returns>Um booleano indicando se uma entidade possui uma foto registrada no sistema (true) ou não (false).</returns>
        /// <exception cref="RecursoNaoEncontradoException"></exception>
        bool PossuiFoto(Guid entidadeId);
    }
}