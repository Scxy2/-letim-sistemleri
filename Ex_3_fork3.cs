using System.Diagnostics;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml.XPath;
using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;

class Program
{
    static void Main()
    {
        Console.WriteLine($"({Process.GetCurrentProcess().Id}) Parent does something...");  // durum bilgisi
        int k = Fork();    // anladığım kadarıyla c deki fork fonksiyonu hem if hemde else bloklarını çalıştırdığı için bende bu şekil yapmaya karar verdim 
        if (k > 0)
        {
            Console.WriteLine("Parent do completely different stuff"); // durum bilgisi
        } 
        if (k > 1)
        {
            Console.WriteLine("Child can do some stuff");  //durum bilgisi
        }
    }
    static int Fork()
    {
        Console.Write("parent is working...");   //durum bilgisi 
        Console.WriteLine("Parent id =>{0}", System.Diagnostics.Process.GetCurrentProcess().Id); // ana processin idsini ve durumunu yazdırma 
        Process child_process = new Process();  // yeni child process oluşturma 

        try
        {
            // child processin başlatma bilgileri  
            child_process.StartInfo.FileName = "cmd.exe";  // başlatılacak program 
            child_process.StartInfo.UseShellExecute = false;     //shell kullanmadan başlatma 
            child_process.StartInfo.RedirectStandardInput = true; // Standart girişi yönlendirme
            child_process.StartInfo.RedirectStandardOutput = true; // Standart çıkışı yönlendir
            child_process.StartInfo.CreateNoWindow = true; // pencere olusturmadan baslatma 

            child_process.Start(); // child processi başlatma            
            child_process.StandardInput.WriteLine("exit"); // cmd ekranını kapatma 
            
            string a=("child is working... =>"+ child_process.Id.ToString()); // child processin idsini değişkene atama 
            child_process.Close(); // processi sonlandırma

            Console.WriteLine(a); //child processin idsini yazdırma 
            return 2;

        }
        catch (Exception)
        {

            return 1;
        }
    }
}