using System.Collections.Generic;

namespace AngularStandaloneDemo.Models
{
    public class CustomErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }

        public CustomErrorResponse()
        {
            Errors = new List<string>();
        }

        public CustomErrorResponse(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
            Errors = new List<string>();
        }

        public CustomErrorResponse(int statusCode, string message, List<string> errors)
        {
            StatusCode = statusCode;
            Message = message;
            Errors = errors;
        }
    }
}