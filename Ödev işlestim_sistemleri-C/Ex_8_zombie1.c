#include <stdio.h>
#include <stdlib.h>

int k(int l){
	int sayi1 ,sayi2,sayi3;
	if(l>0){
		sayi1 = k(l-1);
		sayi2 = k(l-1);
		sayi3 = k(l-1);
		return sayi1 +sayi2 +sayi3;
		
	}
	else{
		return 0 ;
	}
}
int main ( void ) {

	printf("%d",k(5));


	return 0;
}

