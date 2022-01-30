using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaExchangePlatform.Result
{
    #pragma warning disable CS1591

    public class Response : IResponse
    {
        public Response(bool success, string message) : this(success)
        {
            this.Message = message;
        }

        public Response(bool success)
        {
            this.Success = success;
        }

        public bool Success { get; }

        public string Message { get; }

    }
}
