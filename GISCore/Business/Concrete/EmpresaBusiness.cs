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
            if (Consulta.Any(u => u.CNPJ.Equals(pEmpresa.CNPJ.Trim())))
                throw new InvalidOperationException("Não é possível inserir empresa, pois já existe uma empresa registrada com este CNPJ.");

            if (Consulta.Any(u => u.NomeFantasia.ToUpper().Equals(pEmpresa.NomeFantasia.Trim().ToUpper())))
                throw new InvalidOperationException("Não é possível inserir empresa, pois já existe uma empresa registrada com este Nome Fatasia.");

            pEmpresa.UniqueKey = Guid.NewGuid();

            base.Inserir(pEmpresa);
        }

        public override void Alterar(Empresa pEmpresa)
        {
            if (Consulta.Any(u => u.CNPJ.Equals(pEmpresa.CNPJ.Trim()) && !u.UniqueKey.Equals(pEmpresa.UniqueKey)))
                throw new InvalidOperationException("Não é possível atualizar esta empresa, pois o CNPJ já está sendo usado por outra empresa.");

            if (Consulta.Any(u => u.NomeFantasia.ToUpper().Equals(pEmpresa.NomeFantasia.Trim().ToUpper()) && !u.UniqueKey.Equals(pEmpresa.UniqueKey)))
                throw new InvalidOperationException("Não é possível atualizar esta empresa, pois o Nome Fatasia está sendo usado por outra empresa.");

            Empresa tempEmpresa = Consulta.FirstOrDefault(p => p.UniqueKey.Equals(pEmpresa.UniqueKey));
            if (tempEmpresa == null)
            {
                throw new Exception("Não foi possível encontrar a empresa através do ID.");
            }
            else
            {
                tempEmpresa.NomeFantasia = pEmpresa.NomeFantasia;
                tempEmpresa.RazaoSocial = pEmpresa.RazaoSocial;
                tempEmpresa.CNPJ = pEmpresa.CNPJ;
                tempEmpresa.URL_AD = pEmpresa.URL_AD;
                tempEmpresa.URL_WS = pEmpresa.URL_WS;
                tempEmpresa.URL_Site = pEmpresa.URL_Site;

                base.Alterar(tempEmpresa);
            }
        }

    }
}
