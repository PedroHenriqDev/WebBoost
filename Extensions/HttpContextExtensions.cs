using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebBoost.Extensions
{
    public static class HttpContextExtensions
    {
        public static async Task<string> ReadBodyAsStringAsync(this HttpContext httpContext)
        {
            Stream body = httpContext.Request.Body;

            string result = string.Empty;   
            
            using (var sr = new StreamReader(body, Encoding.UTF8))
                result = await sr.ReadToEndAsync();

            return result;
        }
    }
}
