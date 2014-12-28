namespace Assets.Sources.DatabaseClient.Models
{
    public class Phrase
    {
        public string PhraseText { get; set; }
        public int DrawCount { get; set; }
        public int SuccessfullyGuessedCount { get; set; }
        public int DifficultyLevel { get; set; }
    }
}
