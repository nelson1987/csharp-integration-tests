namespace Starpoint.Core
{
    public class ContaBancaria
    {
        public Guid Id { get; set; }
        public required string Descricao { get; set; }
        public decimal Saldo { get; set; }
    }
}