using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Starpoint.Core;
using System.Text;

namespace Starpoint.Tests
{
    public class TransferenciaTests
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());
        [Fact]
        public void Dado_Request_Valido_Incluir_Produto_Com_Sucesso()
        {
            Guid contaBancaria = Guid.NewGuid();
            var debitados = _fixture
                .Build<Transferencia>()
                .With(x => x.Debitante, contaBancaria)
                .CreateMany(5);
            var creditados = _fixture
                .Build<Transferencia>()
                .With(x => x.Creditante, contaBancaria)
                .CreateMany(5);
            List<Transferencia> transferencias = debitados.ToList();
            transferencias.AddRange(creditados);

            var soma = transferencias.Sum(x => x.Debitante == contaBancaria
            ? x.Preco * -1
            : x.Preco);
            Assert.Equal(soma, 1000);
        }
    }
    public class InclusaoTransferenciaIntegrationTests :  IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMovimentacaoRepository _movimentacaoRepository;
        public InclusaoTransferenciaIntegrationTests(WebApplicationFactory<Program> factory)
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

            var conta = _produtoRepository.GetById(transferenciaId);
            Assert.NotNull(conta);
        }
    }
    public class InclusaoTransferenciaHandlerTests
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());
        private readonly Guid _creditante = Guid.NewGuid();
        private readonly Guid _debitante = Guid.NewGuid();
        private readonly InclusaoTransferenciaCommand _request;
        private readonly InclusaoTransferenciaHandler _handler;
        private readonly Mock<IProdutoRepository> _repository;

        public InclusaoTransferenciaHandlerTests()
        {
            ContaBancaria creditante = _fixture
                .Build<ContaBancaria>()
                .With(x => x.Id, _creditante)
                .Create();
            ContaBancaria debitante = _fixture
                .Build<ContaBancaria>()
                .With(x => x.Id, _debitante)
                .With(x => x.Saldo, 100000M)
                .Create();

            _request = new InclusaoTransferenciaCommand()
            {
                Id = Guid.NewGuid(),
                Descricao = "Descrição",
                Preco = 10.00M,
                Debitante = debitante.Id,
                Creditante = creditante.Id
            };
            _repository = _fixture.Freeze<Mock<IProdutoRepository>>();
            _repository
                 .Setup(x => x.GetById(_creditante))
                 .Returns(creditante);
            _repository
                 .Setup(x => x.GetById(_debitante))
                 .Returns(debitante);
            _handler = _fixture.Build<InclusaoTransferenciaHandler>()
                .Create();
        }

        [Fact]
        public void Dado_Request_Valido_Incluir_Produto_Com_Sucesso()
        {
            var result = _handler.Handle(_request);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void Dado_Request_Invalido_Incluir_Produto_Com_Falha_Preco_Igual_Menor_Zero()
        {
            InclusaoTransferenciaCommand request = _request with { Preco = 0.00M };
            var result = _handler.Handle(request);
            Assert.Equal("Comando é inválido.", result.Reasons[0].Message);
        }

        [Fact]
        public void Dado_Request_Invalido_Incluir_Produto_Com_Falha_Descricao_Vazio()
        {
            InclusaoTransferenciaCommand request = _request with { Descricao = "" };
            var result = _handler.Handle(request);
            Assert.Equal("Comando é inválido.", result.Reasons[0].Message);
        }

        [Fact]
        public void Dado_Request_Invalido_Incluir_Produto_Com_Falha_Id_Vazio()
        {
            InclusaoTransferenciaCommand request = _request with { Id = Guid.Empty };
            var result = _handler.Handle(request);
            Assert.Equal("Comando é inválido.", result.Reasons[0].Message);
        }

        [Fact]
        public void Dado_Request_Invalido_Incluir_Produto_Com_Falha_Creditante_Vazio()
        {
            InclusaoTransferenciaCommand request = _request with { Creditante = Guid.Empty };
            var result = _handler.Handle(request);
            Assert.Equal("Comando é inválido.", result.Reasons[0].Message);
        }

        [Fact]
        public void Dado_Request_Invalido_Incluir_Produto_Com_Falha_Debitante_Vazio()
        {
            InclusaoTransferenciaCommand request = _request with { Debitante = Guid.Empty };
            var result = _handler.Handle(request);
            Assert.Equal("Comando é inválido.", result.Reasons[0].Message);
        }
    }
}