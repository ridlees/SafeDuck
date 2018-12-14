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
        public Random rng = new Random();
        string[] Positions = System.IO.File.ReadAllLines("positions.txt"); //now I have all positions here;
        string Userinput;
        public int Positionnumber = 0;

        public Window1()

        {
            InitializeComponent();
        }
        private void Encrypt(object sender, RoutedEventArgs e)
        {
           
            Userinput = UserText.Text;
            if (Userinput.Length<251)
            {
                if(Userinput.Length==250)
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
                File.WriteAllText(@"C:\Users\root\Desktop\fungujiUserinput.txt", Userinput); //test, smazat
                char[] arr;
                arr = Userinput.ToCharArray(0, Userinput.Length);
                //is always 250 chars -->list of all positions :)
                Positionnumber = 0;
                for (int i=0; i<Userinput.Length; i++) {
                    Editpositions();
                    char Char = arr[i];
                        List<int> RGBnumbers = RGBset(Char);
                    RGBsettolister(RGBnumbers);
                    Positionnumber = Positionnumber+ 1;
                   
                }
                Editpicture(RGBsettolists, Positionlist);
                  //create list of RGBnumbers
                    //here is the issue "System.Runtime.InteropServices.ExternalException"s
                    
                    

            }
            
            else
            {
                MessageBox.Show("The input is longer than 250 chars, please, rewrite it");
            }
            MessageBox.Show("The process was succesfull");
        }

        private void Back (object sender, RoutedEventArgs e)
        {

            this.Close();
        }
        List<string> Positionlist = new List<string>();
        List<string> RGBsettolists = new List<string>();
        public List<int> RGBset(char Char)
        {
            //NOTE This method gets char from user and then changes it into three ints that will be used in the encryption phase.
            //It is rather simple method, we look for char in the Alphabet.txt file and when we find it, we used "table".
            List<int> char_set = new List<int>();
            int R = 0;
            int G = 0;
            int B = 0;
            string[] Alphabet = System.IO.File.ReadAllLines("Alphabet.txt");
            var list = Alphabet.Take(75).ToList();
                List<string> Alphastrings = new List<string>(list); ;
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
        private void RGBsettolister( List <int> RGB)
        {
            int R = RGB[0];
            int G = RGB[1];
            int B = RGB[2];
            string RGBstring = R + " " + G + " " + B;
            RGBsettolists.Insert(Positionnumber, RGBstring);
        }
        public void Editpicture (List<string> RGB, List<string> Positionslist)
        {
            StreamWriter sr = new StreamWriter("allTest.txt");
            Bitmap img = new Bitmap(System.Drawing.Image.FromFile("obrazek.png"));
            sr.WriteLine("Test" + Positionnumber);
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
                sr.WriteLine(RGBint);
                sr.WriteLine("Test");
                sr.WriteLine(Possitions);
                System.Drawing.Color c = img.GetPixel(xOrigin, yref);
                img.SetPixel(xOrigin, YOrigin, System.Drawing.Color.FromArgb(c.R - R, c.G - G, c.B - B)); //Doesn´t work, does it? 
                //TODO when picture is mostly white
            }           
            img.Save("Encrypted.png", ImageFormat.Png); // Thank you random user, here is the comment that helped me a lot https://stackoverflow.com/questions/48607238/system-runtime-interopservices-externalexception
            img.Dispose();
             

            }
        }
}




