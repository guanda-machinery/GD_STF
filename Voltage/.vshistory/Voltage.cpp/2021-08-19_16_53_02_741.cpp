// Voltage.cpp : 此檔案包含 'main' 函式。程式會於該處開始執行及結束執行。
//

#include <iostream>
#include "Header.h"
#include "stdafx.h"
char szName[] = "Global\\VBAT";
int main()
{
	HANDLE hLmsensor, mem;
	hLmsensor = CreateFile(_T("\\\\.\\AdvLmDev"),
		GENERIC_WRITE,
		FILE_SHARE_WRITE,
		NULL,
		OPEN_EXISTING,
		0,
		NULL);
	ULONG vCount = 0;
	DWORD retSize = 0, errorCode = 0;
	GetRetSize(hLmsensor, ResourceTypeVoltage, &retSize, &errorCode);
	printf("%d", retSize);
	PULONG pVoltage = (PULONG)malloc(retSize);
	double* write = new double;
	*write = 200;
	LocalSysCreateNamedShm(szName, sizeof(double), &mem, (void**)write);
	while (true)
	{
		*write = 3000;
		/*pVoltage = GetValues(hLmsensor, ResourceTypeVoltage, &vCount);*/
		GetBAVT(hLmsensor, ResourceTypeVoltage, &vCount, pVoltage, retSize);
		if (pVoltage)
		{
			float a = pVoltage[0] / 100.00;
			printf("地址：%p \n", &pVoltage[0]);
			printf("size: %d \n", sizeof(pVoltage[0]));
			printf("+BAVT：%f\n", a);
			Sleep(1000);
		}
	}
}
