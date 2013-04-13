using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Bsnsapp.Attributes;


namespace Bsnsapp.Validators
{
    /// <summary>
    /// Custom validator written for SameAsAttribute, SameAsAttribute needs to be registered with SameAsValidator using DataAnnotationsModelValidatorProvider.Register(....) method
    /// </summary>
    public class SameAsValidator : DataAnnotationsModelValidator
    {
        public SameAsValidator(ModelMetadata metadata, ControllerContext context, ValidationAttribute attribute)
            : base(metadata, context, attribute)
        {

        }

        /// <summary>
        /// Validate method will be called to validate the property on which the attribute is applied. 
        /// </summary>
        /// <param name="container">Sends the entire model as container, this is useful incase property needs to be checked against other prperties in the model</param>
        /// <returns></returns>
        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            var dependentField = Metadata.ContainerType.GetProperty(((SameAsAttribute)Attribute).Property);
            var field = Metadata.ContainerType.GetProperty(this.Metadata.PropertyName);
            if (dependentField != null && field != null)
            {
                object dependentValue = dependentField.GetValue(container, null);
                object value = field.GetValue(container, null);
                if ((dependentValue != null && dependentValue.Equals(value)))
                {
                    if (!Attribute.IsValid(this.Metadata.Model))
                    {
                        yield return new ModelValidationResult { Message = ErrorMessage };
                    }
                }
                else

                    yield return new ModelValidationResult { Message = ErrorMessage };

            }



        }


    }
}

//namespace CorpBusiness.Validators
//{
//    public class SameAsValidator
//    {
//    }
//}