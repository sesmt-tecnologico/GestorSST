using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class FuncaoBusiness : BaseBusiness<Funcao>, IFuncaoBusiness
    {

        public override void Inserir(Funcao pFuncao)
        {
            if (Consulta.Any(u => u.ID.Equals(pFuncao.ID)))
                throw new InvalidOperationException("Não é possível inserir A Função, pois já existe uma Função com este ID.");

            base.Inserir(pFuncao);
        }

        public override void Alterar(Funcao pFuncao)
        {
            Funcao tempFuncao = Consulta.FirstOrDefault(p => p.ID.Equals(pFuncao.ID));

            if (tempFuncao == null)
            {
                throw new Exception("não foi possível encontrar esta Função");
            }

            tempFuncao.NomeDaFuncao = pFuncao.NomeDaFuncao;
            
            base.Alterar(tempFuncao);
        }

    }
}
