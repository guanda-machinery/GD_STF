#include "stdafx.h"
#include "Voltage.h"
#define CSHARP_MMF_API __declspec(dllexport) 
char szName[] = "VBAT";
TCHAR szTCHAR[] = TEXT("Global\\VBAT");

extern "C" CSHARP_MMF_API void  VBAT() {
	static MyStruct  drive;
	static HANDLE hLmsensor, mem;
	if (!hLmsensor)
	{
		hLmsensor = CreateFile(_T("\\\\.\\AdvLmDev"),
			GENERIC_WRITE,
			FILE_SHARE_WRITE,
			NULL,
			OPEN_EXISTING,
			0,
			NULL);
	}

	static ULONG vCount = 0;
	static DWORD retSize = 0, errorCode = 0;
	drive.GetRetSize(hLmsensor, ResourceTypeVoltage, &retSize, &errorCode);
	static PULONG pVoltage = (PULONG)malloc(retSize);
	double* write = NULL;
	drive.LocalSysCreateNamedShm(szTCHAR, &mem, (void**)&write, sizeof(double));

	drive.GetBAVT(hLmsensor, ResourceTypeVoltage, &vCount, pVoltage, retSize);
	*write = pVoltage[0] / 100.00;
	Sleep(10000);
}