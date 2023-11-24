using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Adefola_Adeoye___NEA_Platformer_Game
{
    public class Player
    {
        private string name;
        private char character = '*';
        private int XPos;
        private int YPos;
        private double initialVelocity;
        private double acceleration; // Acceleration due to gravity;
        private double displacement;
        private int lives;

        public Player(string username, int X, int Y, double v, double d)    //Initialize player object
        {
            name = username;
            XPos = X;
            YPos = Y;
            initialVelocity = v;
            acceleration = 6;
            displacement = d;
            lives = 3;
        }

        public void Delete(char[,] map) //Deletes player character
        {
            if (map[XPos, YPos] != '=' || map[XPos, YPos] != '█')
            {
                map[XPos, YPos] = ' ';
                WriteCharToConsole(map);
            }

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
            Console.ForegroundColor = ConsoleColor.Green;
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

        public string GetName()
        {
            return name;
        }

        public int GetPosX() //returns Player Xposition
        {
            return XPos;
        }

        public void MoveUp()
        {
            initialVelocity = -15; // Initial upward velocity
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

        public double GetAcceleration() //returns acceleration
        {
            return acceleration;
        }

        public void ChangeName(string username)
        {
            name = username;
        }

        public void Die()
        {
            lives--;
        }

        public int GetLives()
        {
            return lives;
        }
    }
}
