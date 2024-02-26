using FluentResults;
namespace Starpoint.Core
{
    public static class ErrorDictionary
    {
        private static Dictionary<int, string> Dicionario => new Dictionary<int, string>()
        {
            { 1, "Comando é inválido." },
            { 2, "Debitante não encontrado." },
            { 3, "Creditante não encontrado." },
            { 4, "Saldo insuficiente." }
        };
        public static Error GetErrorByKey(int key)
        {
            var par = Dicionario.FirstOrDefault(x => x.Key == 1);
            return new Error(par.Value)
                    .WithMetadata("Error Code", par.Key);
        }
    }
}
