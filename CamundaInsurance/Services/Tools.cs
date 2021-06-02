using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CamundaInsurance.Services
{
    public static class Tools
    {
        public static IEnumerable<ValidationResult> ValidateModel<T>(T model)
        {
            if (model == null)
            {
                throw new NullReferenceException("Can not validate null reference");
            }
            var validationContext = new ValidationContext(model);
            var results = new List<ValidationResult>();
            if (Validator.TryValidateObject(model, validationContext, results, true))
            {
                return null;
            }
            else
            {
                return results;
            }
        }
    }
}
