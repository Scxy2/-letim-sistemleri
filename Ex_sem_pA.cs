using System;
using System.Threading;

class Program
{
    static Semaphore semaphore = new Semaphore(1, 1); // Semafor oluşturulur ve başlangıçta 1 izin verilen izin sayısı ile başlatılır.

    static void Main(string[] args)
    {
        for (int i = 0; i < 10; i++)
        {
            Thread thread = new Thread(Process); // Her bir iş parçacığı için yeni bir iş parçacığı oluşturulur.
            thread.Start(); // İş parçacığı başlatılır.
            Thread.Sleep(2000); // 1 saniye beklenir.
        }
    }
    static void Process()
    {
        // Critical section işlemleri burada yapılır.
        semaphore.WaitOne(); // Semafor beklenir.
        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} enters the critical section."); // Kritik bölgeye giriş mesajı yazdırılır. 
        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} leaves the critical section."); // Kritik bölgeden çıkış mesajı yazdırılır.
        semaphore.Release(); // Semafor serbest bırakılır.
    }
}