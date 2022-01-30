using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaExchangePlatform.Model.ExchangePlatform
{
    public class TransactionLogs
    {
        public int Id { get; set; }
        public int traderId { get; set; }
        public string ShareCode { get; set; }
        public double ShareAmount { get; set; }
        public double SharePrice { get; set; }
        public string TradeSide { get; set; }
        public string Status { get; set; }
        public DateTime TransactionDate { get; set; }

    }
}
