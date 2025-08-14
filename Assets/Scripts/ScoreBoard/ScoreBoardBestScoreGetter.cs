using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.ScoreBoard
{
    public interface IScoreBoardBestScoreGetter
    {
        uint GetBestScore();
    }

    public class ScoreBoardBestScoreGetter : IScoreBoardBestScoreGetter
    {
        private readonly StoredScoreBoard _storedScoreBoard;

        public ScoreBoardBestScoreGetter(StoredScoreBoard storedScoreBoard)
        {
            _storedScoreBoard = storedScoreBoard;
        }

        public uint GetBestScore()
        {
            List<uint> scoreBoardValues = _storedScoreBoard.GetValueOrStoredDefault();

            if (scoreBoardValues.Count == 0)
            {
                return 0;
            }

            return scoreBoardValues.Max();
        }
    }
}
