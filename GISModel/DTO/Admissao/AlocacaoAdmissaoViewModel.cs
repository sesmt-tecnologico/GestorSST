using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.Admissao
{
    public class AlocacaoAdmissaoViewModel
    {
        public Guid UniqueKey { get; set; }
        public string Contrato { get; set; }
        public string NomeDoCargo { get; set; }
        public string NomeDaFuncao { get; set; }
        public string Descricao { get; set; }
        public string Estabelecimento { get; set; }
        public string NomeDaEquipe { get; set; }
        public string Sigla { get; set; }
        public string Atividade { get; set; }
        public DateTime DataInclusao { get; set; }
        public string UsuarioInclusao { get; set; }
        public Guid UKEstab { get; set; }
    }
}
