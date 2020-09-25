using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Threading;

namespace WPFClinica.Pages {

    public partial class BlankPage : Page {

        DispatcherTimer timer = new DispatcherTimer();
        bool Increasing = false;

        public BlankPage() {
            InitializeComponent();
            MyImage.Source = ImageH.ToBitmapImage(Properties.Resources.Logo);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e) {
            double amount = 0.020;
            if (Increasing) {
                MyImage.Opacity += amount;
                if (MyImage.Opacity > 0.8) {
                    Increasing = false;
                }
            } else {
                MyImage.Opacity -= amount;
                if (MyImage.Opacity < 0.2) {
                    Increasing = true;
                }
            }
        }
    }
}
