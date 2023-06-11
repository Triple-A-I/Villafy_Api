using System.Net;

namespace Villafy_Api.Models
{
    public class APIResponse
    {

        public APIResponse()
        {
            ErrorMessages = new List<string>();
        }
        public HttpStatusCode statusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; }

        public object Result { get; set; }
    }
}
