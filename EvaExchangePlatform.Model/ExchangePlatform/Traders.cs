using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaExchangePlatform.Model.ExchangePlatform
{
    public class Traders
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public double Balance { get; set; }
        public double BlockedBalance { get; set; }
        public DateTime RegisteredDate { get; set; }

    }
}
