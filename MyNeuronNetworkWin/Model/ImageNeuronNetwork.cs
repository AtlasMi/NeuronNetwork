using MyNeuronNetworkWin.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNeuronNetworkWin.Model
{
    public class ImageNeuronNetwork : INeuronNetwork
    {
        const string NeuronsPath = "Neurons.json";

        public INeuron[] Neurons { get; set; }

        public ImageNeuronNetwork(int neuronCount, int width, int height)
        {
            if (!File.Exists(NeuronsPath)) //если файла нет, он будет создан и ему будут переданы введенные параметры
            {
                Neurons = new ImageNeuron[neuronCount];

                for (int i = 0; i < Neurons.Length; i++)
                {
                    Neurons[i] = new ImageNeuron
                    {
                        Weight = new int[width, height]
                    };
                }
            }
            else
            {
                Neurons = JsonHelper.Deserialize<ImageNeuron[]>(NeuronsPath); //если нейрон уже есть, то данные будут загружаться в одномерный массив
            }
        }

        public void JSSer()
        {
            JsonHelper.Serialize(Neurons, NeuronsPath);
        }

        public int getAnswer(int[,] input) //возврат ответа
        {
            var answers = new int[Neurons.Length];

            for (int i = 0; i < Neurons.Length; i++)
            {
                answers[i] = Neurons[i].Handle(input);
            }

            int maxIndex = 0;

            for (int i = 0; i < answers.Length; i++)
            {
                if (answers[i] > answers[maxIndex])
                {
                    maxIndex = i;
                }
            }

            return maxIndex;
        }

        public void Study(int[,] input, int correctAnswer, int factor) //запоминание к корректному нейрону
        {
            Neurons[correctAnswer].Study(input, 1);
        }
    }
}
