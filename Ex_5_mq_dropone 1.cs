using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml.Serialization;
//System.Messaging sanırım .Net Core ve .Net 5.0 ve üstünde desteklenmemektedir. 
//Alternatif framworkler RabbitMQ , Apache Kafka , Azure Service Bus 
//bu kodda kendi process nesnesimi ve kuyruk yapımı kullanarak processlern işlem onceliğine gore kuyrugu yapılandırıyorum 
namespace program
{
    class Ana 
    {      
        static void Main(string[] args)
        {
            Queue is_kuyrugu = new Queue(); //kendi kuyruk nesnem 
            Random rnd = new Random(); // random obje olusturma 
            int process_sayısı=rnd.Next(1,41); // random process sayısı belirleme 
            for (int i=0;i<process_sayısı;i++)     //process kuyruga ekleme 
            {
                My_process prc = new My_process(rnd.Next(0,4));  //random process onceliği ile process olusturma  
                Node k = new Node(prc,i); // node nesneleri 
                is_kuyrugu.Add(k);  // kuyruga ekleme 
            }
            is_kuyrugu.Işlem_planlayıcı();  // job schueler  (kuyruktaki processleri işlem onceliğine gore yeniden düzenler ) 
                Console.WriteLine($"    ID            Kuyruk sirasi         onceligi              Islem sirasi                  Time"); // tablo başlıkları 
            for (int i = 0;i<process_sayısı; i++)
            {
                int[] a = new int[2];  // process id ve cıkıs degerini tutmak için dizi 
                Node k = is_kuyrugu.Pop(); //kuyruktaki bir processi calıstırma 
                DateTime currentTime = DateTime.Now; // zaman nesnesi 
                k.proces.StartInfo.FileName = "cmd.exe";   //calıştırılacak program 
                k.proces.StartInfo.Arguments = $"/c exit {i}";  //argumanlar // burada cıkıs kodunu ayarlıyoruz 
                k.proces.StartInfo.UseShellExecute = false;  // shell kullanma 
                k.proces.StartInfo.CreateNoWindow = true; // yeni pencere oluşturmama 
                k.proces.StartInfo.RedirectStandardInput = true;  //standart  giriş 
                k.proces.StartInfo.RedirectStandardOutput = true;  //standart cıkış 
                k.proces.Start();  //processi başlatma 
                a[0] = k.proces.Id; // process idsini alma 
                
                using (StreamWriter writer = k.proces.StandardInput)
                {
                    if (writer.BaseStream.CanWrite)
                    {
                        writer.WriteLine("exit"); //process cmd'sini exit yazımı 
                    }
                }
                k.proces.WaitForExit();  // wait fonksiyonu----> processin işini yapmasına kadar bekler 
                a[1] = k.proces.ExitCode;  //exit code alımı 
                Console.WriteLine($"Ben ({k.proces.Id}) ,kuyruga alınma sıram {k.sıra} , onceligim {k.proces.oncelik} , öncelik olarak {i} kullanacağım.---->{currentTime}");
                k.proces.Close();  //processi sonlandırma 
            }
        }
    }
    public class My_process : Process  // kendi process nesnem 
    {
        public Process işlem; // process 
        public int oncelik; // islem onceliği 
        public My_process(int a) // yapıcı metotu 
        {
            işlem = new Process();
            oncelik = a;
        }
    }
    class Queue  // kuyruk yapısı
    {
        static int max_queue = 40; // max kuyruk eleman sayısı 
        private Node bas = null;
        private Node son = null;
        public void Add(Node p) // kuyruga eleman ekleme 
        {
            if(bas == null && son==null)
            {
                bas = p;
                son = p;
                son.ileri = null;
            }
            else if (this.count() == 40)
            {
                Console.WriteLine("Kuyruk sınırına ulast");         //
            }                                                       //gerekli algoritmalar 
            else if((bas!= null && son!=null)&&(bas==son))          //
            {
                bas.ileri = p;
                son = p;
                son.ileri = null;
            }
            else
            {
                son.ileri = p; 
                son=p;
                son.ileri = null;
            }
        }
        public Node Pop()    // kuyruktan eleman cıkarma 
        {
            if (bas == null && son == null)
            {
                return null;
            }
            else if ((bas != null && son != null) && (bas == son))
            {
                Node temp = bas;                                       //
                bas = null;                                            //gerekli algoritmalar 
                son = null;                                            //
                return temp;
            }
            else
            {
                Node temp = bas;
                bas = bas.ileri;
                return temp;
            }
        }

        public int count()  // kuyruk eleman sayısı hesaplama 
        {
            if (bas == null && son == null)
            {
                return 0;
            }
            else if ((bas != null && son != null) && (bas == son))              //
            {                                                                  //gerekli algoritmalar 
                return 1;                                                      //
            }
            else
            {
                int sayac = 1; 
                Node temp = bas;
                while (temp.ileri != null)
                {
                    sayac ++;   
                    temp=temp.ileri;
                }
                return sayac;
            }
        }
        public void Işlem_planlayıcı() // processlerin öncelik numarasına göre sıralar 0,1,2,3 sırayla en önemli (Bubble sort) 
        {
            if (bas == null && son == null)
            {
                //eleman yok 
            }
            else if ((bas != null && son != null) && (bas == son))
            {
                //1 elemanı var 
            }
            else
            {
                int sayi= this.count();
                Node[] node_dizi=new Node[sayi];
                for(int i = 0; i < sayi; i++)
                {
                    node_dizi[i]=this.Pop();                                
                }
                bas = null;                                             
                son = null;
                for (int i = 0; i < sayi - 1; i++)    // bubble sort 
                {
                    for (int j = 0; j < sayi - i - 1; j++)
                    {
                        if (node_dizi[j].proces.oncelik > node_dizi[j + 1].proces.oncelik)
                        {
                            // yer değiştirme
                            Node siuu = node_dizi[j];
                            node_dizi[j] = node_dizi[j + 1];
                            node_dizi[j + 1] = siuu;
                        }
                    }
                }
                for(int i = 0;i < sayi; i++)  // kuyruk yeniden yapılandırılır 
                {
                    this.Add(node_dizi[i]);
                }

            }
        }      
    }
    class Node       // düğüm yapısı 
    {
        public My_process proces;   
        public Node ileri;
        public int sıra;
        public Node(My_process l,int a)
        {
            proces = l;
            sıra = a;
            ileri = null;
        }
    }
}