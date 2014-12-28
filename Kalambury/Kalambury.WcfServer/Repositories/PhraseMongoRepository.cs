using System;
using System.Linq;
using Kalambury.Database.Mongo;
using Kalambury.Mongo.Interfaces;
using Kalambury.Mongo.Mongo;
using Kalambury.WcfServer.Helpers;
using Kalambury.WcfServer.Interfaces;
using Kalambury.WcfServer.Models;
using MongoDB.Driver.Builders;

namespace Kalambury.WcfServer.Repositories
{
    public class PhraseMongoRepository : MongoRepository<Phrase>, IPhraseRepository
    {
        public PhraseMongoRepository(IDatabaseServer connectionSettings)
            : base((MongoDatabaseServer)connectionSettings, "Phrases")
        {
            Collection.CreateIndex(IndexKeys<Phrase>.Ascending(_ => _.RandomNumber));
        }

        public Phrase GetRandomPhrase()
        {
            return
                Collection.Find(Query.GTE("RandomNumber", RandomNumber.GetDouble()))
                    .SetSortOrder(SortBy.Ascending("RandomNumber"))
                    .FirstOrDefault() ??
                    Collection.Find(Query.LTE("RandomNumber", RandomNumber.GetDouble()))
                    .FirstOrDefault();
        }
    }
}
