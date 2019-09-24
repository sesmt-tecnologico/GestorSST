using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class NivelHierarquicoBusiness : BaseBusiness<NivelHierarquico>, INivelHierarquicoBusiness
    {

        public override void Inserir(NivelHierarquico entidade)
        {
            if (Consulta.Any(u => string.IsNullOrEmpty(u.UsuarioExclusao) && u.Nome.Trim().ToUpper().Equals(entidade.Nome.ToUpper().Trim())))
                throw new InvalidOperationException("Não é possível inserir o este nível, pois já existe um nível cadastro com este nome.");

            entidade.UniqueKey = Guid.NewGuid();

            base.Inserir(entidade);
        }

        public override void Alterar(NivelHierarquico entidade)
        {
            NivelHierarquico temp = Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(entidade.UniqueKey));
            if (temp == null)
                throw new Exception("Não foi possível encontra o nível através do ID.");
            else if (Consulta.Any(u => string.IsNullOrEmpty(u.UsuarioExclusao) && !u.UniqueKey.Equals(entidade.UniqueKey) && u.Nome.Trim().ToUpper().Equals(entidade.Nome.ToUpper().Trim())))
                throw new InvalidOperationException("Não é possível atualizar este nível, pois já existe um nível cadastro com este nome.");
            else
            {
                temp.DataExclusao = DateTime.Now;
                temp.UsuarioExclusao = entidade.UsuarioExclusao;
                base.Alterar(temp);

                entidade.UniqueKey = temp.UniqueKey;
                entidade.UsuarioExclusao = string.Empty;
                base.Inserir(entidade);
            }
        }

    }
}
