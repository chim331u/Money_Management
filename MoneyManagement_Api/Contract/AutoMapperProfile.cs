using AutoMapper;
using MoneyManagement_Api.Models.Transactions;
using MoneyManagement_Data.DTOs;

namespace MoneyManagement_Api.Contract;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Transaction, TransactionDto>();
        //.ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account.Name))
    }
}