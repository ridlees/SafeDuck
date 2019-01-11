﻿using System.Windows;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Media.Imaging;
namespace WpfApp1
{    
    /// <summary>
    /// Interakční logika pro Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        private int slide=0;
        public string Namer ="saveDuckIntro";
        public Window2()
        {          
            InitializeComponent();
            //TODO vyuzít user_control k tomu, aby šlo projit dynamicky měnit content okna
        }
        private void OK(object sender, RoutedEventArgs e)
        {
            string Albhabet = "Alphabet.txt";
            string Position = "positions.txt";
            if (File.Exists(Albhabet) == true && File.Exists(Position) == true)
            {
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("I dont see my keys. Please, go through first run guide.");
            }
        }
        private void First_run(object sender, RoutedEventArgs e)
        {
            CreateEncryptkeys();
        }
        public void CreateEncryptkeys()
        {
            if(ABCNumbersExist()==false)
            {
                GenerateABC();
                GenerateNumbers();
        }
            else
            {
                MessageBox.Show("ABC and Numbers exist");
            }
            CreateEncrypttables();
        }
        public void CreateEncrypttables()
        {
            if (Alphabet() == false)
            {
                CreateEncryptAlphabet();
                CreateEncryptPosition();
                MessageBox.Show("Keys have been generated, don´t loose them.");
            }
            else
            {
                MessageBox.Show("Aphabet and position exists");
            }
        }
        private bool Alphabet()
        {
            string Albhabet = "Alphabet.txt";
            string Position = "positions.txt";
            if (File.Exists(Albhabet) == true && File.Exists(Position) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void CreateEncryptPosition()
        {   


            StreamWriter sw = new StreamWriter("positions.txt");
            int x;
            int y;
            int z;
            List<int> allX = new List<int>();
            List<int> allY = new List<int>();
            for (int t=0; t<250;t++)
            {   //worst dimensions I have found on the picture is 640x480 //TODO add //more position lists for other sizes (100x100)/(250x250)/(640x480)/other... up to full HD
                x = rng.Next(640);
                y = rng.Next(480);
                while(x == 0 || y == 0)
                {
                    x = rng.Next(640);
                    y = rng.Next(480);
                }
                while (allX.Contains(x) && allY.Contains(y))
                {
                    x = rng.Next(640);
                    y = rng.Next(480);
                }
                allX.Add(x);
                allY.Add(y);
                z = rng.Next(2);
                string line = x + " " + y + " " + z;
            sw.WriteLine(line);
            }
            sw.Close();
        }
        public Random rng = new Random();
        public void CreateEncryptAlphabet()
        {
            string[] ABClines = System.IO.File.ReadAllLines("ABC.txt");
            string[] Number = System.IO.File.ReadAllLines("Numbers.txt");
            var list = Number.Take(76).ToList();
            var list2 = ABClines.Take(68).ToList();
            List<string> listABC = new List<string>(list2);
            List<string> listNumber = new List<string>(list); //place where all numbers will be stored
            StreamWriter sw = new StreamWriter("Alphabet.txt");
            int l = listNumber.Count();
            foreach (var ABC in listABC)
            {
                int i = rng.Next(l);
                string Numbers = listNumber[i];
                l = l - 1;
                //check if the l is inside of the list //not sure if i need thought
                string line = ABC + '/' + Numbers;
                sw.WriteLine(line);
                listNumber.RemoveAt(i);
            }
            string Hash = ABClines[67];
            foreach(var lastnumbers in listNumber)
            {
                string line = Hash + '/' + lastnumbers;
                sw.WriteLine(line);
            }
            sw.Close(); 
        }
        public bool ABCNumbersExist()
        {
            string ABC = "ABC.txt";
            string Numbers = "Numbers.txt";
           if(File.Exists(ABC)== true && File.Exists(Numbers)==true) {
                return true;
        }
           else
            {
                return false;
            }
        }
        public void GenerateABC()
        {
            string[] letters = ABC.Split('/');
        GenerateNumbers();
        StreamWriter sw = new StreamWriter("ABC.txt");
            foreach (var letter in letters)
            {
                sw.WriteLine(letter);
            }
            sw.Close();
        }
        public void GenerateNumbers()
        {
            StreamWriter sw = new StreamWriter("Numbers.txt");
            string number;
            for (int symbol=0;symbol < 5; symbol++)
            {
                for(int symbole = 0; symbole < 3; symbole++)
                {
                    for (int symbola = 0; symbola < 5; symbola++)
                    {
                        number =""+ symbol + symbole + symbola;
                        sw.WriteLine(number);
                    }
                }
            }
            sw.Close();
        }
        public string ABC = "A/a/B/b/C/c/D/d/E/e/F/f/G/g/H/h/I/i/J/j/K/k/L/l/M/m/N/n/O/o/P/p/Q/q/R/r/S/s/T/t/U/u/V/v/W/w/X/x/Y/y/Z/z/0/1/2/3/4/5/6/7/8/9/@/./-/ /_/]";
        private void Button_Next(object sender, RoutedEventArgs e)
        {
            slide = slide + 1;
            Refresher();
        }
        private void Button_Back(object sender, RoutedEventArgs e)
        {
            slide = slide - 1;
            Refresher();
        }
        private void Refresher()
        {
            if(slide > 0)
            {
                Back.Visibility = Visibility.Visible;
            }
            if(slide == 0)
            {
                Back.Visibility = Visibility.Hidden;
            }
            if(slide==4)
            {
                Next.Visibility = Visibility.Hidden;
                First.Visibility = Visibility.Visible;
            }
            if (slide < 4)
            {
                Next.Visibility = Visibility.Visible;
            }
            //string ImageSource = Namer + slide+".png"; does work, but consumes too much RAM power;
            //dynamic content will be here
            //public string ABC= "a/á/b/c/č/d/ď/e/é/ě/f/g/h/ch/i/í/j/k/l/m/n/ň/o/ó/p/q/r/ř/s/š/t/ť/u/ú/ů/v/w/x/y/ý/z/ž/A/Á/B/C/Č/D/Ď/E/É/Ě/F/G/H/CH/I/Í/J/K/L/M/N/Ň/O/Ó/P/Q/R/Ř/S/Š/T/Ť/U/Ú/Ů/V/W/X/Y/Ý/Z/Ž/0/1/2/3/4/5/6/7/8/9/@/./-/ /_/]";
            //-> doesn´t work, because I was not able to install Unif8 :/
            //110 symbols of the ABC --> old version
            //125 numbers
        }
    }
}