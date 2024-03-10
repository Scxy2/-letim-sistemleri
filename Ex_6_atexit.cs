using System;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

class Program
{
    static void Main()
    {
        int k = Fork();
        if (k > 0)
        {
            Console.WriteLine("Parent id =>{0}", Process.GetCurrentProcess().Id); // durum bilgisi 
            ParentCleaner();
        }
    }

    static int Fork()
    {
        try
        {
            int child_processid;
            Process childProcess = new Process();  //child 
            childProcess.StartInfo.FileName = "cmd.exe"; // cmd.exe yi başlatma  
            childProcess.StartInfo.UseShellExecute = false;//shell kullanmadan başlatma 
            childProcess.StartInfo.CreateNoWindow = true;// pencere olusturmadan baslatma 
            childProcess.Start();
            child_processid = childProcess.Id;           
            childProcess.Close();
            Console.WriteLine("this is child...=>{0}",child_processid);  // durum bilgisi 

            return 1;
        }
        catch (Exception)
        {
            return 2;
        }
    }
    static void ParentCleaner()
    {
        Console.WriteLine("cleaning up parent...");//durum bilgisi 
        Environment.Exit(0);  //ana proccessi sonlandırır
    }
}
