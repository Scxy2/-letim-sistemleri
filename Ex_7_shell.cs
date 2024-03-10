using System;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {          
            Console.Write("COMMAND-> ");      // Kullanıcıdan komut girdisini al
            string input = Console.ReadLine();
            
            string[] commandParts = input.Split(' '); // Komutu ayrıştırma
            string command = commandParts[0];
            string[] arguments = new string[commandParts.Length - 1];
            Array.Copy(commandParts, 1, arguments, 0, arguments.Length);
            bool runInBackground = false;

            if (arguments.Length > 0 && arguments[arguments.Length - 1] == "&")    // Eğer '&' karakteri varsa, arka planda çalıştır
            {              
                runInBackground = true;
                Array.Resize(ref arguments, arguments.Length - 1);
            }
            
            ProcessStartInfo psi = new ProcessStartInfo(command);    // başlatma bilgileri 
            psi.FileName = "cmd.exe";
            psi.Arguments = string.Join(" ", arguments);
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardInput = true;

            Process process = Process.Start(psi);          // processi oluşturma 
            Console.WriteLine("Process oluşturuldu");
                 
            using (StreamWriter writer = process.StandardInput) 
            {
                if (writer.BaseStream.CanWrite)
                {
                    writer.WriteLine("exit"); //process cmd'sini exit yazımı 
                }
            }
            if (!runInBackground)
            {
                // Eğer arka planda çalıştırılmıyorsa, işlemi beklet
                process.WaitForExit();
            }
        }
    }
}
