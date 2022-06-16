#pragma once
#include "Header.h"
#pragma warning(disable:4996)
#define SHAREMEM_NAME "BAVT"
#include <string.h>
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
		printf("成功");
		*errorCode = GetLastError();
	}
	else
	{
		printf("失敗");
	}
}
void GetBAVT(HANDLE hLmsensor, ULONG ResourceTypeID, PULONG ObjectCount, PULONG pValues, LPDWORD retSize) {
	if (!DeviceIoControl(hLmsensor, IOCTL_LMSENSOR_GET_VALUES, &ResourceTypeID, sizeof(ResourceTypeID), pValues, (DWORD)retSize, retSize, NULL))
	{
		free(pValues);
	}
	else
	{
		*ObjectCount = (DWORD)retSize / sizeof(ULONG);
	}
}
///// <summary>
///// 取得 DeviceIoControl
///// </summary>
///// <param name="hLmsensor"> 指標</param>
///// <param name="ResourceTypeID">來源 id</param>
///// <param name="ObjectCount">物件指標</param>
///// <returns></returns>
//void GetValues(HANDLE hLmsensor, ULONG resourceTypeID, PULONG objectCount, PULONG* result, DWORD* pRetSize) {
//	DWORD retValue = (DWORD)pRetSize;
//	if (NULL == result)
//		return;
//
//	if (!DeviceIoControl(hLmsensor, IOCTL_LMSENSOR_GET_VALUES, &resourceTypeID, sizeof(resourceTypeID), NULL, 0, pRetSize, NULL))
//	{
//		DWORD errcode = GetLastError();
//		if (errcode == ERROR_MORE_DATA)
//		{
//			static PULONG pValues = (PULONG)malloc(retValue);
//			if (!pValues)
//			{
//				printf("失敗....\n");
//				return;
//			}
//			if (!DeviceIoControl(hLmsensor, IOCTL_LMSENSOR_GET_VALUES, &resourceTypeID, sizeof(resourceTypeID), pValues, retValue, pRetSize, NULL))
//			{
//				printf("失敗....\n");
//				free(pValues);
//			}
//			else
//			{
//				*objectCount = (DWORD)pRetSize / sizeof(ULONG);
//				result = &pValues;
//				return;
//			}
//		}
//	}
//}
//BOOL LocalSysCreateNamedShm(TCHAR szName[], PULONG* value)
//{
//	HANDLE hMapFile = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, sizeof(long), szName);
//}