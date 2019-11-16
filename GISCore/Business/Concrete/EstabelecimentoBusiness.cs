using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class EstabelecimentoBusiness : BaseBusiness<Estabelecimento>, IEstabelecimentoBusiness
    {
        public override void Inserir(Estabelecimento pTEstabelecimento)
        {

            //if (Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.Numero.Equals(entidade.Numero)).Count() > 0)
            //    throw new Exception("Já existe um contrato no banco de dados com o número: " + entidade.Numero);

            base.Inserir(pTEstabelecimento);
        }

        public override void Alterar(Estabelecimento pTEstabelecimento)
        {

            Estabelecimento tempEstabelecimento = Consulta.FirstOrDefault(p => p.UniqueKey.Equals(pTEstabelecimento.UniqueKey));
            if (tempEstabelecimento == null)
            {
                throw new Exception("Não foi possível encontrar o Estabelecimento.");
            }
                               

            base.Alterar(pTEstabelecimento);




            
        }

    }



}

