using System;
using System.Security.Cryptography;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine($"Running parent, pid: {System.Diagnostics.Process.GetCurrentProcess().Id}");  // durum bilgisi 
        var childProcess = new System.Diagnostics.Process(); //child process 
        childProcess.StartInfo.FileName = System.Reflection.Assembly.GetExecutingAssembly().Location;//mecvut programın yolu child processe verilir 
        childProcess.Start(); // child process başlatılır
        Console.WriteLine("Terminating child, pid:{0}",childProcess.Id);  // durum bilgisi

        childProcess.Close(); // child process sonlandırılır
       
        while (true)    // ana process sonsuz döngüye sokulur 
        {
            Console.WriteLine("*");
        }
    }
}
