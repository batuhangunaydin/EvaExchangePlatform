using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaExchangePlatform.Model.ExchangePlatform
{
    public class SellTrade
    {
        public int RegisteredShareId { get; set; }
        public string ShareCode { get; set; }
        public double SellAmount { get; set; }
        public int SellerTraderId { get; set; }

    }
}
