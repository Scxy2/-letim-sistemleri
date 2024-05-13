using System;
using System.IO.MemoryMappedFiles;
using System.Text;

namespace SharedMemoryExample
{
    class Program
    {
        static void Main()
        {
            // Paylaşılan bellek nesnesinin adı
            const string shared_memory_name = "MySharedMemory";
            // Maksimum uzunluk
            const int max_lenght = 50;
            // Bellek eşlemeli dosyayı oluştur
            MemoryMappedFile mmf = MemoryMappedFile.CreateNew(shared_memory_name, max_lenght);
            // Bellek eşlemeli dosyaya yazmak için bir görünüm oluştur
            using (MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor())
            {
                // Mesaj türünü yaz
                int messageType = 1;
                accessor.Write(0, messageType);

                // Mesaj içeriğini UTF-8 kodlamasına göre yaz
                string messageContent = "Merhaba, bu bir test mesajıdır.";
                byte[] contentBytes = Encoding.UTF8.GetBytes(messageContent);
                accessor.WriteArray(sizeof(int), contentBytes, 0, contentBytes.Length);
            }
            // Kullanıcıya mesajın paylaşılan bellek segmentine bırakıldığını bildir
            Console.WriteLine("Mesaj paylaşılan bellek segmentine bırakıldı.");
            // Bellek eşlemeli dosyayı aç
            using (MemoryMappedFile.OpenExisting(shared_memory_name))
            {
                // Bellek eşlemeli dosyadan okumak için bir görünüm oluştur
                using (MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor())
                {
                    // Mesaj türünü oku
                    int messageTypeRead = accessor.ReadInt32(0);
                    // Mesaj içeriğini oku
                    byte[] contentBytesRead = new byte[max_lenght];
                    accessor.ReadArray(sizeof(int), contentBytesRead, 0, max_lenght);
                    string messageContentRead = Encoding.UTF8.GetString(contentBytesRead).TrimEnd('\0');
                    // Mesajı ekrana yazdır
                    Console.WriteLine($"Mesaj tipi: {messageTypeRead}");
                    Console.WriteLine($"Mesaj içeriği: {messageContentRead}");
                }
            }
        }
    }
}