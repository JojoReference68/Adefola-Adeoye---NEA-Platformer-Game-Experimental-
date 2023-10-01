using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adefola_Adeoye___NEA_Platformer_Game
{
    public class HighScoreEntry
    {
        private string username;
        private int score;

        public HighScoreEntry(string Username, int Score)
        {
            username = Username;
            score = Score;
        }
        public string getUsername() { return username; }
        public int getScore() { return score; }
    }
}
