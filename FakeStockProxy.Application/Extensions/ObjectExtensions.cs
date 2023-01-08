using FakeStockProxy.Application.Miscellaneous.Json.Attributes;
using FakeStockProxy.Application.Miscellaneous.Json.ContractResolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FakeStockProxy.Application.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Computes hashsum based on objects properties marked with attribute of T type
        /// </summary>
        public static string GetHashsum<T>(this object inputObject) where T : Attribute
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CustomAttributeContractResolver<T>()
            };

            var serializedObject = JsonConvert.SerializeObject(inputObject, inputObject.GetType(), settings);

            var hashBytes = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(serializedObject));
            string retval = Convert.ToHexString(hashBytes);

            return retval;
        }
    }
}
