using System;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;
//C#'da doğrudan POSIX API'lerine erişmek mümkün olmadığından, DllImport ile libc.so.6 kütüphanesine erişim sağlanarak daha efektif kodlar yazılabilir.
//bu kodda kendi process nesnemi ve kendi mesage_queue yapısını oluşturarak yazılmıştır.
namespace Ex_5_mq_takeone1
{
    class Ana
    {
        static public void Main(string[] args)
        {
            
            Mesaage_queue kuyruk = new Mesaage_queue(); // mesagge kuyruk  olusturulması 
            My_process k1 =new My_process(1); //olusturulması 
            My_process k2 =new My_process(2); //olusturulması 
            k1.StartInfo.FileName = "cmd.exe";   //calıştırılacak program 
            k1.StartInfo.UseShellExecute = false;  // shell kullanma 
            k1.StartInfo.CreateNoWindow = true; // yeni pencere oluşturmama 
            k1.StartInfo.RedirectStandardInput = true;  //standart  giriş 
            k1.StartInfo.RedirectStandardOutput = true;  //standart cıkış 
            k1.Start();  //processi başlatma 
            k1.mesage_create(2, kuyruk.count(), "k2 processine mesaj", kuyruk); // processde mesaj olusturma 

            using (StreamWriter writer = k1.StandardInput)
            {
                if (writer.BaseStream.CanWrite)
                {
                    writer.WriteLine("exit"); //process cmd'sini exit yazımı 
                }
            }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            k2.StartInfo.FileName = "cmd.exe";   //calıştırılacak program 
            k2.StartInfo.UseShellExecute = false;  // shell kullanma 
            k2.StartInfo.CreateNoWindow = true; // yeni pencere oluşturmama 
            k2.StartInfo.RedirectStandardInput = true;  //standart  giriş 
            k2.StartInfo.RedirectStandardOutput = true;  //standart cıkış 
            k2.Start();  //processi başlatma 
            k2.find_mesage(kuyruk, 2);


            using (StreamWriter writer = k2.StandardInput)
            {
                if (writer.BaseStream.CanWrite)
                {
                    writer.WriteLine("exit"); //process cmd'sini exit yazımı 
                }
            }
            k2.Close();  //processi sonlandırma 
            k1.Close();  //processi sonlandırma 
        }
    }
    class My_process : Process   // process nesnem 
    {
        public Process process = new Process();
        public int kimlik;  // ekstra olarak kimlik eklenmiştir 
        public Mesaage_Node siuu;  // processinmesaj olusturabilmesi için gerekli mesage yapısının tanımlanması 
        public My_process(int a)  //constructor 
        {
            kimlik = a;
            siuu = null;
        }
        public void mesage_create(int a ,int b , string mesageee,Mesaage_queue kuyruk)   // mesage olusturma metotu 
        {
            DateTime time = DateTime.Now;
            siuu = new Mesaage_Node(this.kimlik,a,b,mesageee,time);
            kuyruk.Add(siuu);
            Console.WriteLine("Mesage olusturuldu");
        }                                                                       
        public void find_mesage(Mesaage_queue kuyruk , int k)  // mesage arama metotu 
        {
            Mesaage_Node c = kuyruk.Pop(k);
            if(c != null)
            {
                Console.WriteLine("Mesaje bulundu"); 
                Console.WriteLine($"mesagge---->{c.mesage}");
            }
            
        }
    }
    class Mesaage_queue // bağlı liste yapısında 
    {
        const int sınır = 40;   //sınır 
        Mesaage_Node bas;
        Mesaage_Node son;
        public Mesaage_queue()
        {
            bas = null;
            son = null;
        }
        public void Add(Mesaage_Node a)  // kuyruga eleman ekleme 
        {
            if (bas == null && son == null)
            {
                bas = a;
                son = a;
                son.ileri = null;
                son.geri = bas;

            }
            //else if (this.count() == sınır )
            //{ 
            //    Console.WriteLine("Kuyruk sınırına ulast");                                  //
            //}                                                                               //gerekli algoritmalar 
            else if ((bas != null && son != null) && (bas == son))                           //
            {
                bas.ileri = a;
                son = a;
                son.geri = bas;
            }
            else
            {
                a.geri = son;
                son = a;
                son.ileri = null;
            }
        }
        public Mesaage_Node Pop(int alıcı_kimlik)    //alici kimliğine göre arama 
        {
            if (bas == null && son == null)
            {
                return null;
            }                                                                                 //                                                                                             
            else if ((bas != null && son != null) && (bas == son))                           //gerekli algoritmalar 
            {                                                                               //
                if (bas.alici_kimlik == alıcı_kimlik)
                {
                    Mesaage_Node temp = bas;
                    bas = null;
                    son = null;
                    return temp;
                }
                else
                {
                    Console.WriteLine("Aranılan mesaje kuyrukta yok");
                    return null;
                }
            }
            else
            {
                Mesaage_Node temp = bas;
                while (temp.alici_kimlik != alıcı_kimlik)
                {
                    if (temp.ileri == null)
                    {
                        Console.WriteLine("Aranılan mesaje kuyrukta yok");
                        return null;
                    }
                    temp = temp.ileri;
                }
                Mesaage_Node temp2 = temp;
                temp.geri.ileri = temp.ileri;
                return temp2;
            }
        }
        public int count()   //kuyruk eleman sayısı hesaplama 
        {
            if (bas == null && son == null)
            {
                return 0;
            }                                                                                 //                                                                                             
            else if ((bas != null && son != null) && (bas == son))                           //gerekli algoritmalar 
            {                                                                               //
                return 1;
            }
            else
            {
                Mesaage_Node temp = bas;
                int sayac = 1;
                while (temp.ileri != null)
                {
                    sayac++;
                }
                return sayac;
            }

        }       
    }
    class Mesaage_Node   // mesage yapısı
    {
        public Mesaage_Node ileri;  // ileri 
        public Mesaage_Node geri;  // geri 
        public int mesajı_olusturan_kimlik;  // mesajı olusturan kimlik 
        public int mesajı_olusturan_id;  // measjı olusturan id 
        public int alici_kimlik;  // alıcı kimlik 
        public int masaggeid;   // mesage id 
        public string mesage;  //mesage 
        public DateTime time;   //olusturulma zamanı 
        public Mesaage_Node(int a, int c, int d, string mesage, DateTime time)  // constructor 
        {
            mesajı_olusturan_kimlik = a;  
            alici_kimlik = c;
            masaggeid = d;
            this.mesage = mesage;
            this.time = time;
            ileri = null;
            geri = null;
        }
    }
}