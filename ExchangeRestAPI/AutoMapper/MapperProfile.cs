using AutoMapper;
using EvaExchangePlatform.Model.DTO;
using EvaExchangePlatform.Model.ExchangePlatform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRestAPI
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            MapModels();
        }

        private void MapModels()
        {
            CreateMap<RegisteredShares, RegisterShareDTO>();
            CreateMap<RegisterShareDTO, RegisteredShares>();
            CreateMap<TransactionLogs, LogDTO>();
            CreateMap<LogDTO, TransactionLogs>();
            CreateMap<Traders, TraderDTO>();
            CreateMap<TraderDTO, Traders>();
            CreateMap<TradersPortfolios, PortfolioDTO>();
            CreateMap<PortfolioDTO, TradersPortfolios>();
        }
    }
}
