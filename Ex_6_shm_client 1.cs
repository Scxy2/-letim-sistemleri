using System; 
using System.IO.MemoryMappedFiles; 
using System.Runtime.InteropServices; 

class Program
{
    const string shared_memory_name = "/foo1423";  // Paylaşılan bellek nesnesinin yolunu belirten sabit tanımlanır
    const int max_lenght= 50;  // Maksimum mesaj uzunluğunu belirten sabit tanımlanır
    const int Types = 8;  // Mesaj tiplerinin sayısını belirten sabit tanımlanır

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]  // Yapının düzenini ve karakter kümesini belirleyen nitelikler eklenir
    struct msg_s
    {
        public int type;  // Mesajın tipini tutan değişken tanımlanır
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = max_lenght)]  // Verinin yapısını ve boyutunu belirleyen nitelikler eklenir
        public string content;  // Mesajın içeriğini tutan değişken tanımlanır
    }

    //BU kod sadece var olan shared_memoryden veri almak için yazılmıştır  çalıştııldığında System.IO.FileNotFoundException: 'Unable to find the specified file.' hatası vermektedir. 
    //Ex_6_shm_client çalışması için  Ex_6_shm_server'e ihtiyacı var 
    static void Main(string[] args)
    {
        int shared_seg_size = Marshal.SizeOf(typeof(msg_s));  // Yapı boyutunu hesaplayan değişken tanımlanır
        try  // hatta blogu 
        {
            using (var mmf = MemoryMappedFile.OpenExisting(shared_memory_name, MemoryMappedFileRights.ReadWrite))  // Varolan bellek eşlemeli dosyayı açan işlem bloğu başlatılır
            {
                using (var accessor = mmf.CreateViewAccessor(0, shared_seg_size, MemoryMappedFileAccess.ReadWrite))  // Bellek eşlemeli dosya üzerinde bir görünüm oluşturan işlem bloğu başlatılır
                {
                    msg_s shared_msg;  // Bellekten okunan mesajı tutan değişken tanımlanır
                    accessor.Read(0, out shared_msg);  // Bellekten veriyi okuyan işlem gerçekleştirilir

                    Console.WriteLine($"Message type is {shared_msg.type}, content is: {shared_msg.content}");  // Okunan veriyi konsola yazdıran işlem gerçekleştirilir
                }
            }
        }catch (Exception FileNotFoundException) {  
            Console.WriteLine("Shared memory bulunamadı"); 
        }
    }
}