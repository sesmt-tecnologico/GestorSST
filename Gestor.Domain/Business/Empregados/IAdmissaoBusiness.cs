﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <exception cref="CpfInvalidoException"></exception>
        /// <exception cref="CpfJaCadastradoException"></exception>
        /// <exception cref="CampoNaoPodeSerNuloException"></exception>
        /// <exception cref="IdadeNaoPermitidaException"></exception>
        void Admitir(Guid empregadoId, AdmitirEmpregadoViewModel admitirEmpregadoViewModel);
    }
}