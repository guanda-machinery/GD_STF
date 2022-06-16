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
	HANDLE hProcess;

	unsigned long iMinSize;
	unsigned long iMaxSize;
	/* 以下情況需要全局...：
	 - 運行時作為系統服務運行（plcwinnt 的默認設置）
	 - 運行時想要與不在系統帳戶下運行的進程通信
	 -> 會因為不同的命名空間而失敗，至少在 Windows Vista 中！
	 從 Vista 下的服務啟動的 Targetvisu 出現問題*/
	char szNameBuffer[MAX_PATH];
	char szGlobalPrefix[] = "Global\\";

	strcpy(szNameBuffer, szGlobalPrefix);
	strcat(szNameBuffer, szName);
	//全域，然後檢查本地名稱
	*phShmHandle = OpenFileMappingA(FILE_MAP_WRITE, TRUE, szNameBuffer);
	if (*phShmHandle == 0)
		*phShmHandle = OpenFileMappingA(FILE_MAP_WRITE, TRUE, szName);

	if (*phShmHandle == 0)
	{
		* phShmHandle = CreateFileMappingA((HANDLE)0xffffffff, NULL, PAGE_READWRITE, 0, ulSize, szName);
	}
	*ppUserSpace = MapViewOfFile((HANDLE)*phShmHandle, FILE_MAP_WRITE, 0, 0, 0);

	return TRUE;
}