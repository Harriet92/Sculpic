using System;
using Kalambury.Database.Mongo;
using Kalambury.Mongo.Interfaces;
using Kalambury.WcfServer.Interfaces;
using Kalambury.WcfServer.Repositories;

namespace Kalambury.WcfServer.Services
{
    public class PhraseService : IPhraseService
    {
        private readonly IPhraseRepository phraseRepository;

        public PhraseService()
        {
            IDatabaseServer serverConnection = new MongoDatabaseServer(
                new MongoConnectionSettings()
                {
                    DatabaseName = "TestPhraseDB",
                    Ip = "127.0.0.1",
                    Port = "27017"
                });
            phraseRepository = new PhraseMongoRepository(serverConnection);
        }
        public PhraseService(IDatabaseServer serverConnection)
        {
            phraseRepository = new PhraseMongoRepository(serverConnection);
        }
        public string DrawPhrase()
        {
            var phrase = phraseRepository.GetRandomPhrase();
            return phrase == null ? null : phrase.PhraseText;
        }
    }
}
