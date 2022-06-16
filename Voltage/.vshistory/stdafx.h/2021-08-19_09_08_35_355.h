#pragma once
#include "Header.h"

/// <summary>
/// 取得 DeviceIoControl
/// </summary>
/// <param name="hLmsensor"> 指標</param>
/// <param name="ResourceTypeID">來源 id</param>
/// <param name="ObjectCount">物件指標</param>
/// <returns></returns>
PULONG GetValues(HANDLE hLmsensor, ULONG ResourceTypeID, PULONG ObjectCount) {
	DWORD retSize = 0;
	if (NULL == ObjectCount) return NULL;

	if (!DeviceIoControl(hLmsensor, IOCTL_LMSENSOR_GET_VALUES, &ResourceTypeID, sizeof(ResourceTypeID), NULL, 0, &retSize, NULL))
	{
		DWORD errcode = GetLastError();
		if (errcode == ERROR_MORE_DATA)
		{
		 	PULONG pValues = (PULONG)malloc(retSize);
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

/// <summary>
/// 取得 DeviceIoControl
/// </summary>
/// <param name="hLmsensor"> 指標</param>
/// <param name="ResourceTypeID">來源 id</param>
/// <param name="ObjectCount">物件指標</param>
/// <returns></returns>
void GetValues(HANDLE hLmsensor, ULONG resourceTypeID, PULONG objectCount, PULONG* result ) {
	static DWORD retSize = 0;
	if (NULL == result)
		return ;

	if (!DeviceIoControl(hLmsensor, IOCTL_LMSENSOR_GET_VALUES, &resourceTypeID, sizeof(resourceTypeID), NULL, 0, &retSize, NULL))
	{
		DWORD errcode = GetLastError();
		if (errcode == ERROR_MORE_DATA)
		{
			static PULONG pValues = (PULONG)malloc(retSize);
			if (!pValues)
			{
				printf("失敗....\n");
				return;
			}
			if (!DeviceIoControl(hLmsensor, IOCTL_LMSENSOR_GET_VALUES, &resourceTypeID, sizeof(resourceTypeID), pValues, retSize, &retSize, NULL))
			{
				printf("失敗....\n");
				free(pValues);
			}
			else
			{
				*objectCount = retSize / sizeof(ULONG);
				result = &pValues;
				return ;
			}
		}
	}

}

//BOOL LocalSysCreateNamedShm(char* szName, unsigned long ulSize, HANDLE* phShmHandle, void** ppUserSpace)
//{
//	HANDLE	hProcess;
//	unsigned long lMinimumWorkingSetSize;
//	unsigned long lMaximumWorkingSetSize;
//	// 以下情況需要全局...：
//	// - 運行時作為系統服務運行（plcwinnt 的默認設置）
//	// - 運行時想要與不在系統帳戶下運行的進程通信
//	// -> 會因為不同的命名空間而失敗，至少在 Windows Vista 中！
//	// 從 Vista 下的服務啟動的 Targetvisu 出現問題
//
//	char szNameBuffer[MAX_PATH];
//	char szGlobalPrefix[] = "Global\\";
//	char szSessionPrefix[] = "Session\\";
//
//	strcpy(szNameBuffer, szGlobalPrefix);
//	strcat(szNameBuffer, szName);
//
//	// prefer the global entry and afterwards check for the local name
//	*phShmHandle = OpenFileMappingA(FILE_MAP_WRITE, TRUE, szNameBuffer);
//	if (*phShmHandle == 0)
//		*phShmHandle = OpenFileMappingA(FILE_MAP_WRITE, TRUE, szName);
//
//	// check if a shared memory exists in any active user session
//	if (*phShmHandle == 0)
//	{
//		/* walk over all sessions to check if the shared memory is opened somewhere */
//		COPIED_WTS_SESSION_INFO* pResult;
//		DWORD dwCount;
//		DWORD i;
//
//		WTSEnumerateSessions(0 /* current machine */, 0, 1, (WTS_SESSION_INFO**)&pResult, &dwCount);
//
//		for (i = 0; i < dwCount; ++i)
//		{
//			char szTemp[15];
//			strcpy(szNameBuffer, szSessionPrefix);
//			strcat(szNameBuffer, _itoa((int)pResult[i].SessionId, szTemp, 10));
//			strcat(szNameBuffer, "\\");
//			strcat(szNameBuffer, szName);
//
//			*phShmHandle = OpenFileMappingA(FILE_MAP_WRITE, TRUE, szNameBuffer);
//			if (*phShmHandle != 0)
//				break;
//		}
//
//		WTSFreeMemory(pResult);
//	}
//
//	if (*phShmHandle == 0)
//	{
//#if 0
//		//See examlpe above how to obtain a security descriptor. 
//		SECURITY_ATTRIBUTES sa;
//		SECURITY_ATTRIBUTES* psa = NULL;
//		if (s_pSecurityDescriptorData)
//		{
//			sa.nLength = sizeof(sa);
//			sa.bInheritHandle = FALSE;
//			sa.lpSecurityDescriptor = &s_SecurityDescriptor;
//			psa = &sa;
//		}
//#endif
//
//		//*phShmHandle = CreateFileMapping((HANDLE)0xffffffff, psa, PAGE_READWRITE, 0, ulSize, szName);
//		* phShmHandle = CreateFileMappingA((HANDLE)0xffffffff, NULL, PAGE_READWRITE, 0, ulSize, szName);
//	}
//
//	if (*phShmHandle == 0)
//	{
//		return FALSE;
//	}
//	*ppUserSpace = MapViewOfFile((HANDLE)*phShmHandle, FILE_MAP_WRITE, 0, 0, 0);
//	if (*ppUserSpace == NULL)
//	{
//		return FALSE;
//	}
//
//	hProcess = GetCurrentProcess();
//	GetProcessWorkingSetSize(hProcess, (PSIZE_T)&lMinimumWorkingSetSize, (PSIZE_T)&lMaximumWorkingSetSize);
//	if (lMinimumWorkingSetSize <= ulSize)
//	{
//		lMinimumWorkingSetSize = ulSize + 0x10000UL;
//		if (lMaximumWorkingSetSize <= lMinimumWorkingSetSize)
//		{
//			lMaximumWorkingSetSize = lMinimumWorkingSetSize + 0x1000000UL;
//		}
//		if (!SetProcessWorkingSetSize(hProcess, lMinimumWorkingSetSize, lMaximumWorkingSetSize))
//		{
//			DWORD dwError = GetLastError();
//			/*	return 0; When this call produces an error, everything still seems to work, but returning 0 will lead to a crash of Targetvisu.*/
//		}
//	}
//	/* Lock memory mapped file */
//	if (!VirtualLock((LPVOID)*ppUserSpace, ulSize))
//	{
//		DWORD dwError = GetLastError();
//		/* TOCHECK: Don't know why locking does not work for SHM larger than 1MB */
//		/*return 0;*/
//	}
//	return TRUE;
//}