using GISModel.DTO.Admissao;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Abstract
{
    public interface IAdmissaoBusiness: IBaseBusiness<Admissao>
    {
        List<Alocacao> BuscarAlocacoes(string UKAdmissao);

        Admissao GetAdmissao(Guid ukEmpregado);

        List<Admissao> BuscarAdmissoesAtuais(string UKEmpregado);

        List<Alocacao> BuscarAlocacoes(string UKAdmissao, Guid UKEmpregado);

        List<ArquivoEmpregadoViewModel> RetonarListaArquivoEmpregado(Guid ukLocado, Guid ukEmpregado, Guid ukFuncao);
    }
}
