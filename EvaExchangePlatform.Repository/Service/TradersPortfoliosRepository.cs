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
    public class TradersPortfoliosRepository
    {
        private readonly ExchangeContext dbContext;
        public IMapper _mapper;

        public TradersPortfoliosRepository(ExchangeContext context, IMapper mapper)
        {
            dbContext = context;
            _mapper = mapper;
        }


        public void Create(PortfolioDTO portfolioDTO)
        {
            var portfolio = _mapper.Map<TradersPortfolios>(portfolioDTO);

            dbContext.TradersPortfolios.Add(portfolio);
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Function that returns list of all traders portfolios
        /// </summary>
        /// <returns></returns>
        public IList<TradersPortfolios> Get()
        {
            var tradersPortfolios = dbContext.TradersPortfolios.ToList();

            return tradersPortfolios;
        }

        /// <summary>
        /// Function that checks whether the relevant share is in the trader's portfolio
        /// </summary>
        /// <param name="traderId"></param>
        /// <param name="shareCode"></param>
        /// <returns></returns>
        public bool CheckTraderPortfolio(int traderId, string shareCode)
        {
            var hasPortfolio = dbContext.TradersPortfolios.Where(t => t.traderId == traderId && t.ShareCode == shareCode).FirstOrDefault();

            if (hasPortfolio is null)
                return false;

            return true;
        }

        /// <summary>
        /// Function that returns the amount of the relevant share in the trader's portfolio
        /// </summary>
        /// <param name="traderId"></param>
        /// <param name="shareCode"></param>
        /// <returns></returns>
        public double GetPortfiliosShareAmount(int traderId, string shareCode)
        {
            double tradersShareAmount = dbContext.TradersPortfolios.Where(t => t.traderId == traderId && t.ShareCode == shareCode).FirstOrDefault().ShareAmount;

            return tradersShareAmount;
        }

        /// <summary>
        /// Function that updates the amount and blocked amount values of the share in the trader's portfolio
        /// </summary>
        /// <param name="traderId"></param>
        /// <param name="shareCode"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public void UpdateTradersPortfolioForRegisterShare(int traderId, string shareCode, double amount)
        {
            var traderPorfolio = dbContext.TradersPortfolios.Where(t => t.traderId == traderId && t.ShareCode == shareCode).FirstOrDefault();

            //Calculating new amount values
            traderPorfolio.ShareAmount = traderPorfolio.ShareAmount - amount;
            traderPorfolio.ShareBlockedAmount = traderPorfolio.ShareBlockedAmount + amount;

            dbContext.TradersPortfolios.Update(traderPorfolio);
            dbContext.SaveChanges();

        }

        /// <summary>
        /// Function that updates the amount of the share in the buyer trader's portfolio after the trade
        /// </summary>
        /// <param name="traderId"></param>
        /// <param name="shareCode"></param>
        /// <param name="amount"></param>
        public void UpdateBuyerPortfolioForBuyTrade(int traderId, string shareCode, double amount)
        {
            var traderPortfolio = dbContext.TradersPortfolios.Where(t => t.traderId == traderId && t.ShareCode == shareCode).FirstOrDefault();

            traderPortfolio.ShareAmount = traderPortfolio.ShareAmount + amount;

            dbContext.TradersPortfolios.Update(traderPortfolio);
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Function that updates the blocked amount of the share in the seller trader's portfolio after the buy trade 
        /// </summary>
        /// <param name="traderId"></param>
        /// <param name="shareCode"></param>
        /// <param name="amount"></param>
        public void UpdateSellerPortfolioForBuyTrade(int traderId, string shareCode, double amount)
        {
            var traderPortfolio = dbContext.TradersPortfolios.Where(t => t.traderId == traderId && t.ShareCode == shareCode).FirstOrDefault();

            traderPortfolio.ShareBlockedAmount = Math.Round((Double)(traderPortfolio.ShareBlockedAmount - amount), 2);

            dbContext.TradersPortfolios.Update(traderPortfolio);
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Function that updates the blocked amount of the share in the seller trader's portfolio after the sell trade 
        /// </summary>
        /// <param name="traderId"></param>
        /// <param name="shareCode"></param>
        /// <param name="amount"></param>
        public void UpdateSellerPortfolioForSellTrade(int traderId, string shareCode, double amount)
        {
            var traderPortfolio = dbContext.TradersPortfolios.Where(t => t.traderId == traderId && t.ShareCode == shareCode).FirstOrDefault();

            traderPortfolio.ShareAmount = Math.Round((Double)(traderPortfolio.ShareAmount - amount), 2);

            dbContext.TradersPortfolios.Update(traderPortfolio);
            dbContext.SaveChanges();
        }
    }
}
