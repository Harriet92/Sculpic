using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Kalambury.Database.Mongo;
using Kalambury.Mongo.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace Kalambury.Mongo.Mongo
{
    public class MongoRepository<T> : IRepository<T>
    {
        public MongoDatabaseServer MongoServer { get; set; }
        private string collectionName;
        private readonly MongoInsertOptions mongoInsertOptions;

        private MongoCollection<T> collection;
        public MongoCollection<T> Collection
        {
            get
            {
                if (collection == null || !MongoServer.IsConnected)
                    Connect();
                return collection;
            }
            set { collection = value; }
        }

        public MongoRepository(MongoDatabaseServer databaseServer, string collection = null)
        {
            MongoServer = databaseServer;
            collectionName = collection;
            mongoInsertOptions = new MongoInsertOptions { WriteConcern = WriteConcern.Acknowledged };
        }

        private void Connect()
        {
            if (MongoServer == null) return;
            MongoServer.Connect();
            if (string.IsNullOrEmpty(collectionName))
                collectionName = typeof(T).Name + "s";
            collection = MongoServer.GetMongoDatabase().GetCollection<T>(collectionName);
        }

        public T Insert(T item)
        {
            var result = Collection.Insert<T>(item, mongoInsertOptions);
            return result.Ok ? item : default(T);
        }

        public T Save(T item)
        {
            var result = Collection.Save(item, mongoInsertOptions);
            return result.Ok ? item : default(T);
        }

        public bool Update(IMongoQuery query, IMongoUpdate update)
        {
            return Collection.Update(query, update).Ok;
        }

        public T GetItemByQuery(Expression<Func<T, bool>> predicate)
        {
            IMongoQuery query = Query<T>.Where(predicate);
            return Collection.FindOneAs<T>(query);
        }

        public bool Delete(T item)
        {
            return Collection.Remove(Query.EQ("_id", item.ToBsonDocument()["_id"])).Ok;
        }

        public bool DeleteByQuery(Expression<Func<T, bool>> predicate)
        {
            IMongoQuery query = Query<T>.Where(predicate);
            return Collection.Remove(query).Ok;
        }

        public bool DeleteAll()
        {
            return Collection.Drop().Ok;
        }

        public List<T> GetByQuery(Expression<Func<T, bool>> predicate)
        {
            return Collection.AsQueryable().Where(predicate.Compile()).ToList();
        }

        public List<T> GetAll()
        {
            return Collection.FindAllAs<T>().ToList<T>();
        }

        public T AtomicIncrement(Expression<Func<T, bool>> predicate, Expression<Func<T, long>> updatedProperty, long value)
        {
            FindAndModifyArgs args = new FindAndModifyArgs()
            {
                Query = Query<T>.Where(predicate),
                Update = Update<T>.Inc(updatedProperty, value),
                Upsert = true
            };
            return Collection.FindAndModify(args).GetModifiedDocumentAs<T>();
        }

        public T AtomicUpdate<TMember>(Expression<Func<T, bool>> predicate, Expression<Func<T, TMember>> updatedProperty, TMember newValue)
        {
            FindAndModifyArgs args = new FindAndModifyArgs()
            {
                Query = Query<T>.Where(predicate),
                Update = Update<T>.Set(updatedProperty, newValue)
            };
            return Collection.FindAndModify(args).GetModifiedDocumentAs<T>();
        }

        public T AtomicAddToList<TMember>(Expression<Func<T, bool>> predicate, Expression<Func<T, IEnumerable<TMember>>> updatedProperty, TMember newValue)
        {
            FindAndModifyArgs args = new FindAndModifyArgs()
            {
                Query = Query<T>.Where(predicate),
                Update = Update<T>.Push(updatedProperty, newValue),
                Upsert = true
            };
            return Collection.FindAndModify(args).GetModifiedDocumentAs<T>();
        }

        public bool AtomicRemoveFromList<TMember>(Expression<Func<T, bool>> expression, Expression<Func<T, IEnumerable<TMember>>> predicate, TMember value)
        {
            FindAndModifyArgs args = new FindAndModifyArgs()
            {
                Query = Query<T>.Where(expression),
                Update = Update<T>.Pull(predicate, value),
                Upsert = true
            };
            return Collection.FindAndModify(args).Ok;
        }

        public long CountAll()
        {
            return Collection.Count();
        }
    }
}
