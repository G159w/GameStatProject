using System;

namespace DotnetProjectBack.Models
{
    public class GenericResponse<T>
    {
        public bool Success { get; set; }
        public T Result { get; set; }
        public Exception Exception { get; set; }
        public string ErrorMessage { get; set; }

        public GenericResponse(string error, Exception exception)
        {
            Exception = exception;
            Success = false;
            Result = default(T);
            ErrorMessage = error;
        }

        public GenericResponse(T result)
        {
            Exception = null;
            ErrorMessage = null;
            Success = true;
            Result = result;
        }
    }
}
