using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class AnaliseRiscoBusiness : BaseBusiness<AnaliseRisco>, IAnaliseRiscoBusiness
    {

        public override void Inserir(AnaliseRisco pAnaliseRisco)
        {
            pAnaliseRisco.ID = Guid.NewGuid().ToString();
            base.Inserir(pAnaliseRisco);
        }

        public override void Alterar(AnaliseRisco pAnaliseRisco)
        {
            List<AnaliseRisco> lAnaliseRisco = Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.IDAlocacao.Equals(pAnaliseRisco.IDAlocacao)).ToList();

            if (lAnaliseRisco.Count.Equals(1))
            {
                AnaliseRisco oAnaliseRisco = lAnaliseRisco[0];

                oAnaliseRisco.UsuarioExclusao = pAnaliseRisco.UsuarioExclusao;
                oAnaliseRisco.DataExclusao = pAnaliseRisco.DataExclusao;

                base.Alterar(pAnaliseRisco);
            }
        }

    }
}
