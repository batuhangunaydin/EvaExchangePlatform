using AutoMapper;
using EvaExchangePlatform.Business.Business;
using EvaExchangePlatform.Model.DTO;
using EvaExchangePlatform.Repository;
using EvaExchangePlatform.Repository.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRestAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BulkActionsController : ControllerBase
    {
        public IMapper _mapper;
        public ExchangeContext dbContext;
        public ExchangeManager exchangeManager;
        public TradersRepository tradersRepository;
        public TradersPortfoliosRepository tradersPortfoliosRepository;
        public RegisteredShareRepository registeredShareRepository;

        public BulkActionsController(ExchangeContext context, IMapper mapper)
        {
            _mapper = mapper;
            dbContext = context;
            exchangeManager = new(dbContext, _mapper);
            tradersRepository = new(dbContext, _mapper);
            tradersPortfoliosRepository = new(dbContext, _mapper);
        }

        /// <summary>
        /// Inserting traders and trader portfoilos records to database
        /// </summary>
        [HttpGet]
        public void InsertBulkData()
        {
            var trader = new TraderDTO()
            {
                Firstname = "Batuhan",
                Lastname = "Gunaydin",
                Email = "batuhan@test.com",
                Password = "123123",
                Balance = 50000
            };
            int traderIdDb = tradersRepository.Create(trader); 

            var portfolio = new PortfolioDTO();
            portfolio.traderId = traderIdDb;
            portfolio.ShareName = "EVA";
            portfolio.ShareCode = "EVA";
            portfolio.ShareAmount = 100;
            tradersPortfoliosRepository.Create(portfolio);

            portfolio.ShareName = "Tesla";
            portfolio.ShareCode = "TSL";
            portfolio.ShareAmount = 250;
            tradersPortfoliosRepository.Create(portfolio);

            portfolio.ShareName = "SkyNeb";
            portfolio.ShareCode = "SKY";
            portfolio.ShareAmount = 500;
            tradersPortfoliosRepository.Create(portfolio);


            trader = new TraderDTO()
            {
                Firstname = "Kubra",
                Lastname = "Gunaydin",
                Email = "kubra@test.com",
                Password = "123123",
                Balance = 100000
            };
            traderIdDb = tradersRepository.Create(trader); 

            portfolio.traderId = traderIdDb;
            portfolio.ShareName = "EVA";
            portfolio.ShareCode = "EVA";
            portfolio.ShareAmount = 500;
            tradersPortfoliosRepository.Create(portfolio);

            portfolio.ShareName = "Amazon";
            portfolio.ShareCode = "AMZ";
            portfolio.ShareAmount = 150;
            tradersPortfoliosRepository.Create(portfolio);

            portfolio.ShareName = "McDonalds";
            portfolio.ShareCode = "MCD";
            portfolio.ShareAmount = 50;
            tradersPortfoliosRepository.Create(portfolio);


            trader = new TraderDTO()
            {
                Firstname = "Ismail",
                Lastname = "Yurek",
                Email = "ismail@test.com",
                Password = "123123",
                Balance = 140000
            };
            traderIdDb = tradersRepository.Create(trader); 

            portfolio.traderId = traderIdDb;
            portfolio.ShareName = "Tesla";
            portfolio.ShareCode = "TSL";
            portfolio.ShareAmount = 230;
            tradersPortfoliosRepository.Create(portfolio);

            portfolio.ShareName = "Amazon";
            portfolio.ShareCode = "AMZ";
            portfolio.ShareAmount = 300;
            tradersPortfoliosRepository.Create(portfolio);

            portfolio.ShareName = "MercedesBenz";
            portfolio.ShareCode = "MBF";
            portfolio.ShareAmount = 100;
            tradersPortfoliosRepository.Create(portfolio);


            trader = new TraderDTO()
            {
                Firstname = "Luna",
                Lastname = "Gunaydin",
                Email = "luna@test.com",
                Password = "123123",
                Balance = 10000
            };
            traderIdDb = tradersRepository.Create(trader); 

            portfolio.traderId = traderIdDb;
            portfolio.ShareName = "Tesla";
            portfolio.ShareCode = "TSL";
            portfolio.ShareAmount = 100;
            tradersPortfoliosRepository.Create(portfolio);

            portfolio.ShareName = "SkyNeb";
            portfolio.ShareCode = "SKY";
            portfolio.ShareAmount = 50;
            tradersPortfoliosRepository.Create(portfolio);

            portfolio.ShareName = "McDonalds";
            portfolio.ShareCode = "MCD";
            portfolio.ShareAmount = 100;
            tradersPortfoliosRepository.Create(portfolio);
        }
    }
}
