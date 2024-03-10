
using System;

using System;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Message before fork");                  // Process.Start metotunu kullanarak yeni bir işlem başlatır.Bu, ana programın bir kopyasını başlatır ve yürütür.
                                                                                        //Ana programın devam ettiği yerden devam eder.Bu nedenle, 
        System.Diagnostics.Process.Start(System.Reflection.Assembly.GetEntryAssembly().Location);       //"Message after fork" hem ana süreçte hem de yeni başlatılan süreçte yazdırılır.

        Console.WriteLine("Message after fork");
    }
}


