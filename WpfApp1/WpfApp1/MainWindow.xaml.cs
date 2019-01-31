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
            System.Drawing.Image imgur = System.Drawing.Image.FromFile("obrazek.png");
            Bitmap bitmap = new Bitmap(imgur);
            imgur.Dispose();
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
                    //I need to expect anything possible from them
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
         if (File.Exists("obrazek.png")== true)
            {
                string Text = Decrypt();
                char uvozovky = '"';
                MessageBoxResult r = MessageBox.Show("The clear message is: " + uvozovky + Text + uvozovky, "Do you want to save your message?", MessageBoxButton.YesNo);
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
           else
            {
                MessageBox.Show("I am sorry, but there is no picture added", "Add-Error");
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
            //not needed, will remove during decluttering
            //Unfortunately, VS needs the original file to be open the whole time it is used, it can not be saved into a new place//tried many times, but VS always showed me a GDI error.
            File.Delete("obrazek.png");
            System.Drawing.Image imgur = System.Drawing.Image.FromFile("obrazek-line.png");
            Bitmap img = new Bitmap(imgur);
            imgur.Dispose();
            img.Save("obrazek.png");
            img.Dispose();
            File.Delete("obrazek-line.png");
        }
        public void ButtonDecrypt_full(object sender, RoutedEventArgs e)
        {
            if (File.Exists("obrazek.png") == true) { 
                Window3 password = new Window3();
            bool r1 = (bool)password.ShowDialog();
            if (r1 == true)
            {   
            //I get the password and I need to find the User_password via open alphabet on first lines. 
            //then I need to find the alphabet and generate alphabet file from this line
            // and then I can run "almost" normal decryption :D
            GenerateOpenAlphabet();
            string User_message=PasswordFinder(password.password.Text);
            //string Text = Decrypt();
            char uvozovky = '"';
            MessageBoxResult r = MessageBox.Show("The clear message is: " + uvozovky + User_message + uvozovky, "Do you want to save your message?", MessageBoxButton.YesNo);
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
                    System.IO.File.WriteAllText(filename + ".txt", User_message);
                }
                if (result == false)
                {
                    MessageBox.Show("I am sorry, but you didn´t choose the location", "Save-Error");
                }
            }
        }
            else
            {
                MessageBox.Show("I am sorry, but you didn´t input the password", "Password_Input-Error");
            }
        }
            else { MessageBox.Show("I am sorry, but there is no picture added", "No_picture_added-Error"); }
        }
        public string ABC = "A/a/B/b/C/c/D/d/E/e/F/f/G/g/H/h/I/i/J/j/K/k/L/l/M/m/N/n/O/o/P/p/Q/q/R/r/S/s/T/t/U/u/V/v/W/w/X/x/Y/y/Z/z/0/1/2/3/4/5/6/7/8/9/@/./-/ /_/]";
        public string ABCwith = "A/a/B/b/C/c/D/d/E/e/F/f/G/g/H/h/I/i/J/j/K/k/L/l/M/m/N/n/O/o/P/p/Q/q/R/r/S/s/T/t/U/u/V/v/W/w/X/x/Y/y/Z/z/0/1/2/3/4/5/6/7/8/9/@/./-/ /_/]/]/]/]/]/]/]/]";
        public List<string> AplhabetList = new List<string>(); //place where I store the whole alphabet (so letter/and three digits)
        public List<int> Jumplist = new List<int>(); //place to store all jumps
        public string PasswordFinder(string password)
        { //monsterous function
            string User_message = "";
            System.Drawing.Image imgur = System.Drawing.Image.FromFile("obrazek.png");
            Bitmap img = new Bitmap(imgur);
            imgur.Dispose();
            char[] arr;
            for (int m = 10 - password.Length; m > 0; m = m - 1)
            {
                password = password + ']';
            }
            arr = password.ToCharArray(0, password.Length);
            string[] Passwurd = System.IO.File.ReadAllLines("Aplhabet_open.txt");
            var list = Passwurd.Take(75).ToList();
            List<string> passwurdstrings = new List<string>(list);
            List<string> passwordRGB = new List<string>();
            for (int passwordfind = 0; passwordfind < 10; passwordfind++)
            {
                int Found = 0;
                int i = 0;
                while (Found == 0) // and for cycle is too long
                {
                    
                    string str = passwurdstrings[i];
                    string[] Poses = str.Split('/');
                    char[] arte = Poses[0].ToCharArray(0,1);
                    
                    i++;
                    if (arte[0] == arr[passwordfind])
                    {
                        passwordRGB.Insert(passwordfind, Poses[1]);
                        Found = 1;
                    }
                   
                }
                Found = 0;
            }
            //now I have all RGB passwords in my passwordRGB list :/

            //Look at the line and then go wau
            int x = 0;
            int y = 0;
            int foundpixel = 0;
            int lastpixel = 0;
            while ( foundpixel < 10)
            {
                string RGBs = passwordRGB[foundpixel];
                char[] arte = RGBs.ToCharArray(0, RGBs.Length);
                if ( x >= img.Width) //if I am on the end or not
                {
                    x =x - img.Width;
                    y = y + 2;
                }
                System.Drawing.Color cref = img.GetPixel(x, y);
                System.Drawing.Color corigin = img.GetPixel(x, y + 1);
                int R = cref.R - corigin.R;
                int G = cref.G - corigin.G;
                int B = cref.B - corigin.B;
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
                if (arte[0]- '0' == R && arte[1] - '0' == G && arte[2] - '0' == B)
                {
                    int Next = 0;
                    //TODO last pixel
                    if (x< lastpixel) { Next = img.Width - lastpixel;
                        Next = Next + x;
                    }
                    else {Next= x - lastpixel;
                         }
                    Jumplist.Insert(foundpixel, Next);
                    foundpixel++;
                    lastpixel = x;
                }
                x++;

            }
            //look at the aplhabet now. I have Jumps
            int yalphabet = 0;
            if (3 * Jumplist[0] > 2 * Jumplist[5])
            {
                yalphabet = 3 * Jumplist[0] - 2 * Jumplist[5];
            }
            else
            {
                yalphabet = 2 * Jumplist[5] - 3 * Jumplist[0];
            }
            int firstyalphabet = yalphabet;
            //now i know where the alphabet is stored, so the next step is to go for alphabet directly.
            string[] SymbolOpen = ABCwith.Split('/');
            x = 0;// i have the last pixel in lastpixel, so I dont care about this sweetie :)
            int yalpha = y; //but I care aboút Y
            int numberofyalpha = 0; //for encoding
            for (int alphastored = 0; alphastored < 75; alphastored++ )
            {
                if (x > img.Width)
                {
                    yalphabet = yalphabet + 2;
                    x = x - img.Width;
                    numberofyalpha++;
                }
                System.Drawing.Color cref = img.GetPixel(x, yalphabet);
                System.Drawing.Color corigin = img.GetPixel(x, yalphabet + 1);
                int R = cref.R - corigin.R;
                int G = cref.G - corigin.G;
                int B = cref.B - corigin.B;
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
                string line = SymbolOpen[alphastored] + '/' + R + G + B;
                AplhabetList.Insert(alphastored, line);
                x++;
            } //now I have the alphabet ready to be used. So, another for cycle is here for us :/
            //test
           
            
            ///test
            x = lastpixel; //so, now we are back at the last password pixel
            y = yalpha;
            int Jumpnumber = 0; //number of jumps
            for (int Message = 0; Message<250; Message++)
            {
                if (Jumpnumber == 10)
                {
                    Jumpnumber = 0;
                }
                x = x + Jumplist[Jumpnumber];
                Jumpnumber++;
                if (x >= img.Width)
                {
                    y = y + 2;
                    x =  x - img.Width;
                }
                if (y + 1 > img.Height)
                {
                    y = 3;
                }
                if (numberofyalpha != 0)
                {
                    if (y == firstyalphabet && y == firstyalphabet - 1)
                    {
                        y = y + 3 * numberofyalpha;
                    }
                }
                if (numberofyalpha == 0)
                {
                    if (y == firstyalphabet && y == firstyalphabet - 1)
                    {
                        y = y + 3;
                    }
                }
                //checks if x/y are in good places :)
                System.Drawing.Color cref = img.GetPixel(x, y);
                System.Drawing.Color corigin = img.GetPixel(x, y + 1);
                int R = cref.R - corigin.R;
                int G = cref.G - corigin.G;
                int B = cref.B - corigin.B;
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
                //now I have the "digits", I need to get letter :)
                //here is an issue. The AplhabetList works, but the values i get from this while doesn´t work
                //need to rewrite it works for first 9... the bug is in window1 probably
                int letterfound = 0;
                int Alpha = 0;
                while (letterfound == 0){
                  string lister= AplhabetList[Alpha];
                    Alpha++;
                    string[] RGBletters = lister.Split('/');
                    char[] arte = RGBletters[1].ToCharArray(0, RGBletters[1].Length);
                    if (arte[0] - '0' == R && arte[1] - '0' == G && arte[2] - '0' == B)
                    {
                        //I have the char now
                        letterfound = 1;
                        char[] Dante = RGBletters[0].ToCharArray(0, RGBletters[0].Length);
                        char Symbol = Dante[0];
                        if (Symbol == ']')
                        {
                            //nothing
                        }
                        else
                        {
                            User_message = User_message + Symbol;
                        }
                        
                    }
                    
                }
            }
            StreamWriter sw1 = new StreamWriter("decodejump.txt");
            for (int i2 = 0; i2 < 9; i2++)
            {
                string lol = "" + Jumplist[i2];
                sw1.WriteLine(lol);
            }
            sw1.WriteLine("");
            sw1.WriteLine("");
            for (int in2 = 0; in2 < 75; in2++)
            {
                string lol = "" + AplhabetList[in2];
                sw1.WriteLine(lol); 
            }
            sw1.Dispose();
            return User_message; //Doesnt returnt thw correct message, works against me, mistake is in the alphabet;
        }
        public void GenerateOpenAlphabet()
        { 
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