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
	if (NULL == ObjectCount) return NULL;

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
/// <summary>
/// ���o DeviceIoControl
/// </summary>
/// <param name="hLmsensor"> ����</param>
/// <param name="ResourceTypeID">�ӷ� id</param>
/// <param name="ObjectCount">�������</param>
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
				printf("����....\n");
				return;
			}
			if (!DeviceIoControl(hLmsensor, IOCTL_LMSENSOR_GET_VALUES, &resourceTypeID, sizeof(resourceTypeID), pValues, retSize, &retSize, NULL))
			{
				printf("����....\n");
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
BOOL LocalSysCreateNamedShm(TCHAR szName[], PULONG* value)
{
	HANDLE hMapFile = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, sizeof(long), szName);
}