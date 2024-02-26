using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using Starpoint.Core;

namespace Starpoint.Tests
{
    public class InclusaoTransferenciaHandlerUnitTests
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());
        private readonly Guid _creditante = Guid.NewGuid();
        private readonly Guid _debitante = Guid.NewGuid();
        private readonly InclusaoTransferenciaCommand _request;
        private readonly InclusaoTransferenciaHandler _handler;
        private readonly Mock<IProdutoRepository> _repository;

        public InclusaoTransferenciaHandlerUnitTests()
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