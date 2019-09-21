using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class DiretoriaBusiness : BaseBusiness<Diretoria>, IDiretoriaBusiness
    {

        public override void Inserir(Diretoria pDiretoria)
        {

            if (Consulta.Any(u => u.ID.Equals(pDiretoria.ID)))
                throw new InvalidOperationException("Não é possível inserir a Diretoria, pois já existe uma Diretoria com este ID.");

            base.Inserir(pDiretoria);
        }

        public override void Alterar(Diretoria pDiretoria)
        {
            Diretoria tempDiretoria = Consulta.FirstOrDefault(p => p.ID.Equals(pDiretoria.ID));

            if (tempDiretoria == null)
            {
                throw new Exception("não foi possível encontrar esta Diretoria");
            }

            tempDiretoria.Sigla = pDiretoria.Sigla;
            tempDiretoria.Descricao = pDiretoria.Descricao;


            base.Alterar(tempDiretoria);

        }

    }
}
