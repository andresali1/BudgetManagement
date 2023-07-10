using AutoMapper;
using BudgetManagement.Models;

namespace BudgetManagement.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, AccountCreationViewModel>();
            CreateMap<Category, CategoryCreationViewModel>();
            CreateMap<DealUpdateViewModel, Deal>().ReverseMap();
        }
    }
}
