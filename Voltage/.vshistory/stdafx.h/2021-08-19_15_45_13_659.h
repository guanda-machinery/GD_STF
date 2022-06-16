#pragma once
#include "Header.h"
#pragma warning(disable:4996)
#define SHAREMEM_NAME "BAVT"
/// <summary>
/// ���o DeviceIoControl
/// </summary>
/// <param name="hLmsensor"> ����</param>
/// <param name="ResourceTypeID">�ӷ� id</param>
/// <param name="ObjectCount">�������</param>
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
///// <summary>
///// ���o DeviceIoControl
///// </summary>
///// <param name="hLmsensor"> ����</param>
///// <param name="ResourceTypeID">�ӷ� id</param>
///// <param name="ObjectCount">�������</param>
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
//				printf("����....\n");
//				return;
//			}
//			if (!DeviceIoControl(hLmsensor, IOCTL_LMSENSOR_GET_VALUES, &resourceTypeID, sizeof(resourceTypeID), pValues, retValue, pRetSize, NULL))
//			{
//				printf("����....\n");
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
BOOL LocalSysCreateNamedShm(TCHAR szName[], HANDLE* phShmHandle, void** ppUserSpace, unsigned long ulSize)
{
	*phShmHandle = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, ulSize, szName);
	if (phShmHandle == NULL)
	{
		printf("���~");
	}
	*ppUserSpace = MapViewOfFile((HANDLE)*phShmHandle, FILE_MAP_WRITE, 0, 0, 0);

	return true;
}