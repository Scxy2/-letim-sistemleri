using System.Diagnostics;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml.XPath;
using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;

class Program
{
    static void Main()                //Burda ne yaptığımda bende bilmiyorum  verilen c dosyasını çalıştırdığımda 2 4 ve 2 4 3 4 cıktılarını alıyorum. 
    {                                       //chatgpt verilen c dosyasını calıştırmasını söylediğimde ise 1 2 3 4 cıktılarını verecğine söylüyor 
        int a = Fork();                            //benim bu yazdığım koda ise  1 3 4   cıktısını alıyorum. 
        if (a > 0)                                                      
        {
            int b = Fork();
            if (b > 1)
            {
                int c = Fork();
                Console.WriteLine("1");
            }
           if(b > 2) 
            {
                Console.WriteLine("2");
            }
        }
        if (a > 1)
        {
            Console.WriteLine("3");
        }
        Console.WriteLine("4");
    }
    static int Fork()
    {
        Process child_process = new Process();  // yeni child process oluşturma 

        try
        {
            // child processin başlatma bilgileri  
            child_process.StartInfo.FileName = "cmd.exe";  // başlatılacak program 
            child_process.StartInfo.UseShellExecute = false;     //shell kullanmadan başlatma 
            child_process.StartInfo.RedirectStandardInput = true; // Standart girişi yönlendirme
            child_process.StartInfo.CreateNoWindow = true; // pencere olusturmadan baslatma 
            child_process.Start(); // child processi başlatma            
            child_process.StandardInput.WriteLine("exit"); // cmd ekranını kapatma 
            child_process.Close(); // processi sonlandırma
            return 2;

        }
        catch (Exception)
        {

            return 1;
        }
    }
}
