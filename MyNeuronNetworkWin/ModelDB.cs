using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNeuronNetworkWin
{
    public partial class Info
    {
        public MainWindow MainWindow
        {
            get => default;
            set
            {
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
