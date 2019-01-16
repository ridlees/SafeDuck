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
using System.ComponentModel;

namespace WpfApp1
{
    /// <summary>
    /// Interakční logika pro Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        void PasswordClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult r = MessageBox.Show("Do you want to leave without writing your password?", "You need to press enter to proceed", MessageBoxButton.YesNo);
            if (r == MessageBoxResult.Yes)
            {
                //nothing
            }
            else
            {
                e.Cancel = true;
            }
        }
        public Window3()
        {
            InitializeComponent();
            this.Closing += new CancelEventHandler(this.PasswordClose);
        }
        private void Password_MouseClick(Object sender, MouseEventArgs e)
        {
            password.Text = String.Empty;
        }
        private void Enter (object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                 {
                this.Closing -= new CancelEventHandler(this.PasswordClose);
                DialogResult = true;
            }

        }
    }
}
