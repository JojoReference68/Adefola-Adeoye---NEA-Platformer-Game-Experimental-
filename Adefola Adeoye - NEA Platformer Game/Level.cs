using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adefola_Adeoye___NEA_Platformer_Game
{
    public class Level
    {
        private int width;
        private int gameMapHeight;
        private Terrain_Generator terrainGenerator;
        private char[,] gameMap;
        private int heightmulitplier;
        private char terrainChar;
        private Player player;
        private int terminalVelocity;

        public Level(int Width, int GameMapHeight, int HeightMultiplier, char TerrainChar, Player Player)
        {
            width = Width;
            gameMapHeight = GameMapHeight;
            heightmulitplier = HeightMultiplier;
            terrainChar = TerrainChar;
            player = Player;

            // Initialize terrain generator for this level
            terrainGenerator = new Terrain_Generator(width, 0.5f, 5); // You can adjust persistence and octaves as needed

            // Create the game map and generate terrain
            gameMap = new char[width, gameMapHeight];
            GenerateGameWorld();
        }

        // Method to populate the game world map with terrain
        private void GenerateGameWorld()
        {
            float[] terrain = terrainGenerator.GeneratePerlinNoise();

            for (int x = 0; x < width; x++)
            {
                int terrainHeight = (int)(terrain[x] * heightmulitplier);
                for (int y = 0; y < gameMapHeight; y++)
                {
                    if (y >= gameMapHeight - Math.Max(terrainHeight, 1))
                    {
                        gameMap[x, y] = terrainChar;
                    }
                    else
                    {
                        gameMap[x, y] = ' ';
                    }
                }
            }
        }
    }
}
