using System;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Kalambury.WcfServer.Models
{
    [BsonIgnoreExtraElements]
    [DataContract]
    public class Phrase
    {
        public ObjectId Id { get; set; }
        [DataMember]
        public string PhraseText { get; set; }
        [DataMember]
        public int DrawCount { get; set; }
        [DataMember]
        public int SuccessfullyGuessedCount { get; set; }
        [DataMember]
        public int DifficultyLevel { get; set; }
        public double RandomNumber { get; set; }//TODO: Index should be set on this field!
        
        public Phrase()
        {
            RandomNumber = Helpers.RandomNumber.GetDouble();
        }
    }
}
