using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class EstabelecimentoBusiness : BaseBusiness<Estabelecimento>, IEstabelecimentoBusiness
    {

        public override void Alterar(Estabelecimento pTEstabelecimento)
        {

            Estabelecimento tempEstabelecimento = Consulta.FirstOrDefault(p => p.UniqueKey.Equals(pTEstabelecimento.UniqueKey));
            if (tempEstabelecimento == null)
            {
                throw new Exception("Não foi possível encontrar o Estabelecimento.");
            }
            
            
            tempEstabelecimento.Codigo = pTEstabelecimento.Codigo;
            tempEstabelecimento.NomeCompleto = pTEstabelecimento.NomeCompleto;
            tempEstabelecimento.TipoDeEstabelecimento = pTEstabelecimento.TipoDeEstabelecimento;
            tempEstabelecimento.Descricao = pTEstabelecimento.Descricao;

            base.Alterar(tempEstabelecimento);

            tempEstabelecimento.UsuarioExclusao = pTEstabelecimento.UsuarioExclusao;
            base.Terminar(tempEstabelecimento);

            pTEstabelecimento.ID = Guid.NewGuid();
            pTEstabelecimento.UniqueKey = tempEstabelecimento.UniqueKey;
            pTEstabelecimento.NomeCompleto = tempEstabelecimento.NomeCompleto;
            pTEstabelecimento.TipoDeEstabelecimento = tempEstabelecimento.TipoDeEstabelecimento;
            pTEstabelecimento.Descricao = tempEstabelecimento.Descricao;
            pTEstabelecimento.UsuarioInclusao = tempEstabelecimento.UsuarioExclusao;
            pTEstabelecimento.UsuarioExclusao = string.Empty;

            base.Inserir(pTEstabelecimento);




            
        }

    }



}

