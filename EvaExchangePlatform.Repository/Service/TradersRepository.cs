using AutoMapper;
using EvaExchangePlatform.Model.DTO;
using EvaExchangePlatform.Model.ExchangePlatform;
using EvaExchangePlatform.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaExchangePlatform.Repository
{
    public class TradersRepository
    {
        private readonly ExchangeContext dbContext;
        public IMapper _mapper;

        public TradersRepository(ExchangeContext context, IMapper mapper)
        {
            dbContext = context;
            _mapper = mapper;
        }


        /// <summary>
        /// Create a new Trader record in database
        /// </summary>
        /// <param name="traderDTO"></param>
        /// <returns></returns>
        public int Create(TraderDTO traderDTO)
        {
            if (traderDTO is null)
                return 0;

            var trader = _mapper.Map<Traders>(traderDTO);

            dbContext.Traders.Add(trader);
            dbContext.SaveChanges();

            int id = trader.Id;

            return id;
        }

        /// <summary>
        /// Function that returns list of all traders
        /// </summary>
        /// <returns></returns>
        public IList<Traders> Get()
        {
            var traders = dbContext.Traders.ToList();

            return traders;
        }

        /// <summary>
        /// Function that returns the trader's balance value based on the traderId value
        /// </summary>
        /// <param name="traderId"></param>
        /// <returns></returns>
        public double GetTraderBalance(int traderId)
        {
            double traderBalance = dbContext.Traders.Where(t => t.Id == traderId).FirstOrDefault().Balance;

            return traderBalance;
        }

        /// <summary>
        /// Function that updates the trader's balance and blocked balance values after registered share
        /// </summary>
        /// <param name="traderId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public void UpdateTradersBalanceForRegisterShare(int traderId, double amount)
        {
            var trader = dbContext.Traders.Find(traderId);

            //Calculating new balance values
            trader.Balance = trader.Balance - amount;
            trader.BlockedBalance = trader.BlockedBalance + amount;

            dbContext.Traders.Update(trader);
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Function that updates the buyer trader's balance after buy trade
        /// </summary>
        /// <param name="traderId"></param>
        /// <param name="amount"></param>
        public void UpdateBuyerBalanceForBuyTrade(int traderId, double amount)
        {
            var trader = dbContext.Traders.Find(traderId);

            trader.Balance = trader.Balance - amount;

            dbContext.Traders.Update(trader);
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Function that updates the buyer trader's balance after sell trade
        /// </summary>
        /// <param name="traderId"></param>
        /// <param name="amount"></param>
        public void UpdateBuyerBalanceForSellTrade(int traderId, double amount)
        {
            var trader = dbContext.Traders.Find(traderId);

            trader.BlockedBalance = trader.BlockedBalance - amount;

            dbContext.Traders.Update(trader);
            dbContext.SaveChanges();
        }


        /// <summary>
        /// Function that updates the seller trader's balance after buy trade
        /// </summary>
        /// <param name="traderId"></param>
        /// <param name="amount"></param>
        public void UpdateSellerBalanceForTrade(int traderId, double amount)
        {
            var trader = dbContext.Traders.Find(traderId);

            trader.Balance = trader.Balance + amount;

            dbContext.Traders.Update(trader);
            dbContext.SaveChanges();
        }

    }
}
