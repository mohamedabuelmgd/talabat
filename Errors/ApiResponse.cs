using System;

namespace Talabat.APIs.Errors
{
    public class ApiResponse
    {
        public int StatuseCode { get; set; }
        public string Message { get; set; }
        public ApiResponse(int statuseCode, string message=null)
        {
            StatuseCode = statuseCode;
            Message = message?? GetDefaultMessageForStatusCode(statuseCode);
        }

        private string GetDefaultMessageForStatusCode(int statuseCode)
        {
            return statuseCode switch
            {
                400 => "A bad request, you have made",
                401 => "Authorized, you are not",
                404 => "Resource found, it was not",
                500 => "Errors are the path to the dark side, errors lead to anger, anger lead to hate",
                _ => null
            };
        }
    }
}
