using System;
using AutoMapper;
using Api.CrossCutting.Mappings;

namespace Api.Service.Test
{
    public abstract class BaseTestService
    {
        public IMapper Mapper {get;set;}
        public BaseTestService()
        {
            Mapper = new AutoMapperFixture().GetMapper();
        }
        public class AutoMapperFixture : IDisposable
        {
            public IMapper GetMapper()
            {
                var config = new MapperConfiguration(config=>{
                config.AddProfile(new DtoToModelProfile());
                config.AddProfile(new EntityToDtoProfile());
                config.AddProfile(new ModelToEntityProfile());
                });
                return config.CreateMapper();

            } 
            
            public void Dispose()
            {

            }
        }
    }
}