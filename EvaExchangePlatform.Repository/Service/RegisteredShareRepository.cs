using EvaExchangePlatform.Repository.Context;
using EvaExchangePlatform.Model.ExchangePlatform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvaExchangePlatform.Model.DTO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace EvaExchangePlatform.Repository
{
    public class RegisteredShareRepository
    {
        public ExchangeContext dbContext;
        public IMapper _mapper;

        public RegisteredShareRepository(ExchangeContext context, IMapper mapper)
        {
            dbContext = context;
            _mapper = mapper;
        }


        /// <summary>
        /// Function that returns list of all registered shares of traders
        /// </summary>
        /// <returns></returns>
        public IList<RegisteredShares> Get()
        {
            var registeredShares = dbContext.RegisteredShares.ToList();

            return registeredShares;
        }

        /// <summary>
        /// Create a new RegisterShare record in database
        /// </summary>
        /// <param name="shareDTO"></param>
        /// <returns></returns>
        public int Create(RegisterShareDTO shareDTO)
        {
            if (shareDTO is null)
                return 0;

            var share = _mapper.Map<RegisteredShares>(shareDTO);

            dbContext.RegisteredShares.Add(share);
            dbContext.SaveChanges();

            int id = share.Id;

            return id;
        }

        /// <summary>
        /// Function that updates the price and last update date values of a registered share
        /// </summary>
        /// <param name="registeredShare"></param>
        /// <returns></returns>
        public bool UpdatePriceAndUpdateDate(RegisteredShares registeredShare)
        {
            var registeredShareCheck = dbContext.RegisteredShares.Where(t => t.Id == registeredShare.Id).FirstOrDefault();

            registeredShareCheck.SharePrice = registeredShare.SharePrice;
            registeredShareCheck.LastUpdateDate = DateTime.Now;

            dbContext.RegisteredShares.Update(registeredShareCheck);
            int updatedRow = dbContext.SaveChanges();

            if(updatedRow > 0)
                return true;

            return false;
        }

        /// <summary>
        /// Get a record from database with an id key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         public RegisteredShares GetById(int id)
         {
             var registeredShare = dbContext.RegisteredShares.Where(t => t.Id == id).FirstOrDefault();

             return registeredShare;
         }
        
        /// <summary>
        /// Delete a record from database with an id key
        /// </summary>
        /// <param name="shareId"></param>
        /// <returns></returns>
        public void DeleteById(int shareId)
        {
            var registeredShare = dbContext.RegisteredShares.Where(t => t.Id == shareId).FirstOrDefault();

            dbContext.RegisteredShares.Remove(registeredShare);
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Function that checks if the trader has a registered share of the relevant shareCode
        /// </summary>
        /// <param name="id"></param>
        /// <param name="traderId"></param>
        /// <param name="shareCode"></param>
        /// <returns></returns>
        public bool CheckRegisteredShare(int id, int traderId, string shareCode)
        {
            var hasRegisteredShare = dbContext.RegisteredShares.Where(t => t.Id == id && t.traderId == traderId && t.ShareCode == shareCode).FirstOrDefault();

            if (hasRegisteredShare is null)
                return false;

            return true;
        }

        /// <summary>
        /// Function that checks if the database has a registered share of the relevant id and shareCode
        /// </summary>
        /// <param name="id"></param>
        /// <param name="shareCode"></param>
        /// <returns></returns>
        public bool CheckRegisteredShareForOrder(int id, string shareCode)
        {
            var hasRegisteredShare = dbContext.RegisteredShares.Where(t => t.Id == id && t.ShareCode == shareCode).FirstOrDefault();

            if (hasRegisteredShare is null)
                return false;

            return true;
        }

        /// <summary>
        /// Get a last price value of registered share in database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double GetLastPriceOfRegisteredShare(int id)
        {
            double lastPrice = dbContext.RegisteredShares.Where(t => t.Id == id).FirstOrDefault().SharePrice;

            return lastPrice;
        }

        /// <summary>
        /// Get a trade side of registered share in database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTradeSideOfRegisteredShare(int id)
        {
            string tradeSide = dbContext.RegisteredShares.Where(t => t.Id == id).FirstOrDefault().TradeSide;

            return tradeSide;
        }

        /// <summary>
        /// Get a amount of registered share in database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double GetAmountOfRegisteredShare(int id)
        {
            double amount = dbContext.RegisteredShares.Where(t => t.Id == id).FirstOrDefault().RegisteredAmount;

            return amount;
        }

        /// <summary>
        /// Update registered share's amount
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        public void UpdateRegisteredShareAmount(int id, double amount)
        {
            var registeredShare = dbContext.RegisteredShares.Where(t => t.Id == id).FirstOrDefault();

            registeredShare.RegisteredAmount = registeredShare.RegisteredAmount - amount;

            dbContext.RegisteredShares.Update(registeredShare);
            dbContext.SaveChanges();
        }


    }
}
