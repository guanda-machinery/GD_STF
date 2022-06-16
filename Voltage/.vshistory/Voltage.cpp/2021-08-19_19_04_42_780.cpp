// Voltage.cpp : 此檔案包含 'main' 函式。程式會於該處開始執行及結束執行。
//

#include <iostream>
#include "Header.h"
#include "stdafx.h"
char szName[] = "VBAT";
TCHAR szTCHAR[] = TEXT("Global\\VBAT");
int main() {

	FreeConsole();
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
	PULONG pVoltage = (PULONG)malloc(retSize);
	double* write = NULL;
	LocalSysCreateNamedShm(szTCHAR, &mem, (void**)&write, sizeof(double));
	while (true)
	{

		GetBAVT(hLmsensor, ResourceTypeVoltage, &vCount, pVoltage, retSize);
		*write = pVoltage[0] / 100.00;
		Sleep(1000);
	}
}
//int main()
//{
//	HANDLE hLmsensor, mem;
//	hLmsensor = CreateFile(_T("\\\\.\\AdvLmDev"),
//		GENERIC_WRITE,
//		FILE_SHARE_WRITE,
//		NULL,
//		OPEN_EXISTING,
//		0,
//		NULL);
//	ULONG vCount = 0;
//	DWORD retSize = 0, errorCode = 0;
//	GetRetSize(hLmsensor, ResourceTypeVoltage, &retSize, &errorCode);
//	printf("%d", retSize);
//	PULONG pVoltage = (PULONG)malloc(retSize);
//	double* write = NULL;
//	LocalSysCreateNamedShm(szName, sizeof(double), &mem, (void**)&write);
//	while (true)
//	{
//
//		/*pVoltage = GetValues(hLmsensor, ResourceTypeVoltage, &vCount);*/
//		GetBAVT(hLmsensor, ResourceTypeVoltage, &vCount, pVoltage, retSize);
//		*write = pVoltage[0] /100.00;
//		if (pVoltage)
//		{
//			float a = pVoltage[0] / 100.00;
//			printf("地址：%p \n", &pVoltage[0]);
//			printf("size: %d \n", sizeof(pVoltage[0]));
//			printf("+BAVT：%f\n", a);
//			Sleep(1000);
//		}
//	}
//}
