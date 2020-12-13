using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Domain.Dtos.User;
using Newtonsoft.Json;
using Xunit;

namespace Api.Integrationn.Test.User
{
    public class QuandoRequisitarUser : BaseIntegration
    {
        private string _nome { get; set; }
        private string _email { get; set; }

        [Fact]
        public async Task E_Possivel_Realizar_Crud_Usuario()
        {
            await AdicionarToken();
            _nome = Faker.Name.First();
            _email = Faker.Internet.Email();

            var userDto = new UserDtoCreate()
            {
                Nome = _nome,
                Email = _email
            };

            //Post
            var response = await PostJasonAsync(userDto, $"{hostAPI}user", client);
            var postResult = await response.Content.ReadAsStringAsync();
            var registroPost = JsonConvert.DeserializeObject<UserDtoCreateResult>(postResult);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(_nome, registroPost.Nome);
            Assert.Equal(_email, registroPost.Email);
            Assert.NotNull(registroPost.Id);
            Assert.True(registroPost.Id != default(Guid));

            //GetAll
            response = await client.GetAsync($"{hostAPI}user");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonResult = await response.Content.ReadAsStringAsync();
            var listJson = JsonConvert.DeserializeObject<IEnumerable<UserDto>>(jsonResult);
            Assert.NotNull(listJson);
            Assert.True(listJson.Count() > 0);
            Assert.True(listJson.Where(l => l.Id == registroPost.Id).Count() == 1);
        }
    }
}