using MyNeuronNetworkWin.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNeuronNetworkWin.Model
{
    public class ImageNeuron : INeuron
    {
        public int[,] Weight { get; set; }


        public int Handle(int[,] input) //сила
        {
            var power = 0;

            for (int y = 0; y < Weight.GetLength(1); y++)
            {
                for (int x = 0; x < Weight.GetLength(0); x++)
                {
                    power += Weight[x, y] * input[x, y];
                }
            }

            return power;
        }

        public void Study(int[,] input, int factor) //обучение
        {
            for (int y = 0; y < Weight.GetLength(1); y++)
            {
                for (int x = 0; x < Weight.GetLength(0); x++)
                {
                    Weight[x, y] += factor * input[x, y];
                }
            }
        }
    }
}
