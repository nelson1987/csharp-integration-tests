using FluentResults;

namespace Starpoint.Core
{
    public interface IInclusaoTransferenciaHandler
    {
        Result Handle(InclusaoTransferenciaCommand command);
    }

    public class InclusaoTransferenciaHandler : IInclusaoTransferenciaHandler
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMovimentacaoRepository _movimentacaoRepository;

        public InclusaoTransferenciaHandler(IProdutoRepository repository,
            IMovimentacaoRepository movimentacaoRepository)
        {
            _produtoRepository = repository;
            _movimentacaoRepository = movimentacaoRepository;
        }

        public Result Handle(InclusaoTransferenciaCommand command)
        {
            try
            {
                if (!command.IsValid()) return Result.Fail(ErrorDictionary.GetErrorByKey(1));

                var debitante = _produtoRepository.GetById(command.Debitante);
                if (debitante == null) return Result.Fail(ErrorDictionary.GetErrorByKey(2));

                var creditante = _produtoRepository.GetById(command.Creditante);
                if (creditante == null) return Result.Fail(ErrorDictionary.GetErrorByKey(3));

                if (debitante.Saldo < command.Preco) return Result.Fail(ErrorDictionary.GetErrorByKey(4));

                debitante.Saldo -= command.Preco;
                creditante.Saldo += command.Preco;

                _movimentacaoRepository.Insert(new Movimentacao()
                {
                    Id = command.Id,
                    Descricao = "Descricao",
                    Creditante = creditante.Id,
                    Debitante = debitante.Id
                });
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail(new Error("Inclusao de Transferencia not executed successfully").CausedBy(ex));
            }
        }
    }
}