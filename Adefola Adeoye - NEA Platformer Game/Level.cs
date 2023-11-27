using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Adefola_Adeoye___NEA_Platformer_Game
{
    public class Level
    {
        private int width;
        private int gameMapHeight;
        private Terrain_Generator terrainGenerator;
        private char[,] gameMap;
        private float[] terrain;
        private int heightmultiplier;
        private char terrainChar;
        private float persistence;
        private int octaves;
        private Player player;
        private int terminalVelocity;
        private int minTerrainHeight; // Minimum height for the terrain
        private bool isMusicPlaying;
        private PlatformManager platformManager;
        private char platformChar;
        private int score;
        Stopwatch stopwatch = new Stopwatch();
        int maxScore;
        private Dictionary<char, ConsoleColor> colorDictionary;
        private bool playerAlive;
        private List<Enemy> enemies;



        public Level(int Width, int GameMapHeight, int HeightMultiplier, char TerrainChar, float Persistence, int Octaves)
        {
            enemies = new List<Enemy>();
            width = Width;
            gameMapHeight = GameMapHeight;
            heightmultiplier = HeightMultiplier;
            terrainChar = TerrainChar;
            platformChar = '=';
            persistence = Persistence;
            octaves = Octaves;
            minTerrainHeight = 1;
            terminalVelocity = 7;
            maxScore = 1000000;
            Console.SetWindowSize(width, gameMapHeight);

            colorDictionary = new Dictionary<char, ConsoleColor>();
            // Initialize the player
            player = new Player("PlayerName", 0, 0, 0, 0);

            // Initialize the terrain generator and generate the game world
            terrainGenerator = new Terrain_Generator(width, persistence, octaves);
            terrain = terrainGenerator.GeneratePerlinNoise();
            // Initialize the game map based on terrain
            gameMap = new char[width, gameMapHeight];
            InitializeColors();
            platformManager = new PlatformManager(gameMap, width, gameMapHeight);
        }

        private void ClearMap() //clears up the gameMap
        {
            for (int x = 0; x < width; x++)
            {
                int terrainHeight = (int)(terrain[x] * heightmultiplier);
                for (int y = 0; y < gameMapHeight; y++)
                {
                    gameMap[x, y] = ' ';
                }
            }

            enemies.Clear();
        }
        private void InitializeColors()
        {
            colorDictionary.Add('*', ConsoleColor.White);
            colorDictionary.Add('=', ConsoleColor.Gray);
            colorDictionary.Add('█', ConsoleColor.Green);
            colorDictionary.Add('▄', ConsoleColor.DarkRed);
            colorDictionary.Add('+', ConsoleColor.Magenta);
        }

        public void GeneratePlatformsUsingPerlinNoise(int numberOfPlatforms, int minWidth, int maxWidth, int minHeight, int maxHeight, int minYPos, int maxYPos, int minDistance)
        {
            platformManager.GeneratePlatformsUsingPerlin(numberOfPlatforms, minWidth, maxWidth, minHeight, maxHeight, minYPos, maxYPos, minDistance);
        }

        private void GenerateGameWorld()
        {
            for (int x = 0; x < width; x++)
            {
                int terrainHeight = (int)(terrain[x] * heightmultiplier); // Adjust the multiplier for terrain height
                for (int y = 0; y < gameMapHeight; y++)
                {
                    if (y >= gameMapHeight - Math.Max(terrainHeight, minTerrainHeight))
                    {
                        gameMap[x, y] = '█'; // Set the map cell to represent terrain
                    }
                    else
                    {
                        gameMap[x, y] = ' '; // Set empty space for non-terrain cells
                    }
                }
            }
        }

        public void HandleGravity() //Simulate gravity using suvat equations
        {
            double deltaTime = 0.1;
            double velocity = player.GetInitialVelocity() + (player.GetAcceleration() * deltaTime);

            if (velocity >= terminalVelocity) //sets a speed limit to the player
            {
                velocity = terminalVelocity;
            }
            int newYPos = player.GetPosY() + (int)Math.Round(velocity * deltaTime);
            if (newYPos < 0)
            {
                newYPos = 0;
                velocity = 0; // Reset velocity when hitting the top of the screen
            }
            if (player.GetPosY() >= gameMapHeight)
            {
                playerAlive = false;
            }

            player.SetPosY(newYPos);
            player.SetInitialVelocity(velocity);
        }

        public void HandleInput(ConsoleKeyInfo input)   //Handles the player controls and the right response
        {
            if (input.Key == ConsoleKey.LeftArrow && player.GetPosX() > 0) //Moves player left
            {
                if (player.GetPosX() > 0 && CheckCollisionLeft() == false)
                {
                    player.MoveLeft(gameMap);
                }

            }
            else if (input.Key == ConsoleKey.RightArrow && player.GetPosX() < width - 1) //Moves player right
            {
                if (player.GetPosX() < width - 1 && CheckCollisionRight() == false)
                {
                    player.MoveRight(gameMap);
                }

            }
            else if (input.Key == ConsoleKey.UpArrow && CheckTouchingMap() == true && player.GetPosY() <= gameMapHeight - 1) //Lets player jump
            {
                player.MoveUp();
            }
        }
        private bool CheckMapCollisionAbove()
        {
            if (player.GetPosX() >= 0 && player.GetPosX() < width && player.GetPosY() > 0 && player.GetPosY() < gameMapHeight - 1)
            {
                return (gameMap[player.GetPosX(), player.GetPosY() - 1] == terrainChar) || (gameMap[player.GetPosX(), player.GetPosY() - 1] == platformChar);
            }
            else
            {
                return false;
            }

        }
        protected bool CheckCollisionLeft()  //Checks if the player and terrain to the right are touching
        {
            return (gameMap[player.GetPosX() - 1, player.GetPosY()] == terrainChar) || (gameMap[player.GetPosX() - 1, player.GetPosY()] == platformChar);
        }
        protected bool CheckCollisionRight() //Checks if the player and terrain to the left are touching
        {
            return (gameMap[player.GetPosX() + 1, player.GetPosY()] == terrainChar) || (gameMap[player.GetPosX() + 1, player.GetPosY()] == platformChar); ;
        }

        public bool CheckTouchingTerrain() //Checks if the player and terrain below are touching
        {
            // Check if player's position is within map bounds
            if (player.GetPosX() >= 0 && player.GetPosX() < width && player.GetPosY() >= 0 && player.GetPosY() < gameMapHeight - 1)
            {
                return gameMap[player.GetPosX(), player.GetPosY() + 1] == terrainChar;
            }
            return false;
        }

        public bool CheckTouchingMap()
        {
            if (CheckTouchingPlatform() == true || CheckTouchingTerrain() == true)
            {
                return true;
            }
            return false;
        }

        public void DisplayGameWorld()
        {
            int screenHeight = Console.WindowHeight;

            // Calculate the starting Y position for rendering the terrain
            int startY = Math.Max(0, screenHeight - gameMapHeight);

            for (int y = startY; y < screenHeight; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int adjustedY = y - startY; // Adjust the Y coordinate for rendering
                    char mapChar = (adjustedY < gameMapHeight) ? gameMap[x, adjustedY] : ' '; // Use space if outside gameMap

                    Console.SetCursorPosition(x, y);
                    if (mapChar != ' ')
                    {
                        Console.ForegroundColor = colorDictionary[mapChar];
                        Console.Write(mapChar);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write(mapChar);
                    }

                }
            }
        }



        public bool BeginGame()
        {
            playerAlive = true;
            bool quitGame = false;
            UpdateScore();
            stopwatch.Start();

            
            //seperate thread for music playback(yet to be implemented)

            //Main game logic.
            while (quitGame == false && playerAlive == true)
            {
                Console.CursorVisible = false;
                if (Console.KeyAvailable == true)
                {
                    ConsoleKeyInfo input = Console.ReadKey(true);
                    HandleInput(input);
                }
                player.Delete(gameMap);
                HandleGravity();
                UpdateEnemies();
                if (CheckMapCollisionAbove() == true)
                {
                    player.SetInitialVelocity(0.0);
                    player.SetPosY(player.GetPosY() + 1);
                }
                if (CheckTouchingTerrain() == true || CheckTouchingPlatform() == true) //ensures player does not fall through.
                {
                    player.SetInitialVelocity(0.0);
                    if (ReachedEndofLevel() == true) { quitGame = true; } //ends the level once the player has reached the end of the gamemap.
                }
                player.Show(gameMap);
                System.Threading.Thread.Sleep(50);
            }
            stopwatch.Stop();
            TimeSpan elapsedTime = stopwatch.Elapsed;
            score += (int)(maxScore / elapsedTime.TotalSeconds);
            UpdateScore();
            return playerAlive;
        }

        private bool CheckTouchingPlatform()
        {
            if (true)
            {
                if (player.GetPosX() >= 0 && player.GetPosX() < width && player.GetPosY() >= 0 && player.GetPosY() < gameMapHeight - 1)
                {
                    return gameMap[player.GetPosX(), player.GetPosY() + 1] == platformChar;
                }
                return false;
            }
        }

        private void PlayBackgroundMusic()
        {
            string filepath = "C:\\Users\\Adefola\\Documents\\Projects\\Adefola Adeoye - NEA Platformer Game\\Adefola Adeoye - NEA Platformer Game\\bin\\Debug\\Megalovania.wav";
            //plays megalovania
            SoundPlayer soundPlayer = new SoundPlayer(filepath);
            try
            {
                soundPlayer.Play();
                isMusicPlaying = true;
                while (isMusicPlaying)
                {
                    soundPlayer.PlaySync(); // Play the music synchronously
                }
            }
            catch (Exception e)
            {
                string me = e.Message;
                Console.WriteLine(e);
                System.Threading.Thread.Sleep(500);
                Console.Clear();
            }
        }

        // Other level-specific methods and logic
        public void LevelSetUp()
        {
            ClearMap();
            GenerateGameWorld();
            InitializePlayer();
            PlatformsSetUp();
            SpawnEnemies(5);
            DisplayGameWorld();
        }

        public void LevelIntro(int CurrentLevel) // Gives the user a quick introduction
        {
            Console.Clear();
            Console.WriteLine($"Welcome to Level {CurrentLevel}");
            Console.WriteLine("Cue the music.");
            LoadingSequence();
            Console.Clear();
        }


        //Player Object
        private void InitializePlayer()
        {
            Random rnd = new Random();
            int randomX = rnd.Next(0, 6);
            int playerY = gameMapHeight - heightmultiplier;
            player = new Player("PlayerName", randomX, playerY, 0, 0); // Adjust player's initial velocity and displacement as needed
            player.Show(gameMap);
        }

        private void LoadingSequence() //Loading screen until any input is received
        {
            int count = 0;
            while (Console.KeyAvailable == false)
            {

                if (count < 30)
                {
                    Console.Write('.');
                    count++;
                }
                System.Threading.Thread.Sleep(250);
            }
        }

        private bool ReachedEndofLevel() // This works assuming the player has been check to see if they're on the ground.
        {
            return (player.GetPosX() == width - 1);
        }


        private void PlatformsSetUp()
        {
            // Create platforms and add them to the gamemap
            GeneratePlatformsUsingPerlinNoise(7, 8, 11, 1, 2, heightmultiplier - 15, heightmultiplier + 5, 5);

        }

        private void SetUpScore(int S)
        {
            score = S;
            Console.Title = $"NEA Platformer Game - Score: {score}";
        }

        public int GetScore()
        {
            return score;
        }
        private void UpdateScore()
        {
            Console.Title = $"NEA Platformer Game - Score: {score}";
        }

        public void SetScore(int newScore)
        {
            score = newScore;
        }

        public int GetLives()
        {
            return player.GetLives();
        }


        public Player ReturnPlayer()
        {
            return player;
        }

        public void SpawnEnemies(int numberOfEnemies)
        {
            int LeftLimit = 4;
            int RightLimit = 4;
            Random random = new Random();
            for (int i = 0; i < numberOfEnemies; i++)
            {
                int randomX = random.Next(0, width);
                int randomY = random.Next(0, gameMapHeight - heightmultiplier); 

                enemies.Add(new Enemy(randomX, randomY, 0, 0, LeftLimit, RightLimit));
            }

            foreach (Enemy enemy in enemies)
            {
                enemy.AddEnemy(gameMap);
            }
        }

        public void UpdateEnemies()
        {
            foreach (var enemy in enemies)
            {
                enemy.Delete(gameMap);
                enemy.Gravity();
                enemy.CheckTerrainCollision(gameMap);
                enemy.Show(gameMap);
                // Implement logic for enemy movement, gravity, collisions, etc.
                if (player.EnemyCollision(enemy))
                {
                    playerAlive = false;
                    // Optionally, you can handle other things like deducting lives, resetting the level, etc.
                }
            }

        }

        private void ClearEnemies()
        {


        }

    }

}



