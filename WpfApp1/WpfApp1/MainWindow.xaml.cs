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
using Microsoft.Win32;
using System.IO;
using System.Globalization;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;
using WpfApp1;

namespace WpfApp1
{   //I want to thank Rudityas W Anggoro for amazing Icon I am using. Please, check his work here https://dribbble.com/rudityaswahyu
    //and link to the file I am using is here https://icon-icons.com/icon/animal-character-psyduck-screech-yellow/11268
    //have fun with this program.

        //Sincerely,
    //Martin
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        void Windowclosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult r = MessageBox.Show("Do you want to leave?", "You are leaving SafeID", MessageBoxButton.YesNo);
            if (r == MessageBoxResult.Yes)
            {
                File.Delete("obrazek.png");

            }
            else
            {
                e.Cancel = true;

            }

        }
        public  void Abortion (object sender, RoutedEventArgs e)
        {
                canvas.Children.RemoveAt(1);
                

        }
        private void DrawLine (object sender, RoutedEventArgs e)
        {
            Refresh();
           
        }
       
        private void  Refresh ()
        {           
            Bitmap bitmap = new Bitmap(System.Drawing.Image.FromFile("obrazek.png"));
            BitmapImage BitmapImag = new BitmapImage();
            BitmapImag = Bitmap2BitmapImage(bitmap);
            var image = new System.Windows.Controls.Image { Source = BitmapImag };
            canvas.Children.Add(image);
            bitmap.Dispose();
            
           
        }
        public static BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        {
            using (var memory= new MemoryStream())
            {

                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                memory.Dispose();
                return bitmapImage;

            }
        }
        public void BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {

                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);
                bitmap.Save("obrazek.png", ImageFormat.Png);
                outStream.Dispose();
            }
        }
        public bool TestImage(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {

                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);
                int n1 = bitmap.Size.Height * bitmap.Size.Width;
                if (n1 < 500)
                {
                    outStream.Dispose();
                    return false; //this should be uncommon, but who knows what will the user do.
                }
                else
                {
                    outStream.Dispose();
                    return true;
                }
                
            }
        }
        private void Encrypt(object sender, RoutedEventArgs e)
        {
            canvas.Children.RemoveAt(1);
            Window1 win1 = new Window1();
            win1.ShowDialog();
        }
        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter =
                "Image Files (*.png;*.jpg; *.jpeg; *.gif; *.bmp)|*.png;*.jpg; *.jpeg; *.gif; *.bmp";

            if ((bool)dialog.ShowDialog())
            {   
                var bitmap = new BitmapImage(new Uri(dialog.FileName));
                var image = new System.Windows.Controls.Image { Source = bitmap };
                // Test if the image has at least 500 px (so it can work in this project
                bool r = (bool)TestImage(bitmap);
                if (r == true)
                {
                    canvas.Children.Add(image);
                    BitmapImage2Bitmap(bitmap);

                }
                else
                {
                    MessageBox.Show("The image is not supported. Please, use another one that has at least 500 px");
                }

            }

        }
        //TODO drawing with mouse button
       // private System.Windows.Controls.Image draggedImage;
       // private System.Windows.Point mousePosition;

        private void CanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           //
        }

        private void CanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //
        }

        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
           //
        }
        public void ButtonDecrypt(object sender, RoutedEventArgs e)
        {
           string Text = Decrypt();
            MessageBoxResult r = MessageBox.Show(Text, "Do you want to save your message?", MessageBoxButton.YesNo);
            if (r == MessageBoxResult.Yes)
            {
                System.IO.File.WriteAllText("encrypted.txt", Text);
            }
        }

        public string Decrypt()
        {
            Bitmap img = new Bitmap(System.Drawing.Image.FromFile("obrazek.png"));
            string Text = "";
            string[] Positions = System.IO.File.ReadAllLines("positions.txt"); //now I have all positions here;
            for (int DecryptRun = 0; DecryptRun <250; DecryptRun++) //for each char I check the pixels
            {
                int R = 0;
                int G = 0;
                int B = 0;
                string line = Positions[DecryptRun]; //the position line
                string[] Poses = line.Split(' ');
                int xOrigin = Int32.Parse(Poses[0]);
                int YOrigin = Int32.Parse(Poses[1]);
                int Ref = Int32.Parse(Poses[2]);
                int yref; //now I know where to look
                if (Ref == 1)
                {
                    
                    yref = YOrigin - 1;
                }
            else
            {
                    
                    yref = YOrigin + 1;
                }
                System.Drawing.Color cref = img.GetPixel(xOrigin, yref);
                System.Drawing.Color corigin = img.GetPixel(xOrigin, YOrigin);
                R = cref.R - corigin.R;
                G = cref.G - corigin.G;
                B = cref.B - corigin.B;
                char Symbol = DecryptAlpha(R, G, B);
                Text = Text + Symbol;
            }
            return Text;
        } //Decryption works, TODO ---> "the only message show (delete from text all [ symbols)
        public char DecryptAlpha(int R, int G, int B)
        {
            char Symbol = ' ';
            string[] Alphabet = System.IO.File.ReadAllLines("Alphabet.txt");
            var list = Alphabet.Take(75).ToList();
            List<string> Alphastrings = new List<string>(list); ;
            for (int i = 0; i < 75; i++)
            {
                string stringchar = Alphastrings[i];
                string[] Poses = stringchar.Split('/');
                char[] arr;
                arr =Poses[1].ToCharArray(0, 3);
                char RAlpha = arr[0];
                char GBeta = arr[1];
                char BGamma = arr[2];
                if (RAlpha == R + '0' && GBeta == G + '0'&& BGamma == B +'0')
                {
                    Char[] Dante = Poses[0].ToCharArray(0, 1);
                    Symbol = Dante[0];
                }
            }

            return Symbol;
        }
        public MainWindow()
        {
            Window2 win2= new Window2();
            bool r = (bool)win2.ShowDialog();
            if (r == true)
            {
                InitializeComponent();
                this.Closing += new CancelEventHandler(this.Windowclosing);
            }
            else
            {
                this.Close();
            }

        }
 
    }
}
