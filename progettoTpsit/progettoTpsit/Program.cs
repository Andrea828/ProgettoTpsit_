using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;

namespace progettoTpsit
{

    class Program
    {
        private static List<string> lista = new List<string>();
        static SerialPort porta1 = new SerialPort("/dev/usb0", 9600); // porta seriale dove arduino è collegato
        static string valore = ""; 
        

        static void Main(string[] args)
        {
            Thread leggi = new Thread(new ThreadStart(LeggiPorta));
            Thread scrivi = new Thread(new ThreadStart(Scrivi));
            
            leggi.Start();
            scrivi.Start();

            gestisciFile();

            Console.ReadKey();
        }

        static void LeggiPorta() 
        {
            porta1.Open();
            string stringa = porta1.ReadExisting();

            lock (valore) {
                valore = stringa;
            }

            Thread.Sleep(1500); 
        }

        static void Scrivi() //riempie una lista
        {
          
            lock (valore)
            {
                string str = valore;  // valore corrisponde a porta1.ReadExisting()
                try
                {
                    //string str = porta1.ReadExisting(); 
                    Console.Write(str);
                    lista.Add(str);
                }
                catch (Exception) { }
                
            }

            Thread.Sleep(1500);
        }

        public static void gestisciFile()
        {
            try
            {

                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter("C:\\Users\\andre_000\\Desktop\\text.txt");
                int tmp;
                foreach(string i in lista)
                {
                    tmp = Convert.ToInt32(i);
                    //Write a line of text
                    if( tmp > 550)
                    {
                        sw.Write("Periodo: giorno,  " +  "luce:  " + i + ", luce giardino: spenta");
                    } else
                    {
                        sw.Write("Periodo: notte,  " + "luce:  " + i + ", luce giardino: accesa");
                    }
                    sw.Write(i);
                }
        
                //Close the file
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("fine");
            }
        }




    }
}

