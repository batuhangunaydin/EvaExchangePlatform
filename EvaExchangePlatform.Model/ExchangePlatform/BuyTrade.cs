using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaExchangePlatform.Model.ExchangePlatform
{
    public class BuyTrade
    {
        public int RegisteredShareId { get; set; }
        public string ShareCode { get; set; }
        public double BuyAmount { get; set; }
        public int BuyerTraderId { get; set; }

    }
}
