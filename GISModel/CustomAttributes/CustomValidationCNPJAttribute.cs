using GISHelpers.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GISModel.CustomAttributes
{
    /// <summary>
    /// Validação customizada para CPF
    /// </summary>
    public class CustomValidationCNPJAttribute : ValidationAttribute, IClientValidatable
    {

        /// <summary>
        /// Construtor
        /// </summary>
        public CustomValidationCNPJAttribute() { }

        /// <summary>
        /// Validação server
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return true;

            bool valido = Severino.ValidaCNPJ(value.ToString());
            return valido;
        }

        /// <summary>
        /// Validação client
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
            ModelMetadata metadata, ControllerContext context)
        {


            //string errorMessage = ErrorMessageString;

            //// The value we set here are needed by the jQuery adapter
            //ModelClientValidationRule dateGreaterThanRule = new ModelClientValidationRule();
            //dateGreaterThanRule.ErrorMessage = errorMessage;            
            //dateGreaterThanRule.ValidationType = "customvalidationcnpj"; // This is the name the jQuery validator will use
            ////"otherpropertyname" is the name of the jQuery parameter for the adapter, must be LOWERCASE!
            ////dateGreaterThanRule.ValidationParameters.Add("otherpropertyname", ""); 

            //yield return dateGreaterThanRule;



            yield return new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessageString,
                ValidationType = "customvalidationcnpj" 
            };
        }

    }
}
