using MongoDB.Driver;

namespace Starpoint.Core
{
    public interface IProdutoRepository
    {
        public ContaBancaria? GetById(Guid id);
        public void Insert(ContaBancaria conta);
    }

    public class ProdutoRepository : IProdutoRepository
    {
        private readonly IMongoCollection<ContaBancaria> _ticketsCollection;
        public ProdutoRepository()
        {
            var mongoClient = new MongoClient("mongodb://root:example@localhost:27017/");
            var database = mongoClient.GetDatabase("sales");
            _ticketsCollection = database.GetCollection<ContaBancaria>(nameof(ContaBancaria));
        }

        public ContaBancaria? GetById(Guid id)
        {
            var collection = _ticketsCollection.Find(x => x.Id == id);
            return collection.FirstOrDefault();
        }

        public void Insert(ContaBancaria conta)
        {
            _ticketsCollection.InsertOne(conta);
        }
    }
}