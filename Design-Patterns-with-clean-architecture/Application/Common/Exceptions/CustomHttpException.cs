using System;
using System.Net;

namespace Application.Common.Exceptions
{
    public class CustomHttpException : Exception
    {
        public int StatusCode { get; set; }
        public new string Message { get; set; }
        
        public CustomHttpException(HttpStatusCode httpStatusCode, string responseMessage)
        {
            StatusCode = (int)httpStatusCode;
            Message = responseMessage;
        }
    }
} 