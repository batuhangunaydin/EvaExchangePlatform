using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaExchangePlatform.Result
{
    #pragma warning disable CS1591

    public interface IResponseData<T> : IResponse
    {
        T Data { get; }
    }
}
