using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;

namespace MyNeuronNetworkWin.Models
{
    public static class ImageEx
    {
        public static int[,] ToByte(this Image img) //преобразует изображение в двумерный массив в двоичном коде
        {
            int[,] mass = null;

            try
            {
                var bmp = new Bitmap(img);
                mass = new int[bmp.Width, bmp.Height];

                for (int y = 0; y < img.Height; y++)
                {
                    for (int x = 0; x < img.Width; x++)
                    {
                        var IsWhite = bmp.GetPixel(x, y).R >= 230 && bmp.GetPixel(x, y).G >= 230 && bmp.GetPixel(x, y).B >= 230;
                        mass[x, y] = IsWhite ? 0 : 1;
                    }
                }
                
            }
            catch(IndexOutOfRangeException)
            {
                MessageBox.Show("Было выбрано некорректное изображение. Попробуйте выбрать другое изображение. Перезапустите программу", "MyNeuronNetworkWin", MessageBoxButton.OK, MessageBoxImage.Error);
                MainWindow mw = new MainWindow();
                mw.Close();
            }
            return mass;
        }

        public static Image ScaleImage(this Image source, int width, int height) //увеличение изображение в размерах и "размещение по центру"
        {
            Image dest = new Bitmap(width, height);
            using (Graphics gr = Graphics.FromImage(dest))
            {
                gr.FillRectangle(Brushes.White, 0, 0, width, height); //очищаем экран
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                float srcwidth = source.Width;
                float srcheight = source.Height;
                float dstwidth = width;
                float dstheight = height;

                if (srcwidth <= dstwidth && srcheight <= dstheight) //исходное изображение меньше целевого
                {
                    int left = (width - source.Width) / 2;
                    int top = (height - source.Height) / 2;
                    gr.DrawImage(source, left, top, source.Width, source.Height);
                }
            }

            return dest;
        }

        public static int[,] ToInput(this Image sourse, int width, int height) //превращение изображения в необходимый вид для нейронной сети
        {
            return sourse.ScaleImage(width, height).ToByte();
        }
    }
}
