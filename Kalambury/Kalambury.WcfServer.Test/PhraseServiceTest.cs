using FluentAssertions;
using Kalambury.Database.Mongo;
using Kalambury.WcfServer.Interfaces;
using Kalambury.WcfServer.Models;
using Kalambury.WcfServer.Repositories;
using Kalambury.WcfServer.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kalambury.WcfServer.Test
{
    [TestClass]
    public class PhraseServiceTest
    {
        private PhraseService phraseService;
        private IPhraseRepository phraseRepository;
        [TestInitialize]
        public void Initialize()
        {
            var settings = new MongoConnectionSettings { DatabaseName = "SculpicWcfTest", Ip = "localhost", Port = "27017" };
            var mongoServer = new MongoDatabaseServer(settings);

            phraseService = new PhraseService(mongoServer);
            phraseRepository = new PhraseMongoRepository(mongoServer);
        }

        [TestCleanup]
        private void CleanCollections()
        {
            phraseRepository.DeleteAll();
        }
        private void InitializeWithRandomData(int phrasesCount)
        {
            CleanCollections();
            for (int i = 0; i < phrasesCount; i++)
                phraseRepository.Insert(new Phrase()
                {
                    PhraseText = i.ToString()
                });
        }

        [TestMethod]
        [TestCategory("PhraseService DrawPhrase")]
        public void DrawPhrase_BasicTest_ShouldSucceed()
        {
            InitializeWithRandomData(10);
            phraseService.DrawPhrase().Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        [TestCategory("PhraseService DrawPhrase")]
        public void DrawPhrase_OnePhrase_ShouldSucceed()
        {
            InitializeWithRandomData(1);
            phraseService.DrawPhrase().Should().NotBeNullOrEmpty();
        }
        [TestMethod]
        [TestCategory("PhraseService DrawPhrase")]
        public void DrawPhrase_EmptyCollection_ShouldReturnNull()
        {
            CleanCollections();
            phraseService.DrawPhrase().Should().BeNull();
        }
        [TestMethod]
        [TestCategory("PhraseService DrawPhrase")]
        public void DrawPhrase_ManyPhrases_ShouldReturnTwoDifferent()
        {
            InitializeWithRandomData(1000);
            var p1 = phraseService.DrawPhrase();
            var p2 = phraseService.DrawPhrase();
            p1.Should().NotBeNullOrEmpty();
            p2.Should().NotBeNullOrEmpty();
            p1.Should().NotBe(p2);
        }

    }
}
