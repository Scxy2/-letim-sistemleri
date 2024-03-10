using System;
using System.Diagnostics;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("Parent does stuff and then calls fork...");  //Durum bilgisi 
        int k = Fork();
        if (k > 0)
        {
            Console.WriteLine("... parent do something completely different");//Durum bilgisi
        }
        if (k > 1)
        {
            Console.WriteLine("Child runs an executable..."); //Durum bilgisi 
        }

        //Fork fonksiyonu eğer child processi başlatmazsa 1 , başlatırsa 2 döndürür yukardaki iki if bloğuna göre parent ve child düğümleri ayırır. 

    }

    static int Fork()
    {
        try
        {
            Process childProcess = new Process();  //child 
            childProcess.StartInfo.FileName = "cmd.exe"; // cmd.exe yi başlatma 
            childProcess.StartInfo.Arguments = "/c dir"; ; //argumanlar 
            childProcess.StartInfo.UseShellExecute = false;//shell kullanmadan başlatma 
            childProcess.StartInfo.CreateNoWindow = true;// pencere olusturmadan baslatma 
            childProcess.Start();
            childProcess.Close();
            return 2;
        }
        catch (Exception)
        {
            return 1;
        }
    }
}
