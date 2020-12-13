using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Api.Data.Context;
using Api.Data.Implementations;
using Api.Domain.Entities;
using System.Linq;

namespace Api.Data.Test
{
    public class UsuarioCrudCompleto : BaseTest, IClassFixture<DbTest>
    {
        private ServiceProvider _serviceProvider;
        public UsuarioCrudCompleto(DbTest dbTest)
        {
            _serviceProvider = dbTest.ServiceProvider;
        }
        [Fact]
        [Trait("CRUD", "UserEntity")]
        public async Task E_Possivel_CRUP_Usuario()
        {
            using (var context = _serviceProvider.GetService<MyContext>())
            {
                UserImplementations _repositorio = new UserImplementations(context);
                UserEntity _entity = new UserEntity()
                {
                    Email = Faker.Internet.Email(),
                    Nome = Faker.Name.FullName()
                };

                var _registroCriado = await _repositorio.InsertAsync(_entity);
                Assert.NotNull(_registroCriado);
                Assert.Equal(_entity.Email, _registroCriado.Email);
                Assert.Equal(_entity.Nome, _registroCriado.Nome);

                _entity.Nome = Faker.Name.First();
                var _registroAtualizado = await _repositorio.UpdateAsync(_entity);
                Assert.NotNull(_registroAtualizado);
                Assert.Equal(_entity.Email, _registroAtualizado.Email);
                Assert.Equal(_entity.Nome, _registroAtualizado.Nome);

                var _registroExiste = await _repositorio.ExistAsync(_registroAtualizado.Id);
                Assert.True(_registroExiste);

                var _registroSelecionado = await _repositorio.SelectAsync(_registroAtualizado.Id);
                Assert.NotNull(_registroSelecionado);
                Assert.Equal(_registroAtualizado.Email, _registroSelecionado.Email);
                Assert.Equal(_registroAtualizado.Nome, _registroSelecionado.Nome);

                var _todosRegistros = await _repositorio.SelectAsync();
                Assert.NotNull(_todosRegistros);
                Assert.True(_todosRegistros.Count() > 0);

                var _removeu = await _repositorio.DeleteAsync(_registroSelecionado.Id);
                Assert.True(_removeu);

                
            }
        }
    }
}