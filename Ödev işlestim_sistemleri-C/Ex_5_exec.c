#include <stdio.h>
#include <stdlib.h>
int algoritmaB(int n ,int k) {
	int sayi1, sayi2, sayi3;
	if (n > 0) {
		printf("\n%d",k);
		sayi1 = algoritmaB(n - 1,k-1);
		sayi2 = algoritmaB(n - 1,k-1);
		sayi3 = algoritmaB(n - 1,k-1);
		return sayi1 + sayi2 + sayi3;
	} 
	else {
		printf("\n0");
		return 0;
	}
}


int main ( void )  {
	printf("%d",algoritmaB(3,3));
	return 0;
}

