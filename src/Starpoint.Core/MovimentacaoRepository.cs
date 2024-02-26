using MongoDB.Driver;

namespace Starpoint.Core
{
    public interface IMovimentacaoRepository
    {
        public void Insert(Movimentacao movimentacao);
        public Movimentacao? GetById(Guid id);
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

        public Movimentacao? GetById(Guid id)
        {
            var collection = _ticketsCollection.Find(x => x.Id == id);
            return collection.FirstOrDefault();
        }

        public void Insert(Movimentacao movimentacao)
        {
            _ticketsCollection.InsertOne(movimentacao);
        }
    }
}