using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adefola_Adeoye___NEA_Platformer_Game
{
    public class Terrain_Generator
    {
        private int width;
        private float persistence;
        private int octaves;

        public Terrain_Generator(int w, float persist, int octa)
        {
            width = w;
            persistence = persist;
            octaves = octa;
        }

        public float[] GeneratePerlinNoise()
        {
            Random randomiser = new Random();
            float[] x = new float[width];
            float[] y = new float[width];
            float frequency = (float)((randomiser.Next(2, 6) / 10f) + (randomiser.NextDouble() / 10)); //was 0.25f
            float amplitude = randomiser.Next(10, 31); //was 10f
            Random random = new Random(42);

            for (int i = 0; i < width; i++)
            {
                x[i] = (float)i / width * 10f;
                y[i] = 0f;
            }

            for (int octave = 0; octave < octaves; octave++)
            {
                for (int i = 0; i < width; i++)
                {
                    y[i] += amplitude * GeneratePerlinWave(x[i] * frequency, random);
                }
                frequency *= 2f;
                amplitude *= persistence;
            }

            // Normalize the values to fit within [0, 1] 
            float minY = y[0];
            float maxY = y[0];
            for (int i = 1; i < width; i++)
            {
                if (y[i] < minY) minY = y[i];
                if (y[i] > maxY) maxY = y[i];
            }

            for (int i = 0; i < width; i++)
            {
                y[i] = (y[i] - minY) / (maxY - minY);
            }

            return y;
        }

        private float GeneratePerlinWave(float x, Random random)
        {
            int x0 = (int)Math.Floor(x) & 255;
            int x1 = (x0 + 1) & 255;
            float t = x - (float)Math.Floor(x);
            float fade = t * t * t * (t * (t * 6f - 15f) + 10f);
            int p0 = permutation[x0];
            int p1 = permutation[x1];

            return Lerp(Grad(p0, t), Grad(p1, t - 1f), fade);
        }

        private static float Lerp(float a, float b, float t)
        {
            return a + t * (b - a);
        }

        private static float Grad(int hash, float x)
        {
            int h = hash & 15;
            float grad = 1f + (h & 7); // Gradient value
            if ((h & 8) != 0) grad = -grad; // Randomly invert half of the gradients
            return (grad * x);
        }

        private static int[] permutation = new int[]
        {
        151,160,137,91,90,15,
        131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
        190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
        88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
        77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
        102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
        135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
        5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
        223,183,170,213,119,248,152, 2,44,154,163,70,221,153,101,155,167, 43,172,9,
        129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
        251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
        49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
        138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
        };
    }
}
