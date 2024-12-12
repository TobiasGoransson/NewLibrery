using System;
using System.Collections.Generic;
using System.Linq;
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

        private OperationResult(bool isSuccessfull, T data, string message, string errorMessage)
        {
            
            Data = data;
            message = Message;
            errorMessage = ErrorMessage;
            Success = isSuccessfull;

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
