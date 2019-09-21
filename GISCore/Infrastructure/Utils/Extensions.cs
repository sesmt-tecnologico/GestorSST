using GISHelpers.Utils;
using System;
using System.Globalization;
using System.Web;
using System.Xml;

namespace GISCore.Infrastructure.Utils
{
    public static class Extensions
    {

        public static void GravaCookie(string id, string valor, int validadeEmMinutos)
        {
            try
            {
                var cookie = new HttpCookie(id, Compactador.Compactar(valor));
                cookie.Expires = DateTime.Now.AddMinutes(validadeEmMinutos);

                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch { }
        }

        public static string RecuperaCookie(string id, bool clear = false)
        {
            string valor = string.Empty;

            try
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[id];

                if (cookie != null)
                    if (!string.IsNullOrEmpty(cookie.Value))
                        valor = Compactador.Descompactar(cookie.Value);

                if (clear)
                {
                    cookie = new HttpCookie(id);
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
            }
            catch { }

            return valor;
        }

        public static string TratarNomeProprio(string nomeProprio)
        {
            return nomeProprio.Trim().ToUpper();
        }


        [Obsolete("Metodo obsoleto, use os métodos de extensão do Xml 'ToBoolean'. Adicione referência para SPF.Custom.SGEConsulta.Core.Helpers", false)]
        public static bool ConverteValorParaBoolean(XmlAttribute atributoXml)
        {
            try
            {
                if (atributoXml == null)
                    return false;
                else if (string.IsNullOrWhiteSpace(atributoXml.Value))
                    return false;
                else
                    return Convert.ToBoolean(atributoXml.Value.ToString());
            }
            catch { return false; }
        }

        public static bool ConverteValorParaBoolean(XmlAttribute atributoXml, XmlAttribute atributoXml2)
        {
            try
            {
                bool valor1 = false;
                bool valor2 = false;

                if (atributoXml == null)
                    valor1 = false;
                else if (string.IsNullOrWhiteSpace(atributoXml.Value))
                    valor1 = false;
                else
                    valor1 = Convert.ToBoolean(atributoXml.Value.ToString());

                if (atributoXml2 == null)
                    valor2 = false;
                else if (string.IsNullOrWhiteSpace(atributoXml2.Value))
                    valor2 = false;
                else
                    valor2 = Convert.ToBoolean(atributoXml2.Value.ToString());

                return valor1 || valor2;
            }
            catch { return false; }
        }

        public static bool ConverteValorParaBoolean(string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                    return false;
                else
                    return Convert.ToBoolean(value);
            }
            catch { return false; }
        }

        [Obsolete("Metodo obsoleto, use os métodos de extensão da Xml 'ToInteger'. Adicione referência para SPF.Custom.SGEConsulta.Core.Helpers", false)]
        public static int ConverteValorParaInteiro(XmlAttribute atributoXml)
        {
            try
            {
                if (atributoXml == null)
                    return 0;
                else if (string.IsNullOrWhiteSpace(atributoXml.Value))
                    return 0;
                else
                    return Convert.ToInt32(atributoXml.Value.ToString());
            }
            catch { return 0; }
        }


        public static string DataString(string data)
        {
            try
            {
                string dataResult = "";
                if (data != null && data != "")
                {
                    switch (DateTime.ParseExact(data, "dd/MM/yyyy", CultureInfo.InvariantCulture).Month)
                    {
                        case 1:
                            dataResult = "jan";
                            break;
                        case 2:
                            dataResult = "feb";
                            break;
                        case 3:
                            dataResult = "mar";
                            break;
                        case 4:
                            dataResult = "apr";
                            break;
                        case 5:
                            dataResult = "may";
                            break;
                        case 6:
                            dataResult = "jun";
                            break;
                        case 7:
                            dataResult = "jul";
                            break;
                        case 8:
                            dataResult = "aug";
                            break;
                        case 9:
                            dataResult = "sep";
                            break;
                        case 10:
                            dataResult = "oct";
                            break;
                        case 11:
                            dataResult = "nov";
                            break;
                        case 12:
                            dataResult = "dec";
                            break;
                    }
                    return DateTime.ParseExact(data, "dd/MM/yyyy", CultureInfo.InvariantCulture).Year.ToString() + "-" + dataResult + "-" + DateTime.ParseExact(data, "dd/MM/yyyy", CultureInfo.InvariantCulture).Day.ToString();
                    //  return  DateTime.Parse(data).Day.ToString() + "-" + dataResult + "-" + DateTime.Parse(data).Year.ToString();
                }
                else
                    return dataResult;
            }
            catch
            {
                throw new Exception(data);
            }
        }

    }
}
