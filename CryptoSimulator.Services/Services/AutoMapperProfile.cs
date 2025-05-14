using AutoMapper;
using CryptoSimulator.DataContext.Entities;
using CryptoSimulator.DataContext.Dtos;

namespace CryptoSimulator.Services;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // User
        CreateMap<User, UserDto>();
        CreateMap<UserCreateDto, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());
        CreateMap<UserUpdateDto, User>();
        CreateMap<UserPassUpdateDto, User>();

        // Wallet
        CreateMap<Wallet, WalletDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Cryptocurrencies, opt => opt.Ignore()); // Populated manually
        CreateMap<WalletUpdateDto, Wallet>();

        // Cryptos
        CreateMap<CryptoCurrency, CryptoDto>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.TotalSupply, opt => opt.MapFrom(src => src.TotalSupply));
        CreateMap<CryptoCreateDto, CryptoCurrency>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.TotalSupply, opt => opt.MapFrom(src => src.TotalSupply));

        // Trade, Transactions
        CreateMap<Transaction, TradeDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
        CreateMap<TradeCreateDto, Transaction>()
            .ForMember(dest => dest.Price, opt => opt.Ignore())
            .ForMember(dest => dest.Type, opt => opt.Ignore())
            .ForMember(dest => dest.Timestamp, opt => opt.Ignore());

        CreateMap<Transaction, TransactionDto>()
            .ForMember(dest => dest.CryptoId, opt => opt.MapFrom(src => src.CryptoId))
            .ForMember(dest => dest.CryptoName, opt => opt.MapFrom(src => src.CryptoCurrency.Name))
            .ForMember(dest => dest.CryptoSymbol, opt => opt.MapFrom(src => src.CryptoCurrency.Symbol))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

        CreateMap<Transaction, TransactionDetailsDto>()
            .ForMember(dest => dest.CryptoId, opt => opt.MapFrom(src => src.CryptoId))
            .ForMember(dest => dest.CryptoName, opt => opt.MapFrom(src => src.CryptoCurrency.Name))
            .ForMember(dest => dest.CryptoSymbol, opt => opt.MapFrom(src => src.CryptoCurrency.Symbol))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

        // Portfolio
        CreateMap<TradeCreateDto, Portfolio>();

        CreateMap<Portfolio, PortfolioDto>()
            .ForMember(dest => dest.CryptoId, opt => opt.MapFrom(src => src.CryptoId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CryptoCurrency.Name))
            .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.CryptoCurrency.Symbol));

        // Price history
        CreateMap<PriceHistory, PriceHistoryDto>();
        CreateMap<PriceUpdateDto, PriceHistory>()
            .ForMember(dest => dest.CryptoId, opt => opt.MapFrom(src => src.CryptoId))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.NewPrice))
            .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}
