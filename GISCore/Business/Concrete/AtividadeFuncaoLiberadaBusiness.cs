using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class AtividadeFuncaoLiberadaBusiness : BaseBusiness<AtividadeFuncaoLiberada>, IAtividadeFuncaoLiberadaBusiness
    {

        public override void Inserir(AtividadeFuncaoLiberada pAtividadeFuncaoLiberada)
        {
            
            
            pAtividadeFuncaoLiberada.ID = Guid.NewGuid().ToString();
            
            base.Inserir(pAtividadeFuncaoLiberada);

           

        }

        public override void Alterar(AtividadeFuncaoLiberada pAtividadeFuncaoLiberada)
        {
            List<AtividadeFuncaoLiberada> lAtividadeFuncaoLiberada = Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.IDAtividade.Equals(pAtividadeFuncaoLiberada.IDAtividade)&& p.IDAlocacao.Equals(pAtividadeFuncaoLiberada.IDAlocacao)).ToList();

            if (lAtividadeFuncaoLiberada.Count.Equals(1))
            {
                AtividadeFuncaoLiberada oAtividadeFuncaoLiberada = lAtividadeFuncaoLiberada[0];

                oAtividadeFuncaoLiberada.UsuarioExclusao = pAtividadeFuncaoLiberada.UsuarioExclusao;
                oAtividadeFuncaoLiberada.DataExclusao = pAtividadeFuncaoLiberada.DataExclusao;

                base.Alterar(oAtividadeFuncaoLiberada);
            }
        }

    }
}
