using Api.Domain.Dtos.Models;
using Api.Domain.Entities;
using AutoMapper;

namespace Api.CrossCutting.Mappings
{
    public class ModelToEntityProfile : Profile
    {
        public ModelToEntityProfile()
        {
             CreateMap<UserEntity,UserModel>()
            .ReverseMap();
        }
    }
}