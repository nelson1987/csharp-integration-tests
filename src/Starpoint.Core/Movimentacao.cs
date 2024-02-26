namespace Starpoint.Core
{
    public class Movimentacao
    {
        public Guid Id { get; set; }
        public required string Descricao { get; set; }
        public decimal Preco { get; set; }
        public Guid Debitante { get; set; }
        public Guid Creditante { get; set; }
    }
}