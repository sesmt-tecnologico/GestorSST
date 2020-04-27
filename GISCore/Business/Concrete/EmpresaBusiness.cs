using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Linq;

namespace GISCore.Business.Concrete
{
    public class EmpresaBusiness : BaseBusiness<Empresa>, IEmpresaBusiness
    {

        public override void Inserir(Empresa pEmpresa)
        {
            if (Consulta.Any(u => u.CNPJ.Equals(pEmpresa.CNPJ.Trim()) && string.IsNullOrEmpty(u.UsuarioExclusao)))
                throw new InvalidOperationException("Não é possível inserir empresa, pois já existe uma empresa registrada com este CNPJ.");

            if (Consulta.Any(u => u.NomeFantasia.ToUpper().Equals(pEmpresa.NomeFantasia.Trim().ToUpper()) && string.IsNullOrEmpty(u.UsuarioExclusao)))
                throw new InvalidOperationException("Não é possível inserir empresa, pois já existe uma empresa registrada com este Nome Fatasia.");

            base.Inserir(pEmpresa);
        }

        public override void Alterar(Empresa pEmpresa)
        {
            if (Consulta.Any(u => u.CNPJ.Equals(pEmpresa.CNPJ.Trim()) && !u.UniqueKey.Equals(pEmpresa.UniqueKey)))
                throw new InvalidOperationException("Não é possível atualizar esta empresa, pois o CNPJ já está sendo usado por outra empresa.");

            if (Consulta.Any(u => u.NomeFantasia.ToUpper().Equals(pEmpresa.NomeFantasia.Trim().ToUpper()) && !u.UniqueKey.Equals(pEmpresa.UniqueKey)))
                throw new InvalidOperationException("Não é possível atualizar esta empresa, pois o Nome Fatasia está sendo usado por outra empresa.");

            Empresa tempEmpresa = Consulta.FirstOrDefault(p => p.ID.Equals(pEmpresa.ID));
            if (tempEmpresa == null)
            {
                throw new Exception("Não foi possível encontrar a empresa através do ID.");
            }
            else
            {
                tempEmpresa.UsuarioExclusao = pEmpresa.UsuarioExclusao;
                base.Terminar(tempEmpresa);

                pEmpresa.ID = Guid.NewGuid();
                pEmpresa.UniqueKey = tempEmpresa.UniqueKey;
                pEmpresa.UsuarioInclusao = pEmpresa.UsuarioExclusao;
                pEmpresa.UsuarioExclusao = string.Empty;
                base.Inserir(pEmpresa);


            }
        }

    }
}
