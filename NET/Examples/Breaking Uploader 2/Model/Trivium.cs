namespace Uploader.Model
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider 
    public class Trivium
    {
        public string Event { get; set; }
        public Phase[] Phases { get; set; }
        public Ranking[] FinalRankings { get; set; }
    }

    public class Phase
    {
        public string PhaseName { get; set; }
        public Battle[] Battles { get; set; }
        public Ranking[] Rankings { get; set; }

        public Ranking[][] RankingForEachGroup { get; set; }
    }

    public class Battle
    {
        public AthletePreseed SoloPlayer { get; set; }
        public AthleteTrivium RedPlayer { get; set; }
        public AthleteTrivium BluePlayer { get; set; }

        public int LayerIndex { get; set; }
        public int BatchIndex { get; set; }
        public int EntranceIndex { get; set; }
    }

    public class AthletePreseed:Athlete
    {
        public ScoredetailPreseed[] ScoreDetails { get; set; }
    }

    public class AthleteTrivium:Athlete
    {
        public ScoredetailTrivium[] ScoreDetails { get; set; }
    }

    public class Athlete
    {
        public string Name { get; set; }
        public string Min { get; set; }
        public string National { get; set; }
        public decimal FinalScore { get; set; }
    }


    public class ScoredetailPreseed
    {
        public Judge Judge { get; set; }
        public decimal Score { get; set; }
    }

    public class Judge
    {
        public string Name { get; set; }
        public string Min { get; set; }
        public string National { get; set; }
    }

    public class ScoredetailTrivium
    {
        public int RoundIndex { get; set; }
        public Rating[] Ratings { get; set; }
    }

    public class Rating
    {
        public Judge Judge { get; set; }
        public Score[] Scores { get; set; }
    }


    public class Score
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
    }


    public class Ranking
    {
        public int RankIndex { get; set; }
        public string Name { get; set; }
        public string Min { get; set; }
        public decimal Score { get; set; }
        public string National { get; set; }
        public bool IsQuit { get; set; }

    }

}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider 