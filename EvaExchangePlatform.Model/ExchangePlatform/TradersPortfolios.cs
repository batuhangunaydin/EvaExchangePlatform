using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaExchangePlatform.Model.ExchangePlatform
{
    public class TradersPortfolios
    {
        public int Id { get; set; }
        public int traderId { get; set; }
        public string ShareName { get; set; }
        public string ShareCode { get; set; }
        public double ShareAmount { get; set; }
        public double ShareBlockedAmount { get; set; }

    }
}
