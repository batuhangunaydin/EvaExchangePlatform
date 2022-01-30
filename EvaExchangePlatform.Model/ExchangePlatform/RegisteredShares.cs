using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaExchangePlatform.Model.ExchangePlatform
{
    public class RegisteredShares
    {
        public int Id { get; set; } 
        public int traderId { get; set; }
        public string ShareName { get; set; }
        public string ShareCode { get; set; }
        public double RegisteredAmount { get; set; }
        public double SharePrice { get; set; }
        public string TradeSide { get; set; }
        public DateTime LastUpdateDate { get; set; }

    }
}
