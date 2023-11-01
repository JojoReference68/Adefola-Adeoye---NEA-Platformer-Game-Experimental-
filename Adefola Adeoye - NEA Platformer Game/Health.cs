using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adefola_Adeoye___NEA_Platformer_Game
{
    internal class Health
    {
        int maxHealth;
        int health;
        char healthChar;
        public Health(int MAXHEALTH)
        {
            maxHealth = MAXHEALTH;
            healthChar = '★';
        }

        public void SetHealth(int Health)
        {
            health = Health;
        }

        public int GetHealth() { return health; }

        public void Display()
        {
            for (int i = 0; i < maxHealth; i++)
            {
                Console.Title += healthChar;
            }
        }
    }
}
