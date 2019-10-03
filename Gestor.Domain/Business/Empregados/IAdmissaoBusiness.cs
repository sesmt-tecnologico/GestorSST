using Gestor.Domain.Exceptions;
using Gestor.Domain.ViewModels.Empregados;
using System;

namespace Gestor.Domain.Business.Empregados
{
    public interface IAdmissaoBusiness
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="empregadoId"></param>
        /// <param name="admitirEmpregadoViewModel"></param>
        /// <exception cref="RecursoNaoEncontradoException"></exception>
        /// TODO: mapear demais exceções
        void Admitir(Guid empregadoId, AdmitirEmpregadoViewModel admitirEmpregadoViewModel);
    }
}