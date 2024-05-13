using System;
using System.Threading;
class Program
{
    // Sabitlerin tanımlanması
    static readonly int num_thread = 4; // İş parçacığı sayısı
    static readonly int iterasyon = 1000000; // Her iş parçacığı için döngü iterasyon sayısı

    // İş parçacığı fonksiyonu: Her iş parçacığı için yürütülecek işi temsil eder
    static void BusyWork(object threadId)
    {
        long tid = (long)threadId; // İş parçacığı kimliği
        double result = 0.0; // Sonuç

        // İş parçacığı başlangıcı için konsola çıktı
        Console.WriteLine($"Thread {tid} başlıyor...");

        // Rastgele sayı üretmek için Random nesnesi oluşturulması
        Random rand = new Random();

        // Belirlenen iterasyon sayısı kadar döngü
        for (int i = 0; i < iterasyon; i++)
        {
            // Rastgele sayı üretilip sonuca eklenmesi
            result += (double)rand.Next() / int.MaxValue;
        }

        // İş parçacığı tamamlandığında sonucun konsola yazdırılması
        Console.WriteLine($"Thread {tid} bitti. Sonuc = {result:F2}");
    }

    static void Main()
    {
        Thread[] threads = new Thread[num_thread]; // İş parçacıklarını saklamak için dizi oluşturulması

        // Belirtilen sayıda iş parçacığının oluşturulması ve başlatılması
        for (long t = 0; t < num_thread; t++)
        {
            // İş parçacığı oluşturulduğunda konsola çıktı
            Console.WriteLine($"Main: thread oluşturuldu {t}");

            // Yeni iş parçacığı oluşturulması ve başlatılması
            threads[t] = new Thread(new ParameterizedThreadStart(BusyWork));
            threads[t].Start(t);
        }

        // Ana programın tamamlandığına dair konsola çıktı
        Console.WriteLine("Main: Tamamlandı. Çıkılıyor...");
    }
}

