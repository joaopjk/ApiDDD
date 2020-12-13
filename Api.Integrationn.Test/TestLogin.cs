using System.Threading.Tasks;
using Xunit;

namespace Api.Integrationn.Test
{
    public class TestLogin : BaseIntegration
    {
        [Fact]
        public async Task testeLogin()
        {
            await AdicionarToken();
        }
    }
}