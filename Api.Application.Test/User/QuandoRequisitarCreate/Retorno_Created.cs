using Api.Application.Controllers;
using Moq;
using Xunit;
using Api.Domain.Interfaces.Services.User;
using System.Threading.Tasks;
using Api.Domain.Dtos.User;
using System;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.Test.User.QuandoRequisitarCreate
{
    public class Retorno_Created
    {
        private UserController _controller;
        [Fact]
        public async Task E_Possivel_Invocar_a_Controller_Create()
        {
            var serviceMock = new Mock<IUserService>();
            var nome = Faker.Name.FullName();
            var email = Faker.Internet.Email();

            serviceMock.Setup(m => m.Post(It.IsAny<UserDtoCreate>())).ReturnsAsync(
                new UserDtoCreateResult
                {
                    Id = Guid.NewGuid(),
                    Nome = nome,
                    Email = email,
                    CreateAt = DateTime.UtcNow
                }

            );
            _controller = new UserController(serviceMock.Object);
            //_controller.ModelState.AddModelError("Nome","O campo é obrigatório !");
            
            Mock<IUrlHelper> url = new Mock<IUrlHelper>();
            url.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<Object>())).Returns("localhost:3000/teste");
            _controller.Url = url.Object;

            var userDtoCreate = new UserDtoCreate
            {
                Nome = nome,
                Email = email
            };

            var result = await _controller.Post(userDtoCreate);
            Assert.True(result is CreatedResult);

            var resultValue = ((CreatedResult)result).Value as UserDtoCreateResult;
            Assert.NotNull(resultValue);
            Assert.Equal(userDtoCreate.Nome, resultValue.Nome);
            Assert.Equal(userDtoCreate.Email, resultValue.Email);
        }

    }
}