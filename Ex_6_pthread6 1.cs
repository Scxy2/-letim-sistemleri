using System;
using System.Diagnostics.Metrics;
using System.Threading;
class Program
{
    static int counter = 0;

    static void Main(string[] args)
    {
        // İş parçacıklarını depolamak için bir dizi oluşturuluyor.
        Thread[] threads = new Thread[2];

        // İş parçacıkları oluşturulup başlatılıyor.
        for (int i = 0; i < 2; i++)
        {
            threads[i] = new Thread(DoSomeThing);
            threads[i].Start();
        }

        // Tüm iş parçacıklarının bitmesi bekleniyor.
        foreach (Thread thread in threads)
        {
            thread.Join();
        }
    }
    //Bu kod bir yarış koşulu içeriR.İki iş parçacığı aynı anda çalıştığı için counter değişkenine aynı anda erişebilirler,
    //bu da beklenmeyen sonuçlara yol açabilir. Bu durumu düzeltmek için senkronizasyon mekanizmaları kullanılmalıdır.
    static void DoSomeThing()
    {
        // counter değişkeni artırılıyor.
        counter++;
        // Başlangıç mesajı yazdırılıyor.
        Console.WriteLine($"\n thread {counter} başladı\t thread:{Thread.CurrentThread.ManagedThreadId}");

        // 3 saniye uyutuluyor.
        Thread.Sleep(3000);

        // Bitiş mesajı yazdırılıyor.
        Console.WriteLine($"\n thread  {counter} bitti\t thread:{Thread.CurrentThread.ManagedThreadId}");
    }
}