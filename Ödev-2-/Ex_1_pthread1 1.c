#include <pthread.h>
#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>

#define NUM_THREADS	5

// Strings for the Hello World messages 
char *messages[NUM_THREADS];

// Thread işlevi
void *PrintHello(void *threadid) {
   int *myarg;
   sleep(1);                  // 1 saniye uyku
   myarg = (int *) threadid;  // İd'yi argümandan al
   printf("Thread %d: %s\n", *myarg, messages[*myarg]);
   pthread_exit(NULL);        // Thread'in sonlandırılması
}

int main(int argc, char *argv[]) {
  
  pthread_t threads[NUM_THREADS];    // Thread tanımlayıcıları 
  int i, rc, *taskid[NUM_THREADS];   // Her bir thread için id numaraları 

  // Her bir thread için mesajların atanması
  messages[0] = "English: Hello World!"; 
  messages[1] = "French: Bonjour, le monde!";
  messages[2] = "Spanish: Hola al mundo";
  messages[3] = "Klingon: Nuq neH!";
  messages[4] = "German: Guten Tag, Welt!"; 
   
  // Her bir thread için işlevin çağrılması ve thread'in oluşturulması
  for(i = 0; i < NUM_THREADS; i++) {
    // Thread'lere argüman olarak kullanılacak diziye bellek tahsisi
    taskid[i] = (int *) malloc(sizeof(int));
    *taskid[i] = i;
    // taskid[i] dizisindeki argümanı kullanarak thread oluşturma
    rc = pthread_create(&threads[i], NULL, PrintHello, (void *) taskid[i]);
    // Hata kontrolü
    if (rc) { 
      printf("ERROR; return code from pthread_create() is %d\n", rc);
      exit(-1);
    }
  }
  pthread_exit(0); // Ana thread'in sonlandırılması
} 