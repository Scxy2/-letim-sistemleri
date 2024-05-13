using System;
using System.Threading;
class Program
{
    // Toplam iş parçacığı sayısı
    const int thread_num = 5;

    // Farklı dillerdeki mesajlar
    static string[] messages = {
        "English: Hello World!",
        "French: Bonjour, le monde!",
        "Spanish: Hola al mundo",
        "Klingon: Nuq neH!",
        "German: Guten Tag, Welt!"
    };

    // Her bir iş parçacığı tarafından çalıştırılacak metot
    static void PrintHello(object id)
    {
        int threadId = (int)id;
        Thread.Sleep(500); // Her bir iş parçacığının 0,5 saniye uyumasını sağlar
        Console.WriteLine($"Thread {threadId}: {messages[threadId]}");
    }

    static void Main(string[] args)
    {
        // İş parçacıklarını depolamak için dizi
        Thread[] threads = new Thread[thread_num];

        // Her bir iş parçacığı oluşturulur ve başlatılır
        for (int i = 0; i < thread_num; i++)
        {
            int id = i;
            // Her bir iş parçacığına bir thread atanır ve başlatılır
            threads[i] = new Thread(() => PrintHello(id));
            threads[i].Start();
        }

        // Tüm iş parçacıklarının tamamlanmasını bekler
        foreach (Thread t in threads)
        {
            t.Join();
        }
    }
}