using System;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        const int coc = 5;  //sabit değer 
        int[][] childPids = new int[coc+1][];  // durumların saklanacağı düzensiz dizi   
        for (int i = 0; i < coc; i++)  // fork fonksiyonlarını çalıştırma ve düzensiz diziye atma 
        {
            childPids[i] = Fork(i);
            
        }
        childPids[coc] = new int[]{ 0 , childPids[coc - 1][1] };  //son olmayan durum olduğunda son processin kodunu saklayan kod parcaçığı

        for (int i = 0; i < coc + 1; i++)
        {
            if (CHECKSTATUS(childPids[i],i)==1)  //olmayan durum kodlu processi ayırma 
            {
                if (WIFEXITED(childPids[i]))
                {
                    Console.WriteLine($"Child {childPids[i][0]} terminated with exit status {WEXITSTATUS(childPids[i])}");  // durum bilgisi yazdırma 
                }
                else
                {
                    Console.WriteLine($"Child {childPids[i][0]} terminated abnormally");  //durum bilgisi yazdırma 
                }
            }
            else
            {
                Console.WriteLine($"Child {CHECKSTATUS(childPids[i], i)} terminated with exit status {WEXITSTATUS(childPids[i])}");  //durum bilgisi 
            }
        }

        Environment.Exit(0);
    }

    static int[] Fork(int i)                //fork fonksiyonu 
    {
        Process childProcess = new Process();   //process oluşturma 
        try
        {
            int l = 100 + i;  //döndürülecek durum kodu 
            int[] a = new int[2];  // id ve durumun saklanacagi düzensiz dizi 
            childProcess.StartInfo.FileName = "cmd.exe";   //calıştırılacak program 
            childProcess.StartInfo.Arguments = $"/c exit {l}";  //argumanlar // burada cıkıs kodunu ayarlıyoruz 
            childProcess.StartInfo.UseShellExecute = false;  // shell kullanma 
            childProcess.StartInfo.CreateNoWindow = true; // yeni pencere oluşturmama 
            childProcess.StartInfo.RedirectStandardInput = true;  //standart  giriş 
            childProcess.StartInfo.RedirectStandardOutput = true;  //standart cıkış 
            childProcess.Start();  //processi başlatma 
            a[0] = childProcess.Id; // process idsini alma 
            using (StreamWriter writer = childProcess.StandardInput)
            {
                if (writer.BaseStream.CanWrite)
                {
                    writer.WriteLine("exit"); //process cmd'sini exit yazımı 
                }
            }
            childProcess.WaitForExit();  // wait fonksiyonu----> processin işini yapmasına kadar bekler 
            a[1] = childProcess.ExitCode;  //exit code alımı 
            childProcess.Close();  //processi sonlandırma 
            return a;
        }
        catch (Exception)   // hata yakalanırsa bu kod satırı çalışır 
        {
            int[] a = new int[2];
            return a;
        }
    }


    static bool WIFEXITED(int[] k)   //Processin doğru bir şekilde kapatılmış diye mi kontrol eder 
    {
        if (k[0] != 0)
        {
            return true;
        }
        else
        { return false; }
    }

    static int WEXITSTATUS(int[] k) // durum kodunu döndürür 
    {
        return k[1];
    }

    static int CHECKSTATUS(int[] k ,int i)  //olmayan durumm processi ayırmak için yardımcı fonksiyon
    {
        for(int j = 0; j < k.Length; j++)
        {
            if (k.Contains(100 + i))
            {
                return 1;
            }
        }
        return -1;
       
    }
}
//Console.WriteLine("Kontrol");
