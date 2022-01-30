using AutoMapper;
using EvaExchangePlatform.Core.Config;
using EvaExchangePlatform.Core.Helpers;
using EvaExchangePlatform.Model.DTO;
using EvaExchangePlatform.Model.ExchangePlatform;
using EvaExchangePlatform.Repository;
using EvaExchangePlatform.Repository.Context;
using EvaExchangePlatform.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaExchangePlatform.Business.Business
{
    public class ExchangeManager
    {
        public TradersRepository tradersRepository;
        public TradersPortfoliosRepository tradersPortfoliosRepository;
        public RegisteredShareRepository registeredRepository;
        public TransactionLogsRepository logsRepository;
        public ExchangeContext dbContext;

        public ExchangeManager(ExchangeContext context, IMapper mapper)
        {
            dbContext = context;
            registeredRepository = new(dbContext, mapper);
            tradersRepository = new(dbContext, mapper);
            tradersPortfoliosRepository = new(dbContext, mapper);
            logsRepository = new(dbContext, mapper);
        }

        /// <summary>
        /// Controls before function Register Share. Checking trader's balance, share amount etc.
        /// </summary>
        /// <param name="share"></param>
        /// <returns></returns>
        public Response RegisterShare(RegisterShareDTO share)
        {
            //Return an error message if any data is blank
            if (string.IsNullOrEmpty(share.ShareName) || string.IsNullOrEmpty(share.ShareCode) || string.IsNullOrEmpty(share.TradeSide))
                return new Response(false, Constants.emptyInput);

            //SQL Injection Checker
            if (CoreHelpers.SqlInjectionChecker(Convert.ToString(share.traderId)) ||
            CoreHelpers.SqlInjectionChecker(share.ShareName) ||
            CoreHelpers.SqlInjectionChecker(share.ShareCode) ||
            CoreHelpers.SqlInjectionChecker(Convert.ToString(share.RegisteredAmount)) ||
            CoreHelpers.SqlInjectionChecker(Convert.ToString(share.SharePrice)) ||
            CoreHelpers.SqlInjectionChecker(share.TradeSide))
            {
                return new Response(false, Constants.inputForbiddenWordError);
            }

            //TraderId Check
            if (share.traderId <= 0)
                return new Response(false, Constants.traderIdError);

            //Amount Check
            if (share.RegisteredAmount <= 0)
                return new Response(false, Constants.registerAmountError);

            //Price Check
            if (share.SharePrice <= 0)
                return new Response(false, Constants.priceError);

            //Trader portfolio check
            var isTruePortfolio = tradersPortfoliosRepository.CheckTraderPortfolio(share.traderId, share.ShareCode);

            if (!isTruePortfolio)
                return new Response(false, Constants.portfolioError);

            //If register transaction side buy, checking trader's balance 
            if (share.TradeSide == Constants.buySide)
            {
                double traderBalance = tradersRepository.GetTraderBalance(share.traderId); //Get trader balance from database
                double shareTotalBuyPrice = CoreHelpers.GetTotalSharePrice(share.RegisteredAmount, share.SharePrice);//Calculating total price

                if (traderBalance < shareTotalBuyPrice)
                    return new Response(false, Constants.traderBalanceError);
            }
            else if (share.TradeSide == Constants.sellSide) //If register transaction side sell, checking trader's share amount
            {
                double portfoliosShareAmount = tradersPortfoliosRepository.GetPortfiliosShareAmount(share.traderId, share.ShareCode);//Get trader's amount of share from their portfolios
                if (portfoliosShareAmount < share.RegisteredAmount)
                    return new Response(false, Constants.shareAmountError);
            }

            return new Response(true, "OK");
        }

        /// <summary>
        /// Controls before function of Update Price of Trader's Registered Share. Checking trader's balance, share amount, new price, last update time etc.
        /// </summary>
        /// <param name="updateShare"></param>
        /// <returns></returns>
        public Response UpdatePriceRegisteredShare(RegisteredShares updateShare)
        {
            //Return an error message if any data is blank
            if (string.IsNullOrEmpty(updateShare.ShareCode))
                return new Response(false, Constants.emptyInput);

            //SQL Injection Checker
            if (CoreHelpers.SqlInjectionChecker(Convert.ToString(updateShare.Id)) ||
            CoreHelpers.SqlInjectionChecker(Convert.ToString(updateShare.traderId)) ||
            CoreHelpers.SqlInjectionChecker(updateShare.ShareCode) ||
            CoreHelpers.SqlInjectionChecker(Convert.ToString(updateShare.SharePrice)))
            {
                return new Response(false, Constants.inputForbiddenWordError);
            }

            //Id Check
            if (updateShare.Id <= 0)
                return new Response(false, Constants.idError);

            //TraderId Check
            if (updateShare.traderId <= 0)
                return new Response(false, Constants.traderIdError);

            //Share Price Check
            if (updateShare.SharePrice <= 0)
                return new Response(false, Constants.priceError);

            //Registered share check in database
            var isTrueRegisteredShare = registeredRepository.CheckRegisteredShare(updateShare.Id, updateShare.traderId, updateShare.ShareCode);

            if (!isTrueRegisteredShare)
                return new Response(false, Constants.checkRegisteredShareError);//If there is a no record return error message

            var registeredShare = registeredRepository.GetById(updateShare.Id);

            //Last Update Date Check
            DateTime lastUpdateDate = registeredShare.LastUpdateDate;
            var differenceHours = (DateTime.Now - lastUpdateDate).TotalHours;

            if (differenceHours < 1)
                return new Response(false, Constants.updateRegisteredShareDateError);

            //If registered transaction side buy and new price is bigger than old price, check trader's balance
            if ((updateShare.SharePrice > registeredShare.SharePrice) && (registeredShare.TradeSide == Constants.buySide))
            {
                double sharePriceDifference = updateShare.SharePrice - registeredShare.SharePrice;
                double traderBalance = tradersRepository.GetTraderBalance(updateShare.traderId);//Get trader balance from database
                double shareTotalBuyPriceDifference = CoreHelpers.GetTotalSharePrice(registeredShare.RegisteredAmount, sharePriceDifference);//Calculating total price

                //if trader hasn't enough balance for update price
                if (traderBalance < shareTotalBuyPriceDifference)
                    return new Response(false, Constants.traderBalanceError);
            }

            return new Response(true, "OK");
        }

        /// <summary>
        /// Controls before function of Buy a Registered Share. Checking amount, price etc.
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        public Response BuyRegisteredShare(BuyTrade trade)
        {
            //Return an error message if any data is blank
            if (string.IsNullOrEmpty(trade.ShareCode))
                return new Response(false, Constants.emptyInput);

            //SQL Injection Checker
            if (CoreHelpers.SqlInjectionChecker(Convert.ToString(trade.RegisteredShareId)) ||
            CoreHelpers.SqlInjectionChecker(trade.ShareCode) ||
            CoreHelpers.SqlInjectionChecker(Convert.ToString(trade.BuyAmount)) ||
            CoreHelpers.SqlInjectionChecker(Convert.ToString(trade.BuyerTraderId)))
            {
                return new Response(false, Constants.inputForbiddenWordError);
            }

            //Registered Id Check
            if (trade.RegisteredShareId <= 0)
                return new Response(false,Constants.registeredShareIdError);

            //Buy Amount Check
            if (trade.BuyAmount <= 0)
                return new Response(false, Constants.registerAmountError);

            //Buyer Trader Id Check
            if (trade.BuyerTraderId <= 0)
                return new Response(false, Constants.traderIdError);

            //Trader portfolio check
            var isTruePortfolio = tradersPortfoliosRepository.CheckTraderPortfolio(trade.BuyerTraderId, trade.ShareCode);

            if (!isTruePortfolio)
                return new Response(false, Constants.portfolioError);

            //Get Registered Share Record for check
            RegisteredShares registeredShare = new();
            registeredShare = registeredRepository.GetById(trade.RegisteredShareId);
            
            //Record check
            if (registeredShare is null)
                return new Response(false, Constants.checkRegisteredShareRecordError);

            //Check registered share's trade side, Should be SELL for this trader!
            if (registeredShare.TradeSide != Constants.sellSide)
                return new Response(false, Constants.registeredShareBuySideError);

            //Check amount of registered share and trade amount
            if (registeredShare.RegisteredAmount < trade.BuyAmount)
                return new Response(false, Constants.amountCheckError);

            //Check trader's balance for this transaction
            double traderBalance = tradersRepository.GetTraderBalance(trade.BuyerTraderId);//Get trader's balance from database
            double totalAmount = Math.Round((Double)(registeredShare.SharePrice * trade.BuyAmount), 2);//Calculate total price of registered share

            if (traderBalance < totalAmount)
                return new Response(false, Constants.traderBalanceError);


            return new Response(true, "OK");
        }

        /// <summary>
        /// Controls before function of Sell a Registered Share. Checking amount, price etc.
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        public Response SellRegisteredShare(SellTrade trade)
        {
            //Return an error message if any data is blank
            if (string.IsNullOrEmpty(trade.ShareCode))
                return new Response(false, Constants.emptyInput);

            //SQL Injection Checker
            if (CoreHelpers.SqlInjectionChecker(Convert.ToString(trade.RegisteredShareId)) ||
            CoreHelpers.SqlInjectionChecker(trade.ShareCode) ||
            CoreHelpers.SqlInjectionChecker(Convert.ToString(trade.SellAmount)) ||
            CoreHelpers.SqlInjectionChecker(Convert.ToString(trade.SellerTraderId)))
            {
                return new Response(false, Constants.inputForbiddenWordError);
            }

            //Registered Id Check
            if (trade.RegisteredShareId <= 0)
                return new Response(false, Constants.registeredShareIdError);

            //Buy Amount Check
            if (trade.SellAmount <= 0)
                return new Response(false, Constants.registerAmountError);

            //Buyer Trader Id Check
            if (trade.SellerTraderId <= 0)
                return new Response(false, Constants.traderIdError);

            //Trader portfolio check
            var isTruePortfolio = tradersPortfoliosRepository.CheckTraderPortfolio(trade.SellerTraderId, trade.ShareCode);

            if (!isTruePortfolio)
                return new Response(false, Constants.portfolioError);

            //Get Registered Share Record for check
            RegisteredShares registeredShare = new();
            registeredShare = registeredRepository.GetById(trade.RegisteredShareId);

            //Record check
            if (registeredShare is null)
                return new Response(false, Constants.checkRegisteredShareRecordError);

            //Check registered share's trade side, Should be BUY for this trader!
            if (registeredShare.TradeSide != Constants.buySide)
                return new Response(false, Constants.registeredShareSellSideError);

            //Check amount of registered share and trade amount
            if (registeredShare.RegisteredAmount < trade.SellAmount)
                return new Response(false, Constants.amountCheckError);

            //Check trader's portfolio for this transaction
            double shareAmount = tradersPortfoliosRepository.GetPortfiliosShareAmount(trade.SellerTraderId, trade.ShareCode);//Get trader's share amount from portfolio in database

            if (shareAmount < trade.SellAmount)
                return new Response(false, Constants.amountSellError);

            return new Response(true, "OK");
        }


    }
}
