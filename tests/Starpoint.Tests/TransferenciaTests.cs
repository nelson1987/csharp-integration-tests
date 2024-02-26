using AutoFixture;
using AutoFixture.AutoMoq;
using Starpoint.Core;

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
}