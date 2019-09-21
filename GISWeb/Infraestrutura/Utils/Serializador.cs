using GISModel.DTO.Conta;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace GISWeb.Infraestrutura.Utils
{
    public class Serializador
    {
        public static string Serializar(AutenticacaoModel autenticacaoModel)
        {
            var serializer = new XmlSerializer(typeof(AutenticacaoModel));
            var sw = new StringWriter();
            var xw = XmlWriter.Create(sw);
            serializer.Serialize(xw, autenticacaoModel);
            var autenticacaoModelSerializado = sw.ToString();
            return autenticacaoModelSerializado;
        }

        public static AutenticacaoModel Deserializar(string autenticacaoModelSerializado)
        {
            var serializer = new XmlSerializer(typeof(AutenticacaoModel));
            var autenticacaoModel = (AutenticacaoModel)serializer.Deserialize(new StringReader(autenticacaoModelSerializado));
            return autenticacaoModel;
        }

    }
}