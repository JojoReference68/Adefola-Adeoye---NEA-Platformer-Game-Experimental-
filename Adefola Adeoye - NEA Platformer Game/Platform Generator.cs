using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adefola_Adeoye___NEA_Platformer_Game
{
    public class Platform_Generator
    {
        private List<Platform> platforms;
        private Random random;

        public Platform_Generator()
        {
            platforms = new List<Platform>();
            random = new Random();
        }

        public void CreatePlatform(int X, int Y, int WIDTH, int HEIGHT)
        {
            Platform platform = new Platform(HEIGHT, WIDTH, X, Y);
            platforms.Add(platform);
        }

        public void AddPlatformsToMap(char[,] gameMap)
        {
            foreach (var platform in platforms)
            {
                // Add the platform to the game map
                for (int i = platform.getX(); i < platform.getX() + platform.getWidth(); i++)
                {
                    for (int j = platform.getY(); j < platform.getY()+ platform.getHeight(); j++)
                    {
                        gameMap[i, j] = platform.getCharacter();
                    }
                }
            }
        }

        private bool IsOverlap(Platform newPlatform, List<Platform> existingPlatforms, char[,] gameMap) 
        {

            foreach (var platform in existingPlatforms)
            {
                if (newPlatform.getX() < platform.getX() + platform.getWidth() &&
                newPlatform.getX() + newPlatform.getWidth() > platform.getX() &&
                newPlatform.getY() < platform.getY() + platform.getHeight() &&
                newPlatform.getY() + newPlatform.getHeight() > platform.getY())
                {
                    return true; // Overlapping
                }
            }

            // Check for overlap with terrain
            for (int i = newPlatform.getX(); i < newPlatform.getX() + newPlatform.getWidth(); i++)
            {
                for (int j = newPlatform.getY(); j < newPlatform.getY() + newPlatform.getHeight(); j++)
                {
                    if (gameMap[i, j] != ' ')
                    {
                        return true; // Overlapping with terrain
                    }
                }
            }

            return false; // No overlap
        }

        public void CreateRandomPlatform(int width, int height, char[,] gameMap)
        {
            int maxX = gameMap.GetLength(0) - width;
            int maxY = gameMap.GetLength(1) - height;

            Platform newPlatform;

            while (true)
            {
                int randomX = random.Next(maxX);
                int randomY = random.Next(maxY);
                newPlatform = new Platform(height, width, randomX, randomY);

                if (!IsOverlap(newPlatform, platforms, gameMap))
                {
                    platforms.Add(newPlatform);
                    break;
                }
            }
        }
    }
}
