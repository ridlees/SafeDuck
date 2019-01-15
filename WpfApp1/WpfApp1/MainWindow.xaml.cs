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
{   //I want to thank Rudityas W. Anggoro for the amazing Icon I am using. Please, check his work here https://dribbble.com/rudityaswahyu
    //and link to the file I am using is here https://icon-icons.com/icon/animal-character-psyduck-screech-yellow/11268 //
    //have fun with this program.
    //Sincerely,
    //Martin
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int ChildrenExists = 0;
        void Windowclosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult r = MessageBox.Show("Do you want to leave?", "You are leaving SafeDuck", MessageBoxButton.YesNo);
            if (r == MessageBoxResult.Yes)
            {
                try
                {
                      
                if (File.Exists("obrazek.png") == true)
                {
                    if (ChildrenExists == 1)
                    {
                        canvas.Children.RemoveAt(ChildrenExists);
                        ChildrenExists = 0;
                    }
                    File.Delete("obrazek.png");
                }
            }
                catch (IOException ie)
                {
                    // Extract some information from this exception, and then 
                    // throw it to the parent method.         
                    throw;
                }

            }
            else
            {
                e.Cancel = true;
            }
        }
        public void Abort ()
        {
            if (ChildrenExists != 0)
            {
                canvas.Children.RemoveAt(ChildrenExists);
                ChildrenExists = ChildrenExists - 1;
            }
        }
        public  void Abortion (object sender, RoutedEventArgs e)
        {
            Abort();
        }
        //Please, take a moment of silence for Tim May, who passed this week (around 15th of December). Cyperpunk inspired me to create this project and I am sad that Mr. May passed away. Rest in Piece. Please, if you are unaware of his work, check it here: 
        private void DrawLine (object sender, RoutedEventArgs e)
        {
            //TODO drawing           
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
                //TODO experimental decryption via číselné řady ... will work via a passphrase (password) that will be encoded as first word of the text- then I will use series of numbers that will tell me where to look for the pixels. Then, you will use a system of Math rules to deterrminate the position of ref pixel. It could be one of 15 pixels around. 
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
            if( ChildrenExists== 1) { 
            canvas.Children.RemoveAt(ChildrenExists);
                ChildrenExists = 0;
                Window1 win1 = new Window1();
                win1.ShowDialog();
               // Refresh(); --> doesnt make sense in this context
            }
            else
            {
                MessageBox.Show("Please, add a picture to continue. How can I encrypt your text into your picture without it?", "ERROR - No image added");
            }
            
        }
        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            Abort();
            if (File.Exists("obrazek.png")==true)
            { File.Delete("obrazek.png"); }
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
                    ChildrenExists = ChildrenExists + 1;

                }
                else
                {
                    MessageBox.Show("The image is not supported. Please, use another one that has at least 500 px");
                }

            }

        }
        //TODO margin-top a few px
        //TODO drawing with mouse button ---> I will throw this idea away, kinda stupid and hard to work on.
       // private System.Windows.Controls.Image draggedImage;
       // private System.Windows.Point mousePosition;
        private void CanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           //Button
        }
        private void CanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //Button released
        }
        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
           //Draw line move
        }
        public void ButtonDecrypt(object sender, RoutedEventArgs e)
        {
           string Text = Decrypt();
            char uvozovky= '"';
            MessageBoxResult r = MessageBox.Show("The clear message is: "+uvozovky+Text+uvozovky, "Do you want to save your message?", MessageBoxButton.YesNo);
            if (r == MessageBoxResult.Yes)
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "encrypted"; // Default file name
                dlg.DefaultExt = ".txt"; // Default file extension
                dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    // Save document
                    string filename = dlg.FileName;
                    System.IO.File.WriteAllText(filename + ".txt", Text);
                } 
                else
                {
                    MessageBox.Show("I am sorry, but you didn´t choose the location", "Save-Error");
                }
            }
        }
        public string Decrypt()
        {
            System.Drawing.Image imgur = System.Drawing.Image.FromFile("obrazek.png");
            Bitmap img = new Bitmap(imgur);
            imgur.Dispose();
            string Text = "";
            string[] Positions = System.IO.File.ReadAllLines("positions.txt"); //now I have all positions here;
            //TODO test
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
                if (R < 0)
                {
                    R = R * -1;
                }
                if (G < 0)
                {
                    G = G * -1;
                }
                if (B < 0)
                {
                    B = B * -1;
                }
                char Symbol = DecryptAlpha(R, G, B);
                if (Symbol == ']')
                {
                    return Text;
                }
                Text = Text + Symbol;
            }
            return Text;
        } 
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
        public void Switch() //not used now
        {   //to save the changed picture into the original file
            //Unfortunately, VS needs the original file to be open the whole time it is used, it can not be saved into a new place//tried many times, but VS always showed me a GDI error.
            File.Delete("obrazek.png");
            Bitmap img = new Bitmap(System.Drawing.Image.FromFile("obrazek-line.png"));
            img.Save("obrazek.png");
            img.Dispose();
            File.Delete("obrazek-line.png");
        }
        public void ButtonDecrypt_full(object sender, RoutedEventArgs e)
        {
            Window3 password = new Window3();
            password.ShowDialog();
            string User_password = password.password.Text;
            //I get the password and I need to find the User_password via open alphabet on first lines. 
            //then I need to find the alphabet and generate alphabet file from this line
            // and then I can run "almost" normal decryption :D
            GenerateOpenAlphabet();
            PasswordFinder(User_password);
            //string Text = Decrypt();
            char uvozovky = '"';
            MessageBoxResult r = MessageBox.Show("The clear message is: " + uvozovky + User_password + uvozovky, "Do you want to save your message?", MessageBoxButton.YesNo);
            if (r == MessageBoxResult.Yes)
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "encrypted"; // Default file name
                dlg.DefaultExt = ".txt"; // Default file extension
                dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    // Save document
                    string filename = dlg.FileName;
                    System.IO.File.WriteAllText(filename + ".txt", User_password);
                }
                else
                {
                    MessageBox.Show("I am sorry, but you didn´t choose the location", "Save-Error");
                }
            }
        }
        public List<string> AplhabetList = new List<string>();
        public void PasswordFinder(string password)
        {
            MessageBox.Show("I am sorry, but you didn´t choose the location", "Save-Error");
        }
        public void GenerateOpenAlphabet()
        {
            string ABC = "A/a/B/b/C/c/D/d/E/e/F/f/G/g/H/h/I/i/J/j/K/k/L/l/M/m/N/n/O/o/P/p/Q/q/R/r/S/s/T/t/U/u/V/v/W/w/X/x/Y/y/Z/z/0/1/2/3/4/5/6/7/8/9/@/./-/ /_/]";
            string[] SymbolOpen = ABC.Split('/');
            int open_position_symbol = 0;
            StreamWriter sw = new StreamWriter("Aplhabet_open.txt");
            string number = "";
            for (int symbol = 0; symbol < 5; symbol++)
            {
                for (int symbole = 0; symbole < 3; symbole++)
                {
                    for (int symbola = 0; symbola < 5; symbola++)
                    {
                        if (open_position_symbol < SymbolOpen.Count())
                        {
                            number = SymbolOpen[open_position_symbol] + "/" + symbol + symbole + symbola;
                        }
                        else if (open_position_symbol >= SymbolOpen.Count())
                        {
                            number = "]" + "/" + symbol + symbole + symbola;

                        }
                        AplhabetList.Insert(open_position_symbol, number);
                        sw.WriteLine(number);
                        open_position_symbol++;
                    }
                }
            }
            sw.Close();
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