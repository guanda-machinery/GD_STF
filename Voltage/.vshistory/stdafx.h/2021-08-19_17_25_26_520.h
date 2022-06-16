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
	*ppUserSpace = MapViewOfFile(*phShmHandle, FILE_MAP_ALL_ACCESS, 0, 0, ulSize);

	return true;
}

BOOL LocalSysCreateNamedShm(char* szName, unsigned long ulSize, HANDLE* phShmHandle, void** ppUserSpace)
{
	unsigned long lMinimumWorkingSetSize;
	unsigned long lMaximumWorkingSetSize;

	char szNameBuffer[MAX_PATH];
	char szGlobalPrefix[] = "Global\\";
	char szSessionPrefix[] = "Session\\";

	strcpy(szNameBuffer, szGlobalPrefix);
	strcat(szNameBuffer, szName);

	*phShmHandle = OpenFileMappingA(FILE_MAP_WRITE, TRUE, szNameBuffer);
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
}