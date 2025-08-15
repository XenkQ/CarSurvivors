using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.ScoreBoard
{
    public interface IScoreBoardNewScoreSaver
    {
        void Save(uint score);
    }

    public class ScoreBoardNewScoreSaver : IScoreBoardNewScoreSaver
    {
        private readonly StoredScoreBoard _storedScoreBoard;

        public ScoreBoardNewScoreSaver(StoredScoreBoard storedScoreBoard)
        {
            _storedScoreBoard = storedScoreBoard;
        }

        public void Save(uint score)
        {
            var scoreBoardValues = new SortedSet<uint>(
                _storedScoreBoard.GetValueOrStoredDefault(),
                Comparer<uint>.Create((a, b) => b.CompareTo(a))
            );

            void SaveNewScore()
            {
                scoreBoardValues.Add(score);
                _storedScoreBoard.SaveValue(scoreBoardValues.ToList());
            }

            var lastValue = scoreBoardValues.LastOrDefault();
            if (scoreBoardValues.Count >= _storedScoreBoard.MAX_SAVED_SCORES_COUNT
                && score > lastValue)
            {
                scoreBoardValues.Remove(lastValue);
                SaveNewScore();
            }
            else
            {
                SaveNewScore();
            }
        }
    }
}
