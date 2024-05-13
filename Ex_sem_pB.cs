using System;
using System.Threading;

class Program
{
    static Semaphore semaphore = new Semaphore(1, 1); // Semafor oluşturulur: İzin verilen eş zamanlı işlem sayısı 1, başlangıçta kullanılabilir olan izin sayısı 1
    static int counter = 0;
    static void Main()
    {
        for (int i = 0; i < 10; i++)
        {
            Thread thread = new Thread(Process);
            thread.Start();
        }
    }
    static void Process()
    {
        for (int i = 0; i < 10; i++)
        {
            semaphore.WaitOne(); // Kritik bölgeye girmek için semafor bekler
            Console.WriteLine($"Process enters the critical section at {DateTime.Now:HH:mm:ss.fff}"); // Kritik bölgeye giriş zamanını yazdırır
            counter++;
            Console.WriteLine($"Counter value: {counter}"); // Counter değerini yazdırır
            Console.WriteLine($"Process leaves the critical section at {DateTime.Now:HH:mm:ss.fff}"); // Kritik bölgeden çıkış zamanını yazdırır
            semaphore.Release(); // Kritik bölgeden çıkar ve başka bir iş parçacığının girmesine izin verir
            Thread.Sleep(2000); // 2 saniye bekler
        }
    }
}
