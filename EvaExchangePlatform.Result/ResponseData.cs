using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaExchangePlatform.Result
{
    #pragma warning disable CS1591

    public class ResponseData<T> : Response, IResponseData<T>
    {
        public ResponseData(T data, bool success, string message) : base(success, message)
        {
            this.Data = data;
        }

        public ResponseData(T data, bool success) : base(success)
        {
            this.Data = data;
        }

        public T Data { get; }

    }
}

