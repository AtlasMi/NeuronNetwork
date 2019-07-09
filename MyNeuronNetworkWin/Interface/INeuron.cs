using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNeuronNetworkWin.Interface
{
    public interface INeuron
    {
        int[,] Weight { get; set; } //память нейрона

        int Handle(int[,] input); //сила нейрона

        void Study(int[,] input, int factor); //обучение нейрона
    }
}
