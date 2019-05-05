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
        private static List<string> lista = new List<string>();  // lista di supporto
        static SerialPort porta1 = new SerialPort("/dev/usb0", 9600); // porta seriale dove arduino è collegato
        static string valore = ""; //variabile condivisa
        

        static void Main(string[] args)
        {
            Thread leggi = new Thread(new ThreadStart(LeggiPorta)); // primo thread
            Thread scrivi = new Thread(new ThreadStart(Scrivi));    // secondo thread
            
            leggi.Start(); // start primo thread
            scrivi.Start(); // start secondo thread

            gestisciFile();  // gestione finale del file dove viene scritto in un file

            Console.ReadKey();
        }

        static void LeggiPorta() // legge la porta dell'arduino
        {
            porta1.Open(); // porta aperta
            string stringa = porta1.ReadExisting(); // salvo su stringa valore letto

            lock (valore) {
                valore = stringa;
            }

            Thread.Sleep(1500); // aspetto 1,5 secondi
        }

        static void Scrivi() //riempie una lista
        {
          
            lock (valore)
            {
                string str = valore;  // valore corrisponde a porta1.ReadExisting()
                try
                {
                    //string str = porta1.ReadExisting(); 
                    Console.Write(str); // scrive sulla console i valori
                    lista.Add(str); // aggiunge i valori alla lista con Add
                }
                catch (Exception) { }
                
            }

            Thread.Sleep(1500); // aspetta 1,5 secondi
        }

        public static void gestisciFile() // scrive su file
        {
            try
            {
                //creo il file
                StreamWriter sw = new StreamWriter("C:\\Users\\andre_000\\Desktop\\text.txt");
                int tmp;
                foreach(string i in lista) //scorro la lista
                {
                    tmp = Convert.ToInt32(i); //variabile di supporto
                   
                    if( tmp > 550) // se il valore della fotoresistenza è maggiore di 550 il led resta spento sennò si accende
                    {
                        sw.Write("Periodo: giorno,  " +  "luce:  " + i + ", luce giardino: spenta");
                    } else
                    {
                        sw.Write("Periodo: notte,  " + "luce:  " + i + ", luce giardino: accesa");
                    }
                    sw.Write(i); //scrivo su file
                }
        
                //chiudo il file
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

