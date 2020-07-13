using System;

namespace Cubra.Heplers
{
    [Serializable]
    public class LeaderboardHelper
    {
        // Рейтинг игрока
        public int Rating;

        // Результаты лучших
        public string[] Names;
        public long[] Results;
    }
}