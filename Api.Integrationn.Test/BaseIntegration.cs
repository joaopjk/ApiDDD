using System;
using System.Net.Http;
using System.Threading.Tasks;
using Api.CrossCutting.Mappings;
using Api.Data.Context;
using Api.Domain.Dtos;
using App;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Api.Integrationn.Test
{
    public abstract class BaseIntegration : IDisposable
    {

        public MyContext myContext { get; private set; }
        public HttpClient client { get; private set; }
        public IMapper mapper { get; private set; }
        public string hostAPI { get; private set; }
        public HttpResponseMessage respose { get; private set; }
        public BaseIntegration()
        {
            hostAPI = "http://localhost:5000/api/";
            var builder = new WebHostBuilder()
            .UseEnvironment("Testing")
            .UseStartup<Startup>();
            var server = new TestServer(builder);

            myContext = server.Host.Services.GetService(typeof(MyContext)) as MyContext;
            myContext.Database.Migrate();

            mapper = new AutoMapperFixture().GetMapper();
            client = server.CreateClient();
        }
        public async Task AdicionarToken()
        {
            var loginDto = new LoginDto()
            {
                Email = "joao@joao.com.br"
            };
            var resultLogin = await PostJasonAsync(loginDto,$"{hostAPI}login",client);
            var jsonLogin = await resultLogin.Content.ReadAsStringAsync();
            var loginObject = JsonConvert.DeserializeObject<LoginResponseDto>(jsonLogin);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginObject.accessToken);
        }
        public static async Task<HttpResponseMessage> PostJasonAsync(object dataclass, string url, HttpClient client)
        {
            return await client.PostAsync(url,
            new StringContent(JsonConvert.SerializeObject(dataclass), System.Text.Encoding.UTF8, "application/json"));
        }
        public void Dispose()
        {
            myContext.Dispose();
            client.Dispose();
        }
    }
    public class AutoMapperFixture : IDisposable
    {
        public IMapper GetMapper()
        {
            var config = new AutoMapper.MapperConfiguration(config =>
            {
                config.AddProfile(new DtoToModelProfile());
                config.AddProfile(new EntityToDtoProfile());
                config.AddProfile(new ModelToEntityProfile());
            });
            return config.CreateMapper();
        }
        public void Dispose() { }

    }
}