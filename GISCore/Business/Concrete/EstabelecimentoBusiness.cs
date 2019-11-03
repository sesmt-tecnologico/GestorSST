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
                                  

            tempEstabelecimento.UsuarioExclusao = pTEstabelecimento.UsuarioExclusao;
            base.Terminar(tempEstabelecimento);

            tempEstabelecimento.ID = Guid.NewGuid();           
            //tempEstabelecimento.Codigo = pTEstabelecimento.Codigo;
            tempEstabelecimento.NomeCompleto = pTEstabelecimento.NomeCompleto;
            tempEstabelecimento.TipoDeEstabelecimento = pTEstabelecimento.TipoDeEstabelecimento;
            tempEstabelecimento.Descricao = pTEstabelecimento.Descricao;
            tempEstabelecimento.UsuarioInclusao = pTEstabelecimento.UsuarioExclusao;
            tempEstabelecimento.UsuarioExclusao = string.Empty;

            base.Inserir(tempEstabelecimento);




            
        }

    }



}

