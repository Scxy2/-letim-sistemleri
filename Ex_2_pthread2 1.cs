using System;
using System.Threading;
class Program
{
    // Hello World mesajları için diziler
    static string[] messages = { 
         "İngilizce: Merhaba Dünya!",
         "Fransızca: Bonjour, le monde!",
         "İspanyolca: Hola mundo!",
         "Klingonca: Nuq neH!",
         "Almanca: Guten Tag, Welt!" 
    };
    // Bir thread'e iletilen argüman değerlerinin yapısı
    class ThreadData
    {
        public int thread_id; // Thread'in kimliği
        public int sum; // Toplam değer
        public string message; // Mesaj
        public ThreadData(int a, int b, string c) // Yapıyı başlatan kurucu metot
        {
            thread_id = a;
            sum = b;
            message = c;
        }
    }
    static void PrintHello(object k)
    {
        ThreadData my_data = (ThreadData)k; // Gelen veriyi ThreadData türüne dönüştür
        int id = my_data.thread_id; // Thread kimliğini al
        int sum = my_data.sum; // Toplam değeri al
        string hello_msg = my_data.message; // Mesajı al
        Thread.Sleep(500); // 0.5 saniye bekleme
        Console.WriteLine("Thread {0}: {1}  Toplam={2}", id, hello_msg, sum); // Mesajı ve toplam değeri ekrana yazdır
    }
    static void Main(string[] args)
    {
        Thread[] threads = new Thread[5]; // Thread dizisi oluştur
        int sum = 0;
        for (int i = 0; i < 5; i++)
        {
            // Bir thread için argümanları başlat
            sum += i; // Toplam değeri güncelle
            ThreadData data = new ThreadData(i, sum, messages[i]); // Yeni bir ThreadData nesnesi oluştur
            threads[i] = new Thread(PrintHello); // Bir thread oluştur
            threads[i].Start(data); // Oluşturulan thread'i başlat ve veri ile birlikte gönder
        }
        // Ana thread'in diğer thread'leri beklemesi
        foreach (Thread t in threads)
        {
            t.Join();
        }
    }
}
