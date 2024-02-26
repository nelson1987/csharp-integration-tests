namespace Starpoint.Core
{
    public record InclusaoTransferenciaCommand
    {
        public Guid Id { get; set; }
        public Guid Creditante { get; set; }
        public Guid Debitante { get; set; }
        public required string Descricao { get; set; }
        public decimal Preco { get; set; }

        public bool IsValid()
        {
            return Id != Guid.Empty &&
                !string.IsNullOrEmpty(Descricao) &&
                Preco > 0 &&
                Creditante != Guid.Empty &&
                Debitante != Guid.Empty;
        }
    }
}