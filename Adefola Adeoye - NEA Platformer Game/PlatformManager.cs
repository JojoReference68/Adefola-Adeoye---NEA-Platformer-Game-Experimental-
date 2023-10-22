using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adefola_Adeoye___NEA_Platformer_Game
{
    public class PlatformManager
    {
        private char[,] gameMap;
        private int mapWidth;
        private int mapHeight;
        private Random random;

        public PlatformManager(char[,] map, int width, int height)
        {
            gameMap = map;
            mapWidth = width;
            mapHeight = height;
            random = new Random();
        }

        public void GeneratePlatformsUsingPerlin(int numberOfPlatforms, int minWidth, int maxWidth, int minHeight, int maxHeight, int minYPos, int maxYPos, int minDistance)
        {
            int generatedPlatforms = 0;
            int previousPlatformX = -1;

            while (generatedPlatforms < numberOfPlatforms)
            {
                int platformWidth = random.Next(minWidth, maxWidth);
                int platformHeight = random.Next(minHeight, maxHeight);

                int maxAttempts = 100; // Adjust as needed
                for (int attempt = 0; attempt < maxAttempts; attempt++)
                {
                    int posX = random.Next(0, mapWidth - platformWidth);
                    int posY = random.Next(minYPos, maxYPos);

                    if (!CheckOverlap(posX, posY, platformWidth, platformHeight) &&
                        !CheckTerrainOverlap(posX, posY, platformWidth, platformHeight) &&
                        (previousPlatformX == -1 || Math.Abs(previousPlatformX - posX) >= minDistance))
                    {
                        // Both platform, terrain, and minimum distance checks pass, so create the platform
                        CreatePlatform(posX, posY, platformWidth, platformHeight);
                        previousPlatformX = posX + platformWidth - 1; // Update the previous platform's X position
                        generatedPlatforms++;
                        break;
                    }
                }
            }
        }

        private int FindValidPlatformPosition(int platformWidth)
        {
            int maxAttempts = 100; // Adjust as needed
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                int posX = random.Next(0, mapWidth - platformWidth);
                int posY = random.Next(0, mapHeight);

                if (!CheckOverlap(posX, posY, platformWidth, 1))
                {
                    return posY;
                }
            }

            // If a suitable position is not found, return a fallback position.
            return random.Next(0, mapHeight);
        }

        private bool CheckTerrainOverlap(int posX, int posY, int width, int height)
        {
            for (int x = posX; x < posX + width; x++)
            {
                for (int y = posY; y < posY + height; y++)
                {
                    if (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
                    {
                        if (gameMap[x, y] == '█')
                        {
                            return true; // There is an overlap with terrain
                        }
                    }
                }
            }
            return false; // No overlap with terrain
        }

        private bool CheckOverlap(int x, int y, int width, int height)
        {
            for (int i = x; i < x + width; i++)
            {
                for (int j = y; j < y + height; j++)
                {
                    if (i >= 0 && i < mapWidth && j >= 0 && j < mapHeight)
                    {
                        if (gameMap[i, j] == '=' || gameMap[i, j] == '*' || gameMap[i, j] == '█')
                        {
                            return true; // Overlaps with terrain or existing platforms
                        }
                    }
                }
            }
            return false;
        }

        private void CreatePlatform(int x, int y, int width, int height)
        {
            for (int i = x; i < x + width; i++)
            {
                for (int j = y; j < y + height; j++)
                {
                    if (i >= 0 && i < mapWidth && j >= 0 && j < mapHeight)
                    {
                        gameMap[i, j] = '=';
                    }
                }
            }
        }
    }
}
