

using AutoMapper;
using CriptoBank.Application.DTOs.UserToken;
using CriptoBank.Domain.Models;

namespace CriptoBank.Application.AutoMapperProfile
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterRequestDTO, User>()
                .ConstructUsing(src =>
                    new User(src.Name, src.Email, src.Password));
        }
    }
}
