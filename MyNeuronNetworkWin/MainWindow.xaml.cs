using MyNeuronNetworkWin.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using MyNeuronNetworkWin.Models;
using Microsoft.VisualBasic;

namespace MyNeuronNetworkWin
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public NeuronNetworkBaseEntities1 nnentities;

        public MainWindow()
        {
            InitializeComponent();
            #region Подключение БД
            try
            {
                nnentities = new NeuronNetworkBaseEntities1();
                #endregion


                #region ListBox
                foreach (var name in nnentities.Info) actors_ListBox.Items.Add(name);
                #endregion


            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при подключении к БД! Обратитесь к разработчику", "MyNeuronNetworkWin", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            actors_ListBox.SelectedIndex = 0;
            link_TextBox.Text = "";
            name_TextBox.Text = "";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (link_TextBox.Text != "")
                {
                    int neuronCount = 10; int width = 750; int height = 750;
                    ImageNeuronNetwork net = new ImageNeuronNetwork(neuronCount, width, height);
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.DownloadFile(link_TextBox.Text, "digit.bmp");
                    }
                    var img = System.Drawing.Image.FromFile("digit.bmp");
                    var answer = net.getAnswer(img.ToInput(width, height));

                    bool check = false;

                    if (name_CheckBox.IsChecked == false)
                    {
                        foreach (var name in nnentities.Info)
                        {
                            if (name.IdNeuronNetwork == answer) //сравнивает существующий код в БД со считываемым значением с изображения
                            {
                                var qustion = MessageBox.Show($"Это - {name.Name}. Правильно?", "MyNeuronNetworkWin", MessageBoxButton.YesNo);

                                if (qustion == MessageBoxResult.Yes)
                                {
                                    check = true;
                                    MessageBox.Show("Нейросеть не ошибается! :)", "MyNeuronNetworkWin", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else if (qustion == MessageBoxResult.No)
                                {
                                    check = true;
                                    MessageBox.Show("Без подсказки не справится :)", "MyNeuronNetworkWin", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                Close();
                                break;
                            }
                        }

                        if (check == false)
                        {
                            MessageBox.Show("Данных об этом актере нет", "MyNeuronNetworkWin", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        if (name_TextBox.Text != "")
                        {
                            bool checkA = true;

                            foreach (var name in nnentities.Info)
                            {
                                if (name.IdNeuronNetwork == answer) //сравнивает существующий код в БД с тем кодом которая считает нейросеть
                                {
                                    var qustion = MessageBox.Show($"Это - {name.Name}. Правильно?", "MyNeuronNetworkWin", MessageBoxButton.YesNo);

                                    if (qustion == MessageBoxResult.Yes)
                                    {
                                        check = true;
                                        MessageBox.Show("Нейросеть не ошибается! :)", "MyNeuronNetworkWin", MessageBoxButton.OK, MessageBoxImage.Information);
                                        Close();
                                    }
                                    else if (qustion == MessageBoxResult.No)
                                    {
                                        checkA = true;
                                    }
                                    break;
                                }
                            }

                            if (check == false) //если данных об актере нет или нейросеть допустила ошибку, то заносим данные в нейросеть (и базу данных)
                            {
                                int neu = -1;
                                foreach (var name in nnentities.Info)
                                {
                                    if (name.Name == name_TextBox.Text)
                                    {
                                        neu = int.Parse(name.IdNeuronNetwork.ToString());
                                        checkA = false;
                                        break;
                                    }
                                    else neu = int.Parse(name.IdNeuronNetwork.ToString());
                                }

                                if (checkA == true)
                                {
                                    var add_info = new Info
                                    {
                                        Name = name_TextBox.Text,
                                        IdNeuronNetwork = (neu + 1)
                                    };

                                    nnentities.Info.Add(add_info);
                                    nnentities.SaveChanges();

                                    NeuronAnswer(name_TextBox.Text, int.Parse((neu + 1).ToString()), width, height, img, net);
                                }
                                else NeuronAnswer(name_TextBox.Text, int.Parse(neu.ToString()), width, height, img, net);
                            }
                        }
                        else MessageBox.Show("Поле с именем и фамилией не заполнено", "MyNeuronNetworkWin", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else MessageBox.Show("Поле с ссылкой не заполнено", "MyNeuronNetworkWin", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception)
            {
                MessageBox.Show("Перезапустите программу и повторите запрос", "MyNeuronNetworkWin", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }

        public void NeuronAnswer(string name, int input, int width, int height, System.Drawing.Image img, ImageNeuronNetwork net)
        {
            try
            {
                img.ToInput(width, height);
                net.Study(img.ToInput(width, height), input, 1);
                MessageBox.Show($"Хорошо, я запомнил что это - {name}", "MyNeuronNetworkWin", MessageBoxButton.OK, MessageBoxImage.Information);
                net.JSSer();
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка записью в нейросеть! Обратитесь к разработчику", "MyNeuronNetworkWin", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        public void WebSite(string link)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(link, "digit.bmp");
            }
        }

        private void PasteLink_Click(object sender, RoutedEventArgs e)
        {
            if (Clipboard.ContainsText() == true) link_TextBox.Text = Clipboard.GetText();
        }

        private void Name_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            name_TextBox.IsEnabled = true;
        }

        private void Name_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            name_TextBox.IsEnabled = false; name_TextBox.Text = "";
        }

        private void Actors_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (name_CheckBox.IsChecked == true)
            {
                var selected_actors = actors_ListBox.SelectedItem as Info;

                if (selected_actors != null)
                {
                    name_TextBox.Text = selected_actors.Name;
                }
            }
        }
    }
}
