using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{

    public class OperationResult<T>
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public T Data { get; private set; }
        public string ErrorMessage { get; private set; }

        private OperationResult(bool success, T data, string message, string errorMessage)
        {
            Success = success;
            Data = data;
            Message = message;
            ErrorMessage = errorMessage;
        }

        public static OperationResult<T> Successfull(T data, string message = "Operation Successful")
        {
            return new OperationResult<T>(true, data, message, null);
        }

        public static OperationResult<T> Failure(string errorMessage, string message = "Operation Failed")
        {
            return new OperationResult<T>(false, default, message, errorMessage);
        }
        
    }

}
