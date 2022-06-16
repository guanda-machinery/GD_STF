#pragma once
#include "Header.h"
#pragma warning(disable:4996)
#define SHAREMEM_NAME "BAVT"
/// <summary>
/// 取得 DeviceIoControl
/// </summary>
/// <param name="hLmsensor"> 指標</param>
/// <param name="ResourceTypeID">來源 id</param>
/// <param name="ObjectCount">物件指標</param>
/// <returns></returns>
PULONG GetValues(HANDLE hLmsensor, ULONG ResourceTypeID, PULONG ObjectCount) {
	DWORD retSize = 0;
	if (NULL == ObjectCount)
		return NULL;

	if (!DeviceIoControl(hLmsensor, IOCTL_LMSENSOR_GET_VALUES, &ResourceTypeID, sizeof(ResourceTypeID), NULL, 0, &retSize, NULL))
	{
		DWORD errcode = GetLastError();
		if (errcode == ERROR_MORE_DATA)
		{
			static PULONG pValues = (PULONG)malloc(retSize);
			if (!pValues)
			{
				goto End;
			}
			if (!DeviceIoControl(hLmsensor, IOCTL_LMSENSOR_GET_VALUES, &ResourceTypeID, sizeof(ResourceTypeID), pValues, retSize, &retSize, NULL))
			{
				free(pValues);
				goto End;
			}
			else
			{
				*ObjectCount = retSize / sizeof(ULONG);
				return pValues;
			}
		}
	}
End:
	return NULL;
}

void  GetRetSize(HANDLE hLmsensor, ULONG resourceTypeID, LPDWORD retSize, LPDWORD errorCode) {

	if (!DeviceIoControl(hLmsensor, IOCTL_LMSENSOR_GET_VALUES, &resourceTypeID, sizeof(resourceTypeID), NULL, 0, retSize, NULL))
	{
		*errorCode = GetLastError();
	}
}
void GetBAVT(HANDLE hLmsensor, ULONG ResourceTypeID, PULONG ObjectCount, PULONG pValues, DWORD retSize) {
	if (!DeviceIoControl(hLmsensor, IOCTL_LMSENSOR_GET_VALUES, &ResourceTypeID, sizeof(ResourceTypeID), pValues, retSize, &retSize, NULL))
	{
		free(pValues);
	}
	else
	{
		*ObjectCount = retSize / sizeof(ULONG);
	}
}
BOOL LocalSysCreateNamedShm(TCHAR szName[], HANDLE* phShmHandle, void** ppUserSpace, unsigned long ulSize)
{
	*phShmHandle = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, ulSize, szName);
	if (phShmHandle == NULL)
	{
		printf("錯誤");
	}
	*ppUserSpace = MapViewOfFile((HANDLE)*phShmHandle, FILE_MAP_WRITE, 0, 0, 0);

	return true;
}

BOOL LocalSysCreateNamedShm(char* szName, unsigned long ulSize, HANDLE* phShmHandle, void** ppUserSpace)
{
	HANDLE	hProcess;
	unsigned long lMinimumWorkingSetSize;
	unsigned long lMaximumWorkingSetSize;
	// the global... is needed for the following case: 
	// - runtime is running as a system service (the default for the plcwinnt)
	// - runtime wants to communicate with a process that does not run under the system accout
	// -> Will fail because of the different namespaces, at least in Windows Vista!
	// Problem occurred in the Targetvisu that is started from the service unter Vista

	char szNameBuffer[MAX_PATH];
	char szGlobalPrefix[] = "Global\\";
	char szSessionPrefix[] = "Session\\";

	strcpy(szNameBuffer, szGlobalPrefix);
	strcat(szNameBuffer, szName);

	// prefer the global entry and afterwards check for the local name
	*phShmHandle = OpenFileMappingA(FILE_MAP_WRITE, TRUE, szNameBuffer);
	//if (*phShmHandle == 0) 
	//	*phShmHandle = OpenFileMappingA(FILE_MAP_WRITE, TRUE, szName);

	//// check if a shared memory exists in any active user session
	//if (*phShmHandle == 0)
	//{
	//	/* walk over all sessions to check if the shared memory is opened somewhere */
	//	COPIED_WTS_SESSION_INFO* pResult;
	//	DWORD dwCount;
	//	DWORD i;

	//	WTSEnumerateSessions(0 /* current machine */, 0, 1, (WTS_SESSION_INFO**)&pResult, &dwCount);

	//	for (i = 0; i < dwCount; ++i)
	//	{
	//		char szTemp[15];
	//		strcpy(szNameBuffer, szSessionPrefix);
	//		strcat(szNameBuffer, _itoa((int) pResult[i].SessionId, szTemp, 10));
	//		strcat(szNameBuffer, "\\");
	//		strcat(szNameBuffer, szName);		

	//		*phShmHandle = OpenFileMapping(FILE_MAP_WRITE, TRUE, szNameBuffer);
	//		if (*phShmHandle != 0)
	//			break;
	//	}

	//	WTSFreeMemory(pResult);
	//}

	if (*phShmHandle == 0)
	{
#if 0
		//See examlpe above how to obtain a security descriptor. 
		SECURITY_ATTRIBUTES sa;
		SECURITY_ATTRIBUTES* psa = NULL;
		if (s_pSecurityDescriptorData)
		{
			sa.nLength = sizeof(sa);
			sa.bInheritHandle = FALSE;
			sa.lpSecurityDescriptor = &s_SecurityDescriptor;
			psa = &sa;
		}
#endif

		//*phShmHandle = CreateFileMapping((HANDLE)0xffffffff, psa, PAGE_READWRITE, 0, ulSize, szName);
		* phShmHandle = CreateFileMappingA((HANDLE)0xffffffff, NULL, PAGE_READWRITE, 0, ulSize, szName);
	}

	if (*phShmHandle == 0)
	{
		return FALSE;
	}
	*ppUserSpace = MapViewOfFile((HANDLE)*phShmHandle, FILE_MAP_WRITE, 0, 0, 0);
	if (*ppUserSpace == NULL)
	{
		return FALSE;
	}

	//hProcess = GetCurrentProcess();
	//GetProcessWorkingSetSize(hProcess, (PSIZE_T)&lMinimumWorkingSetSize, (PSIZE_T)&lMaximumWorkingSetSize);
	//if (lMinimumWorkingSetSize <= ulSize)
	//{
	//	lMinimumWorkingSetSize = ulSize + 0x10000UL;
	//	if (lMaximumWorkingSetSize <= lMinimumWorkingSetSize)
	//	{
	//		lMaximumWorkingSetSize = lMinimumWorkingSetSize + 0x1000000UL;
	//	}
	//	if (!SetProcessWorkingSetSize(hProcess, lMinimumWorkingSetSize, lMaximumWorkingSetSize))
	//	{
	//		DWORD dwError = GetLastError();
	//		/*	return 0; When this call produces an error, everything still seems to work, but returning 0 will lead to a crash of Targetvisu.*/
	//	}
	//}
	///* Lock memory mapped file */
	//if (!VirtualLock((LPVOID)*ppUserSpace, ulSize))
	//{
	//	DWORD dwError = GetLastError();
	//	/* TOCHECK: Don't know why locking does not work for SHM larger than 1MB */
	//	/*return 0;*/
	//}

}