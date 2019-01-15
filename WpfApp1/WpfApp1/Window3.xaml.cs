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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interakční logika pro Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        public Window3()
        {
            InitializeComponent();
           
        }
        private void Password_MouseClick(Object sender, MouseEventArgs e)
        {
            password.Text = String.Empty;
        }
        private void Enter (object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                 {
                Close();

            }

        }
    }
}
