using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class ContratoBusiness : BaseBusiness<Contrato>, IContratoBusiness
    {

        public override void Inserir(Contrato entidade)
        {

            if (Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.Numero.Equals(entidade.Numero)).Count() > 0)
                throw new Exception("Já existe um contrato no banco de dados com o número: " + entidade.Numero);
            
            base.Inserir(entidade);
        }


        public override void Alterar(Contrato pContrato)
        {

            Contrato tempContrato = Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(pContrato.UniqueKey));
            if (tempContrato == null)
            {
                throw new Exception("Não foi possível encontrar o Contrato através do ID.");
            }
            else
            {
                tempContrato.Descricao = pContrato.Descricao;
                base.Alterar(tempContrato);
            }
        }

    }

}
        

