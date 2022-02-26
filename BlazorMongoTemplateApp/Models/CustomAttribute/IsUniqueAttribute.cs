using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BlazorMongoTemplateApp.Database;
using Engine.Database;

namespace BlazorMongoTemplateApp.Models.CustomAttribute
{
    public class IsUniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            using var context = ContextFactory.MakeContext(); 

            var theType = validationContext.ObjectType;
            var propertyName = validationContext.MemberName;
            
            var methodInfo = context
                .GetType()
                .GetMethods()
                .FirstOrDefault(v => v.Name == (nameof(BaseContext.QueryCollection)) && 
                                     v.GetParameters().Length == 0);
            var genericMethod = methodInfo?.MakeGenericMethod(theType);
            var queryResult = (IEnumerable)genericMethod?.Invoke(context, Array.Empty<object>());

            foreach (var obj in queryResult)
            {
                var val= obj.GetType().GetProperty(propertyName).GetValue(obj);
                if (val.ToString().Equals(value))
                {
                    return new ValidationResult(@"Value allready exist");
                }
            }
            
            return null; 
        }
    }
}
