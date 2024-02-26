namespace Starpoint.Tests
{
    public class InclusaoProdutoHandlerTests
    {
        private readonly InclusaoProdutoCommand _request;
        private readonly InclusaoProdutoHandler _handler;

        public InclusaoProdutoHandlerTests()
        {
            _request = new InclusaoProdutoCommand()
            {
                Id = Guid.NewGuid(),
                Descricao = "Descrição",
                Preco = 1.00M
            }; 
            _handler = new InclusaoProdutoHandler();
        }

        [Fact]
        public void Dado_Request_Valido_Incluir_Produto_Com_Sucesso()
        {
            Assert.True(_handler.Handle(_request));
        }

        [Fact]
        public void Dado_Request_Invalido_Incluir_Produto_Com_Falha_Preco_Igual_Menor_Zero()
        {
            InclusaoProdutoCommand request = _request with { Preco = 0.00M };
            Assert.False(_handler.Handle(request));
        }

        [Fact]
        public void Dado_Request_Invalido_Incluir_Produto_Com_Falha_Descricao_Vazio()
        {
            InclusaoProdutoCommand request = _request with { Descricao = "" };
            Assert.False(_handler.Handle(request));
        }

        [Fact]
        public void Dado_Request_Invalido_Incluir_Produto_Com_Falha_Id_Vazio()
        {
            InclusaoProdutoCommand request = _request with { Id = Guid.Empty };
            Assert.False(_handler.Handle(request));
        }
    }

    public record InclusaoProdutoCommand
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }

        public bool IsValid()
        {
            return Id != Guid.Empty && !string.IsNullOrEmpty(Descricao) && Preco > 0;
        }
    }

    public class InclusaoProdutoHandler
    {
        public bool Handle(InclusaoProdutoCommand command)
        {
            return command.IsValid();
        }
    }
}