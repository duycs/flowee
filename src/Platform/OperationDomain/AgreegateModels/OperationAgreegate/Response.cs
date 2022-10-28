using System;
namespace SpecificationDomain.AgreegateModels.OperationAgreegate
{
    public class Response
    {
        public int StatusCode { get; set; }

        /// <summary>
        /// Json result
        /// </summary>
        public string Data { get; set; }

        public static Response Create(int statusCode, string data)
        {
            return new Response()
            {
                StatusCode = statusCode,
                Data = data
            };
        }
    }
}

