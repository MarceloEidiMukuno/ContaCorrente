using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ContaCorrente.ApiDebito.Extensions
{
    public static class ModelStateExtension
    {
        public static List<string> GetErros(this ModelStateDictionary model)
        {
            var result = new List<string>();
            foreach (var item in model.Values)
            {

                foreach (var error in item.Errors)
                {

                    result.Add(error.ErrorMessage);

                }
            }
            return result;

        }
    }
}