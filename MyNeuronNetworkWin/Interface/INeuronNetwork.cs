using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNeuronNetworkWin.Interface
{
    public interface INeuronNetwork
    {
        INeuron[] Neurons { get; set; } //массив интерфейсов

        int getAnswer(int[,] input); //возврат правильного ответа по мнению нейронной сети

        void Study(int[,] input, int correctAnswer, int factor); //обучение конкретного нейрона и уверенность выбора
    }
}
