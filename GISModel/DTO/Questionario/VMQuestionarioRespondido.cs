using System.Collections.Generic;

namespace GISModel.DTO.Questionario
{
    public class VMQuestionarioRespondido
    {

        public string UKFonteGeradora { get; set; }

        public string UKEmpregado { get; set; }

        public string UKQuestionario { get; set; }

        public string UKEmpresa { get; set; }

        public string Registro { get; set; }

        public string latitude { get; set; }

        public string longitude { get; set; }

        public string Status { get; set; }

        public List<string[]> PerguntasRespondidas { get; set; }

    }
}
