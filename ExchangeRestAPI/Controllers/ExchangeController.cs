using EvaExchangePlatform.Result;
using EvaExchangePlatform.Business.Business;
using EvaExchangePlatform.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvaExchangePlatform.Core.Config;
using EvaExchangePlatform.Model.ExchangePlatform;
using EvaExchangePlatform.Repository.Context;
using EvaExchangePlatform.Repository;
using EvaExchangePlatform.Model.DTO;
using AutoMapper;

namespace ExchangeRestAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        public IMapper _mapper;
        public ExchangeContext dbContext;
        public ExchangeManager exchangeManager;
        public TradersRepository tradersRepository;
        public TradersPortfoliosRepository tradersPortfoliosRepository;
        public RegisteredShareRepository registeredShareRepository;
        public TransactionLogsRepository transactionLogsRepository;

        public ExchangeController(ExchangeContext context, IMapper mapper)
        {
            _mapper = mapper;
            dbContext = context;
            exchangeManager = new(dbContext, _mapper);
            tradersRepository = new(dbContext, _mapper);
            tradersPortfoliosRepository = new(dbContext, _mapper);
            registeredShareRepository = new(dbContext, _mapper);
            transactionLogsRepository = new(dbContext, _mapper);
        }

        /// <summary>
        /// Function that allows the get all traders records
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IList<Traders> GetAllTraders()
        {
            return tradersRepository.Get();
        }

        /// <summary>
        /// Function that allows the get all portfolios records
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IList<TradersPortfolios> GetAllPortfolios()
        {
            return tradersPortfoliosRepository.Get();
        }

        /// <summary>
        /// Function that allows the get all registered shares
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IList<RegisteredShares> GetAllRegisteredShares()
        {
            return registeredShareRepository.Get();
        }

        /// <summary>
        /// Function that allows the get all transaction logs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IList<TransactionLogs> GetAllLogs()
        {
            return transactionLogsRepository.Get();
        }

        /// <summary>
        /// Function that allows the trader to record the stock their owns
        /// </summary>
        /// <param name="share"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponse RegisterShare([FromBody]RegisterShareDTO share)
        {
            var inputControls = exchangeManager.RegisterShare(share);//Initial checks (SQL Injection, Empty Data etc.)

            if (!inputControls.Success) //If an error occurred in any of the initial checks, we return the error.
                return inputControls;

            int registerShareId = registeredShareRepository.Create(share); //The request is being created in the database
            
            if (registerShareId > 0) //If register was successful
            {
                if (share.TradeSide == Constants.buySide)//If register side is buy, update trader's balance and blocked balance
                {
                    double totalAmount = share.RegisteredAmount * share.SharePrice;
                    tradersRepository.UpdateTradersBalanceForRegisterShare(share.traderId, Math.Round((Double)totalAmount, 2));
                    return new Response(true, Constants.registerShareSuccessful);
                }
                else if (share.TradeSide == Constants.sellSide)//If register side is sell, update trader's share amount
                {
                    tradersPortfoliosRepository.UpdateTradersPortfolioForRegisterShare(share.traderId, share.ShareCode, Math.Round((Double)share.RegisteredAmount, 2));
                    return new Response(true, Constants.registerShareSuccessful);
                }
            }

            return new Response(false, Constants.shareRegisterError);
        }

        /// <summary>
        /// Function that allows the trader to update a registered share
        /// </summary>
        /// <param name="updateShare"></param>
        /// <returns></returns>
        [HttpPut]
        public IResponse UpdatePriceRegisteredShare([FromBody]RegisteredShares updateShare)
        {
            var inputControls = exchangeManager.UpdatePriceRegisteredShare(updateShare);//Initial checks (SQL Injection, Empty Data etc.)

            if (!inputControls.Success) //If an error occurred in any of the initial checks, we return the error.
                return inputControls;

            //Get last values of registered share
            RegisteredShares registeredShare = registeredShareRepository.GetById(updateShare.Id);
            double oldPrice = registeredShare.SharePrice;
            DateTime oldUpdateDate = registeredShare.LastUpdateDate;

            //Update registered share's price and lastUpdateDate
            bool updateRegisteredShareRequest = registeredShareRepository.UpdatePriceAndUpdateDate(updateShare);

            //If update successfull check trade side
            if (updateRegisteredShareRequest)
            {
                //if trade side is BUY, update trader's balance and blocked balance values
                if(updateShare.TradeSide == Constants.buySide)
                {
                    //double priceDifference = updateShare.SharePrice - registeredShare.SharePrice; // Didn't work!! After UpdatePrice, the registeredshare object gets the same values as updatesshare.
                    double priceDifference = updateShare.SharePrice - oldPrice;
                    double totalAmount = priceDifference * updateShare.RegisteredAmount;
                    tradersRepository.UpdateTradersBalanceForRegisterShare(updateShare.traderId, Math.Round((Double)totalAmount, 2));

                    return new Response(true, Constants.updateShareSuccessful);
                }
            }

            return new Response(false, Constants.updateRegisteredShareError);
        }
        

        [HttpPost]
        public IResponse BuyRegisteredShare([FromBody]BuyTrade trade)
        {
            var inputControls = exchangeManager.BuyRegisteredShare(trade);//Initial checks (SQL Injection, Empty Data etc.)

            if (!inputControls.Success) //If an error occurred in any of the initial checks, we return the error.
                return inputControls;

            RegisteredShares registeredShare = new();
            registeredShare = registeredShareRepository.GetById(trade.RegisteredShareId);

            int sellerId = registeredShare.traderId;
            double registeredShareAmount = registeredShare.RegisteredAmount;
            double lastPrice = registeredShare.SharePrice;

            double totalPrice = Math.Round((Double)(trade.BuyAmount * lastPrice), 2);

            //Update buyer trader's portfolio
            tradersPortfoliosRepository.UpdateBuyerPortfolioForBuyTrade(trade.BuyerTraderId, trade.ShareCode, trade.BuyAmount);

            //Update buyer trader's balance
            tradersRepository.UpdateBuyerBalanceForBuyTrade(trade.BuyerTraderId, totalPrice);


            //Update seller trader's portfolio
            tradersPortfoliosRepository.UpdateSellerPortfolioForBuyTrade(sellerId, trade.ShareCode, trade.BuyAmount);

            //Update seller trader's balance
            tradersRepository.UpdateSellerBalanceForTrade(sellerId, totalPrice);

            //Update registered share record
            if (registeredShareAmount == trade.BuyAmount)
            {
                registeredShareRepository.DeleteById(trade.RegisteredShareId);
            }
            else if (registeredShareAmount > trade.BuyAmount)
            {
                registeredShareRepository.UpdateRegisteredShareAmount(trade.RegisteredShareId, trade.BuyAmount);
            }

            //Create traders transactions logs
            LogDTO buyerLog = new()
            {
                traderId = trade.BuyerTraderId,
                ShareCode = trade.ShareCode,
                ShareAmount = trade.BuyAmount,
                SharePrice = lastPrice,
                TradeSide = Constants.buySide,
                Status = Constants.success
            };
            transactionLogsRepository.Create(buyerLog);

            LogDTO sellerLog = new()
            {
                traderId = sellerId,
                ShareCode = trade.ShareCode,
                ShareAmount = trade.BuyAmount,
                SharePrice = lastPrice,
                TradeSide = Constants.sellSide,
                Status = Constants.success
            };
            transactionLogsRepository.Create(sellerLog);

            return new Response(true, Constants.buyTradeSuccessful);
        }


        [HttpPost]
        public IResponse SellRegisteredShare([FromBody]SellTrade trade)
        {
            var inputControls = exchangeManager.SellRegisteredShare(trade);//Initial checks (SQL Injection, Empty Data etc.)

            if (!inputControls.Success) //If an error occurred in any of the initial checks, we return the error.
                return inputControls;

            RegisteredShares registeredShare = new();
            registeredShare = registeredShareRepository.GetById(trade.RegisteredShareId);

            int buyerId = registeredShare.traderId;
            double registeredShareAmount = registeredShare.RegisteredAmount;
            double lastPrice = registeredShare.SharePrice;

            double totalPrice = Math.Round((Double)(trade.SellAmount * lastPrice), 2);

            //Update seller trader's portfolio
            tradersPortfoliosRepository.UpdateSellerPortfolioForSellTrade(trade.SellerTraderId, trade.ShareCode, trade.SellAmount);

            //Update seller trader's balance
            tradersRepository.UpdateSellerBalanceForTrade(trade.SellerTraderId, totalPrice);


            //Update buyer trader's portfolio
            tradersPortfoliosRepository.UpdateBuyerPortfolioForBuyTrade(buyerId, trade.ShareCode, trade.SellAmount);

            //Update buyer trader's balance
            tradersRepository.UpdateBuyerBalanceForSellTrade(buyerId, totalPrice);

            //Update registered share record
            if (registeredShareAmount == trade.SellAmount)
            {
                registeredShareRepository.DeleteById(trade.RegisteredShareId);
            }
            else if (registeredShareAmount > trade.SellAmount)
            {
                registeredShareRepository.UpdateRegisteredShareAmount(trade.RegisteredShareId, trade.SellAmount);
            }

            //Create traders transactions logs
            LogDTO sellerLog = new()
            {
                traderId = trade.SellerTraderId,
                ShareCode = trade.ShareCode,
                ShareAmount = trade.SellAmount,
                SharePrice = lastPrice,
                TradeSide = Constants.sellSide,
                Status = Constants.success
            };
            transactionLogsRepository.Create(sellerLog);

            LogDTO buyerLog = new()
            {
                traderId = buyerId,
                ShareCode = trade.ShareCode,
                ShareAmount = trade.SellAmount,
                SharePrice = lastPrice,
                TradeSide = Constants.buySide,
                Status = Constants.success
            };
            transactionLogsRepository.Create(buyerLog);

            return new Response(true, Constants.sellTradeSuccessful);
        }
    }
}
