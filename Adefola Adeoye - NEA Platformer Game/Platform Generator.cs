using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adefola_Adeoye___NEA_Platformer_Game
{
    public class Platform_Generator
    {
        private Platform[] platforms;
        private int width;
        private int height;
        private char platformchar;
        private int maxPlatforms;


        public Platform_Generator(int WIDTH, int HEIGHT, char PLATFORMCHAR)
        {
            width = WIDTH;
            height = HEIGHT;
            platformchar = '▄';
            ConsoleColor platformColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Green;
            platforms = new Platform[WIDTH];

        }

    }
}
