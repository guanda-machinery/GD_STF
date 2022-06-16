#define CSHARP_MMF_API __declspec(dllexport) //請注意！正確的是Export要亮起
#include <iostream>
#include <wtypes.h>
#include <minwinbase.h>
#include <atlbase.h>
#include <winnt.h>
#include <atlstr.h>
#include <WinBase.h>
#include <WtsApi32.h>
#include <Windows.h>
#include <Windowsx.h>
#include <stdio.h>
#include <conio.h>
#include <stdlib.h>

char szName[] = "VBAT";
TCHAR szTCHAR[] = TEXT("Global\\VBAT");

extern "C" CSHARP_MMF_API void  VBAT() {
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

	GetBAVT(hLmsensor, ResourceTypeVoltage, &vCount, pVoltage, retSize);
	*write = pVoltage[0] / 100.00;
	Sleep(60000);
}