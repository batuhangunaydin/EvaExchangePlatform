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
    public class TransactionLogsRepository
    {
        public ExchangeContext dbContext;
        public IMapper _mapper;

        public TransactionLogsRepository(ExchangeContext context, IMapper mapper)
        {
            dbContext = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Function that returns list of all transaction logs
        /// </summary>
        /// <returns></returns>
        public IList<TransactionLogs> Get()
        {
            var transactionLogs = dbContext.TransactionLogs.ToList();

            return transactionLogs;
        }

        /// <summary>
        /// Create a new Log record in database
        /// </summary>
        /// <param name="logDTO"></param>
        /// <returns></returns>
        public int Create(LogDTO logDTO)
        {
            if (logDTO is null)
                return 0;

            var log = _mapper.Map<TransactionLogs>(logDTO);

            dbContext.TransactionLogs.Add(log);
            dbContext.SaveChanges();

            int id = log.Id;

            return id;
        }
    }
}
