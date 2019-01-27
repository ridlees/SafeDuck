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
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace WpfApp1
{
    /// <summary>
    /// Interakční logika pro Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        int Saved = 0;
        int changedText= 0;
        public Random rng = new Random();
        string[] Positions = System.IO.File.ReadAllLines("positions.txt"); //now I have all positions here;
        string Userinput;
        public int Positionnumber = 0;

        public Window1()

        {
            InitializeComponent();
            Number_of_chars.Content = "You are fine!";

        }
        private void Encrypt(object sender, RoutedEventArgs e)
        {
            Userinput = UserText.Text;
            if (Userinput.Length < 251)
            {
                if (Userinput.Length == 250)
                {
                    //Nothing :)
                }
                else
                {
                    for (int n = 250 - Userinput.Length; n > 0; n = n - 1)
                    {
                        Userinput = Userinput + ']';
                    }

                }
                char[] arr;
                arr = Userinput.ToCharArray(0, Userinput.Length);
                //is always 250 chars -->list of all positions :)
                Positionnumber = 0;
                for (int i = 0; i < Userinput.Length; i++)
                {
                    Editpositions();
                    char Char = arr[i];
                    List<int> RGBnumbers = RGBset(Char,0);
                    RGBsettolister(RGBnumbers);
                    Positionnumber = Positionnumber + 1;

                }
                Editpicture(RGBsettolists, Positionlist);
                //create list of RGBnumbers
            }
            else
            {
                MessageBox.Show("The input is longer than 250 chars, please, rewrite it","Input is too large for me");
            }
            if (Saved==1)
            {
                MessageBox.Show("The process was succesfull", "Hurray, your words are now secret");
            }
            else
            {
                MessageBox.Show("Sorry, please, use the encrypt button again to save your picture","You cancelled your save :(");
            }
        }
        private void Back(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        List<string> Positionlist = new List<string>();
        List<string> RGBsettolists = new List<string>();
        public List<int> RGBset(char Char, int ISExperimental)
        {
            //NOTE This method gets char from user and then changes it into three ints that will be used in the encryption phase.
            //It is rather simple method, we look for char in the Alphabet.txt file and when we find it, we used "table".
            List<int> char_set = new List<int>();
            int R = 0;
            int G = 0;
            int B = 0;
            string[] Alphabet;
            if (ISExperimental == 1)
            {
                Alphabet = System.IO.File.ReadAllLines("Aplhabet_open.txt");
            }
            else
            {
                Alphabet = System.IO.File.ReadAllLines("Alphabet.txt");
            }
            var list = Alphabet.Take(75).ToList();
            List<string> Alphastrings = new List<string>(list); 
            if (Char == ']')
            {
                string stringchar;
                int RandomHash = rng.Next(8);
                RandomHash = RandomHash + 67;
                stringchar = Alphastrings[RandomHash];
                string[] Poses = stringchar.Split('/');
                char[] RGBnumber;
                RGBnumber = Poses[1].ToCharArray(0, Poses[1].Length);
                R = RGBnumber[0] - '0';
                G = RGBnumber[1] - '0';
                B = RGBnumber[2] - '0';
                char_set.Add(R);
                char_set.Add(G);
                char_set.Add(B);
                return char_set;
            }
            else
            {
                for (int i = 0; i < 68; i++)
                {
                    string stringchar = Alphastrings[i];
                    string[] Poses = stringchar.Split('/');
                    char[] arr;
                    arr = Poses[0].ToCharArray();
                    char Alpha = arr[0];
                    if (Char == Alpha)
                    {
                        char[] RGBnumber;
                        RGBnumber = Poses[1].ToCharArray(0, Poses[1].Length);
                        R = RGBnumber[0] - '0';
                        G = RGBnumber[1] - '0';
                        B = RGBnumber[2] - '0';
                    }
                }
                char_set.Add(R);
                char_set.Add(G);
                char_set.Add(B);
                return char_set; //encrypting the char from user input <string> and then sets in in the picture.
            }
        }
        private void Editpositions()
        {
            string line = Positions[Positionnumber];
            string[] Poses = line.Split(' ');
            int xOrigin = Int32.Parse(Poses[0]);
            int YOrigin = Int32.Parse(Poses[1]);
            int Ref = Int32.Parse(Poses[2]);
            int yref;
            if (Ref == 1)
            {
                yref = YOrigin - 1;
            }
            else
            {
                yref = YOrigin + 1;
            }
            line = xOrigin + " " + YOrigin + " " + yref;
            Positionlist.Insert(Positionnumber, line);
        }
        private void RGBsettolister(List<int> RGB)
        {
            int R = RGB[0];
            int G = RGB[1];
            int B = RGB[2];
            string RGBstring = R + " " + G + " " + B;
            RGBsettolists.Insert(Positionnumber, RGBstring);
        }
        public void Editpicture(List<string> RGB, List<string> Positionslist)
        {

            System.Drawing.Image imgur = System.Drawing.Image.FromFile("obrazek.png");
            Bitmap img = new Bitmap(imgur);
            imgur.Dispose();
                    
            for (int Positionnumb = 0; Positionnumb < Positionnumber; Positionnumb++)
            {
                string RGBint = RGB[Positionnumb];
                string Possitions = Positionslist[Positionnumb];
                string[] RGBposes = RGBint.Split(' ');
                string[] Possitionposes = Possitions.Split(' ');
                int xOrigin = Int32.Parse(Possitionposes[0]);
                int YOrigin = Int32.Parse(Possitionposes[1]);
                int yref = Int32.Parse(Possitionposes[2]);
                int R = Int32.Parse(RGBposes[0]);
                int G = Int32.Parse(RGBposes[1]);
                int B = Int32.Parse(RGBposes[2]); 
                System.Drawing.Color c = img.GetPixel(xOrigin, yref);
                int Red     = c.R - R;
                int Green   = c.G - G;
                int Blue    = c.B - B;
                int RGBError = 0;
                if (Red <0) { Red = c.R + R; RGBError++; }
                if (Blue <0) { Blue = c.B + B; RGBError++; }
                if (Green <0) { Green = c.G + G; RGBError++; }

                img.SetPixel(xOrigin, YOrigin, System.Drawing.Color.FromArgb(Red, Green, Blue)); 
                            }
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "encrypted"; // Default file name
            dlg.DefaultExt = ".png"; // Default file extension
            dlg.Filter = "PNG files (.png)|*.png"; // Filter files by extension 
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                img.Save(filename, ImageFormat.Png);
                img.Dispose();
                Saved = 1;
            }
        // Thank you random user, here is the comment that helped me a lot https://stackoverflow.com/questions/48607238/system-runtime-interopservices-externalexception
        else
        {
             MessageBox.Show("I am sorry, but you didn´t choose the location", "Save-Error");
             Saved = 0;
        }
    }

        private List<string> passphrase2passlist(string password)
        {
            List<string> Passlist = new List<string>();
            char[] arr;
            arr = password.ToCharArray(0, password.Length);          
            for (int i = 0; i < password.Length; i++)
            {
                char Char = arr[i];
                List<int> RGBnumbers = RGBset(Char, 1);
                string passline = RGBnumbers[0] + " " + RGBnumbers[1] + " " + RGBnumbers[2];
                Passlist.Insert(i, passline);
            }
            return Passlist;
        }
        public List<string> AplhabetList = new List<string>();
        public void GenerateOpenAlphabet()
        {
            string ABC = "A/a/B/b/C/c/D/d/E/e/F/f/G/g/H/h/I/i/J/j/K/k/L/l/M/m/N/n/O/o/P/p/Q/q/R/r/S/s/T/t/U/u/V/v/W/w/X/x/Y/y/Z/z/0/1/2/3/4/5/6/7/8/9/@/./-/ /_/]";
            string[] SymbolOpen = ABC.Split('/');
            int open_position_symbol = 0;
            StreamWriter sw = new StreamWriter("Aplhabet_open.txt");
            string number="";
            for (int symbol = 0; symbol < 5; symbol++)
            {
                for (int symbole = 0; symbole < 3; symbole++)
                {
                    for (int symbola = 0; symbola < 5; symbola++)
                    {
                        if(open_position_symbol < SymbolOpen.Count())
                        {
                            number = SymbolOpen[open_position_symbol] + "/" + symbol  + symbole  + symbola;
                        }
                        else if (open_position_symbol >= SymbolOpen.Count())
                        {
                            number = "]" + "/" + symbol   + symbole + symbola;
                            
                        }
                        AplhabetList.Insert(open_position_symbol, number);
                        sw.WriteLine(number);
                        open_position_symbol++;
                    }
                }
            }
            sw.Close();
        }

        private void Encrypt_full_button(object sender, RoutedEventArgs e)
    {
            GenerateOpenAlphabet();
            Window3 password = new Window3();          
            bool r = (bool)password.ShowDialog();
            if (r == true)
            {
                string passphrase = password.password.Text;
                for (int m = 10 - passphrase.Length; m > 0; m = m - 1)
                {
                    passphrase = passphrase + ']';
                }
                List<string> Passlist = new List<string>();
                Passlist = passphrase2passlist(passphrase);

                Userinput = UserText.Text;
                if (Userinput.Length < 251)
                {
                    if (Userinput.Length == 250)
                    {
                        //Nothing :)
                    }
                    else
                    {
                        for (int n = 250 - Userinput.Length; n > 0; n = n - 1)
                        {
                            Userinput = Userinput + ']';
                        }

                    }
                    char[] arr;
                    arr = Userinput.ToCharArray(0, Userinput.Length);
                    //is always 250 chars -->list of all positions :)
                    Positionnumber = 0;
                    for (int i = 0; i < Userinput.Length; i++)
                    {
                        char Char = arr[i];
                        List<int> RGBnumbers = RGBset(Char, 0);
                        RGBsettolister(RGBnumbers);
                        Positionnumber = Positionnumber + 1;
                    }
                    Editpicture_experimental(RGBsettolists, Passlist);
                    //create list of RGBnumbers
                }
                else
                {
                    MessageBox.Show("The input is longer than 250 chars, please, rewrite it", "Input is too large for me");
                }
                if (Saved == 1)
                {
                    MessageBox.Show("The process was succesfull", "Hurray, your words are now secret");
                    File.Delete("Aplhabet_open.txt");
                }
                else
                {
                    MessageBox.Show("Sorry, please, use the encrypt button again to save your picture", "You cancelled your save :(");
                }
            }
            else
            {
                MessageBox.Show("I am sorry, please, write your password and hit enter to proceed", "Password-input error");
            }    
    }
        public System.Drawing.Bitmap Clearline (System.Drawing.Bitmap img, List<string> Passwords)
        {  //
            for(int passwordnumber = 0; passwordnumber < 10 ; passwordnumber++) {
                string Passw = Passwords[passwordnumber];
                string[] PassInt = Passw.Split(' ');
                int R = Int32.Parse(PassInt[0]);
                int G = Int32.Parse(PassInt[1]);
                int B = Int32.Parse(PassInt[2]);
                for (int y = 0; y < 4; y++)
            { 
                for (int x = 0; x < img.Width ;x++)
                {
                        System.Drawing.Color c = img.GetPixel(x, y);
                        System.Drawing.Color cREF = img.GetPixel(x, y + 1);
                        int Red = c.R - cREF.R;
                        int Green = c.G - cREF.G;
                        int Blue = c.B - cREF.B;
                        if (Red < 0) { Red = cREF.R - c.R; }
                        if (Blue < 0) { Blue = cREF.B - c.B; }
                        if (Green < 0) { Green = cREF.G - c.G; }
                        if (Red == R && Green == G && Blue == B)
                        {
                            int cG = c.G + 4;
                            if (cG >255) { cG = c.G - 4; }
                            
                            img.SetPixel(x, y, System.Drawing.Color.FromArgb(c.R, cG, c.B)); 
                        }
                        //doesnt work
                    }
            }

            }
            return img;
        }
        public void Editpicture_experimental(List<string> RGB, List<string> Passwords)
        {

            System.Drawing.Image imgur = System.Drawing.Image.FromFile("obrazek.png");
            Bitmap img = new Bitmap(imgur);
            imgur.Dispose();
            int x = 0;
            int y = 0;
            List<int> Jumps = new List<int>();
            img = Clearline(img, Passwords);
            for (int Passpheres = 0; Passpheres < 10; Passpheres++)
            {
                int next=rng.Next(img.Width / 10) + 1;
                string Passw = Passwords[Passpheres];
                string[] PassInt = Passw.Split(' ');
                int R = Int32.Parse(PassInt[0]);
                int G = Int32.Parse(PassInt[1]);
                int B = Int32.Parse(PassInt[2]);
                x = x + next;
                if (x > img.Width)
                {
                    y++;
                    y++;
                    x = x - img.Width;
                }
                Jumps.Insert(Passpheres,next);
                System.Drawing.Color c = img.GetPixel(x, y+1);
                int Red = c.R - R;
                int Green = c.G - G;
                int Blue = c.B - B;
                if (Red < 0) { Red = c.R + R;  }
                if (Blue < 0) { Blue = c.B + B;  }
                if (Green < 0) { Green = c.G + G; }
                img.SetPixel(x, y, System.Drawing.Color.FromArgb(Red, Green, Blue));
            }
            int yalphabet = 0;
            int numberofyalpha = 0;
            if (3 * Jumps[0] > 2 * Jumps[5])
            {
                 yalphabet = 3 * Jumps[0] - 2 * Jumps[5];
            }
            else
            {
                 yalphabet = 2 * Jumps[5] - 3 * Jumps[0];
            }
            int XAplha=0;
            int firstyalphabet = yalphabet;
            string[] alphafromfile = System.IO.File.ReadAllLines("Alphabet.txt"); //TODO LIst
            var list = alphafromfile.Take(75).ToList();
            List<string> Alphastringsfromfile = new List<string>(list);

            for (int k = 0; k < Alphastringsfromfile.Count();k++)
            {
                string Aplhabess = Alphastringsfromfile[k];
                string[] AphaInt = Aplhabess.Split('/');
                char[] RGBarray = AphaInt[1].ToCharArray(0, AphaInt[1].Length); 
                int R = RGBarray[0] - '0';
                int G = RGBarray[1] - '0';
                int B = RGBarray[2] - '0';
                if (XAplha > img.Width)
                {
                    yalphabet = yalphabet + 2;
                    numberofyalpha++;
                    XAplha = 0;
                } //todo change the 424 line to work with more aplhabet lines, i think I did
                System.Drawing.Color c = img.GetPixel(XAplha, yalphabet + 1);
                int Red = c.R - R;
                int Green = c.G - G;
                int Blue = c.B - B;
                if (Red < 0) { Red = c.R + R; }
                if (Blue < 0) { Blue = c.B + B; }
                if (Green < 0) { Green = c.G + G; }
                img.SetPixel(XAplha, yalphabet, System.Drawing.Color.FromArgb(Red, Green, Blue));
                XAplha++;
            }
            y = y++;
            int jumpNumber = 0;
            //test
            StreamWriter sw1 = new StreamWriter("encryptjumps.txt");
            for (int i2 = 0; i2 < 9; i2++)
            {
                string lol = "" + Jumps[i2];
                sw1.WriteLine(lol);
            }
            sw1.Dispose();
            ///test
            for (int Charnumb = 0; Charnumb <RGB.Count() ; Charnumb++)
            {
                string RGBint = RGB[Charnumb];
                string[] RGBposes = RGBint.Split(' ');
                int R = Int32.Parse(RGBposes[0]);
                int G = Int32.Parse(RGBposes[1]);
                int B = Int32.Parse(RGBposes[2]);
                x = x + Jumps[jumpNumber];
                jumpNumber++;
                if (jumpNumber == 10)
                {
                    jumpNumber = 0;
                }
                if (x >= img.Width)
                {
                    y++;
                    y++;
                    x = x - img.Width;
                }
                
                if (numberofyalpha != 0)
                {
                    if (y == firstyalphabet && y == firstyalphabet - 1)
                    {
                        y = y + 3*numberofyalpha;
                    }
                }
                if (numberofyalpha == 0)
                {
                    if (y == yalphabet && y == yalphabet - 1)
                    {
                        y = y + 3;
                    }
                }
                if (y + 1 > img.Height && y + 1 == img.Height)
                {
                    y = 3;
                }
                System.Drawing.Color c = img.GetPixel(x, y+1);
                int Red = c.R - R;
                int Green = c.G - G;
                int Blue = c.B - B;
                if (Red < 0) { Red = c.R + R; }
                if (Blue < 0) { Blue = c.B + B; }
                if (Green < 0) { Green = c.G + G; }
                img.SetPixel(x, y, System.Drawing.Color.FromArgb(Red, Green, Blue));           
            }
            
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "encrypted"; // Default file name
            dlg.DefaultExt = ".png"; // Default file extension
            dlg.Filter = "PNG files (.png)|*.png"; // Filter files by extension 
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                if (File.Exists(filename) == true) { File.Delete(filename); }
                img.Save(filename, ImageFormat.Png);
                img.Dispose();
                Saved = 1;
            }
            // Thank you random user, here is the comment that helped me a lot https://stackoverflow.com/questions/48607238/system-runtime-interopservices-externalexception
            else
            {
                MessageBox.Show("I am sorry, but you didn´t choose the location", "Save-Error");
                Saved = 0;
            }
        }

        private void UserText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (changedText == 1)
            {
                
                Number();

            }

            if (UserText.Text.Length > 1)
            {
                changedText = 1;
      
            }
        else if (UserText.Text.Length == 0)
            {
                changedText =0;
                Number_of_chars.Content = "Please, write something";
            }
            
        }
        private void Number()
        {
            changedText = 0;
            int Writen = UserText.Text.Length;
            Number_of_chars.Content = "You used "+Writen+" out of 250";
        }

    }
}




