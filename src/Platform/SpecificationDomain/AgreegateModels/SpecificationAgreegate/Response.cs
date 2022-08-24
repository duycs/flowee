using System;
namespace SpecificationDomain.AgreegateModels.SpecificationAgreegate
{
    public class Response
    {
        public int StatusCode { get; set; }

        /// <summary>
        /// Json result
        /// </summary>
        public string Result { get; set; }

        public static Response Create(int statusCode, string result)
        {
            return new Response()
            {
                StatusCode = statusCode,
                Result = result
            };
        }
    }
}

