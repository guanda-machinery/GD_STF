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

BOOL LocalSysCreateNamedShm(char* szName, unsigned long ulSize, HANDLE* phShmHandle, void** ppUserSpace)
{
	HANDLE hProcess;

	unsigned long iMinSize;
	unsigned long iMaxSize;
	/* �H�U���p�ݭn����...�G
	 - �B��ɧ@���t�ΪA�ȹB��]plcwinnt ���q�{�]�m�^
	 - �B��ɷQ�n�P���b�t�αb��U�B�檺�i�{�q�H
	 -> �|�]�����P���R�W�Ŷ��ӥ��ѡA�ܤ֦b Windows Vista ���I
	 �q Vista �U���A�ȱҰʪ� Targetvisu �X�{���D*/
	char szNameBuffer[MAX_PATH];
	char szGlobalPrefix[] = "Global\\";

	strcpy(szNameBuffer, szGlobalPrefix);
	strcat(szNameBuffer, szName);
	//����A�M���ˬd���a�W��
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