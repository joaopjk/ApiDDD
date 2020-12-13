using Api.Domain.Interfaces.Services.User;
using Api.Service.Test.User;
using Moq;
using Xunit;
using System.Threading.Tasks;

namespace Api.Service.Test
{
    public class QuandoForExecutadoGet : UserTest
    {
        private IUserService _service;
        private Mock<IUserService> _mock;

        [Fact]
        public async Task E_Possivel_Executar_MetodoGet()
        {
            _mock = new Mock<IUserService>();
            _mock.Setup(m => m.Get(IdUser)).ReturnsAsync(userDto);
            _service = _mock.Object;

            var result = await _service.Get(IdUser);
            Assert.NotNull(result);
            Assert.True(result.Id == IdUser);
            Assert.Equal(NomeUser,result.Nome);
        }
    }
}