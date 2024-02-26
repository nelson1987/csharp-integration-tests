using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Starpoint.Core;
using System.Text;

namespace Starpoint.Tests
{
    public class ProdutoControllerIntegrationTests :  IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMovimentacaoRepository _movimentacaoRepository;
        public ProdutoControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _produtoRepository = _factory.Server.Services.GetRequiredService<IProdutoRepository>();
            _movimentacaoRepository = _factory.Server.Services.GetRequiredService<IMovimentacaoRepository>();
        }

        [Fact]
        public async Task Post_EndpointsReturnSuccessAndCorrectContentType()
        {
            var transferenciaId = Guid.NewGuid();
            var debitante = new ContaBancaria() { Id = Guid.NewGuid(), Saldo = 100.00M, Descricao = "Descricao" };
            var creditante = new ContaBancaria() { Id = Guid.NewGuid(), Saldo = 0.00M, Descricao = "Descricao" };
            _produtoRepository.Insert(debitante);
            _produtoRepository.Insert(creditante);

            var jsonContent = JsonConvert.SerializeObject(new InclusaoTransferenciaCommand()
            {
                Id = transferenciaId,
                Descricao = "Descrição",
                Preco = 10.00M,
                Debitante = debitante.Id,
                Creditante = creditante.Id
            });
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var response = await client.PostAsync("/produto", new StringContent(jsonContent, Encoding.UTF8, "application/json"));
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            var conta = _movimentacaoRepository.GetById(transferenciaId);
            Assert.NotNull(conta);
        }
    }
}