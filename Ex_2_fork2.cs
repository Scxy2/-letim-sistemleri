using System;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        int forkResult;

        Console.WriteLine("process id : {0}", System.Diagnostics.Process.GetCurrentProcess().Id);  //Main processin idsini yazdırma

        forkResult = Fork();  //Fork metotu
        Console.WriteLine("process id : {0} - result : {1}", System.Diagnostics.Process.GetCurrentProcess().Id, forkResult); // Main process ve chid process'in idsini yazdırma 
    }

    static int Fork()
    {
        Process child_process = new Process();  // yeni child process oluşturma 

        int result = 0;  // sonda döndürülecek değer 

        // child processin başlatma bilgileri  
        child_process.StartInfo.FileName = "cmd.exe";  // başlatılacak program 
        child_process.StartInfo.UseShellExecute = false;     //shell kullanmadan başlatma 
        child_process.StartInfo.RedirectStandardInput = true; // Standart girişi yönlendir
        child_process.StartInfo.CreateNoWindow = true; // pencere olusturmadan baslatma 

        child_process.Start(); // child processi başlatma 

        result = child_process.Id; // child processin idsini değişkene atama 
        child_process.StandardInput.WriteLine("exit"); // cmd ekranını kapatma 
        child_process.Close(); // processi sonlandırma 
        return result; // id döndürme 
    }
}

