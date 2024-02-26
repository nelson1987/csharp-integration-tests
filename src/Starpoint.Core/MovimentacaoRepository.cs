using MongoDB.Driver;

namespace Starpoint.Core
{
    public interface IMovimentacaoRepository
    {
        public void Insert(Movimentacao movimentacao);
    }

    public class MovimentacaoRepository : IMovimentacaoRepository
    {
        private readonly IMongoCollection<Movimentacao> _ticketsCollection;
        public MovimentacaoRepository()
        {
            var mongoClient = new MongoClient("mongodb://root:example@localhost:27017/");
            var database = mongoClient.GetDatabase("sales");
            _ticketsCollection = database.GetCollection<Movimentacao>(nameof(Movimentacao));
        }
        public void Insert(Movimentacao movimentacao)
        {
            _ticketsCollection.InsertOne(movimentacao);
        }
    }
}