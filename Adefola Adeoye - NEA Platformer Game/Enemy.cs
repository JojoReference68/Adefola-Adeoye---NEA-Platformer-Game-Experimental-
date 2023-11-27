using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Adefola_Adeoye___NEA_Platformer_Game
{
    public class Enemy
    {
        private int XPos;
        private int YPos;
        private double initialVelocity;
        private double acceleration;// Acceleration due to gravity;
        private char character = '+';
        private int terminalVelocity = 7;
        private double Xvelocity;
        private int leftBoundary;
        private int rightBoundary;
        public Enemy(int x, int y, double v, double d, int left, int right)
        {
            XPos = x;
            YPos = y;
            Xvelocity = 2;
            initialVelocity = v;
            acceleration = 6; // You can adjust this value based on how fast you want the enemies to fall.
            leftBoundary = left;
            rightBoundary = right;
        }

        public void Delete(char[,] map) //Deletes player character
        {
            if (map[XPos, YPos] != '=' || map[XPos, YPos] != '█')
            {
                map[XPos, YPos] = ' ';
                WriteCharToConsole(map);
            }

        }
        public void Move()
        {

            if (XPos <= leftBoundary)
            {
                XPos = leftBoundary;
                initialVelocity = Math.Abs(initialVelocity); // Move to the right
            }
            else if (XPos >= rightBoundary)
            {
                XPos = rightBoundary;
                initialVelocity = -Math.Abs(initialVelocity); // Move to the left
            }

            XPos += (int)initialVelocity;
        }
        public void Show(char[,] map) //Shows player character
        {
            if (map[XPos, YPos] != '=' || map[XPos, YPos] != '█')
            {
                map[XPos, YPos] = character;
                WriteCharToConsole(map);
            }

        }

        public void WriteCharToConsole(char[,] map) //Writes the player character to the console in correspondence to where it should be on the map and writes in the green colour
        {
            Console.CursorLeft = XPos;
            Console.CursorTop = YPos;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(map[XPos, YPos]);
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorLeft = XPos;
            Console.CursorTop = YPos;
        }

        public void SetPosX(int newX)
        {
            XPos = newX;
        }

        public void SetPosY(int newY)
        {
            YPos = newY;
        }

        public void SetPos(int newX, int newY)
        {
            XPos = newX;
            YPos = newY;
        }

        public void MoveLeft(char[,] map) //moves the character to the left of the map
        {
            if (map[XPos - 1, YPos] != '=' || map[XPos, YPos] != '█')
            {
                Delete(map);
                XPos--;
                Show(map);
            }
        }

        public void MoveRight(char[,] map) //moves the character to the left of the map
        {
            if (map[XPos + 1, YPos] != '=' || map[XPos, YPos] != '█')
            {
                Delete(map);
                XPos++;
                Show(map);
            }
        }

        public int GetPosX() //returns Player Xposition
        {
            return XPos;
        }
        public int GetPosY()    //returns Player Yposition
        {
            return YPos;
        }

        public double GetInitialVelocity() //returns IniitialVelocity
        {
            return initialVelocity;
        }
        public void SetInitialVelocity(double newV)
        {
            initialVelocity = newV;
        }

        public double GetAcceleration() //returns acceleration,
        {
            return acceleration;
        }

        public void Gravity() //Simulate gravity using suvat equations
        {
            double deltaTime = 0.1;
            double velocity = initialVelocity + (acceleration * deltaTime);

            if (velocity >= terminalVelocity) //sets a speed limit to the player
            {
                velocity = terminalVelocity;
            }
            int newYPos = YPos + (int)Math.Round(velocity * deltaTime);
            if (newYPos < 0)
            {
                newYPos = 0;
                velocity = 0;
            }

            YPos = newYPos;
            initialVelocity = velocity;
        }

        public void CheckTerrainCollision(char[,] map)
        {
            if (map[XPos, YPos + 1] == '=' || map[XPos, YPos + 1] == '█')
            {
                YPos = YPos - (int)initialVelocity;
                initialVelocity = 0; 
            }
        }

        public char Character()
        {
            return character;
        }

        public void AddEnemy(char[,] map)
        {
            map[XPos, YPos] = character;
        }
    }
}
