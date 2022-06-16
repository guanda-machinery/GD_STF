#pragma once
#include "Header.h"

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

//BOOL LocalSysCreateNamedShm(char* szName, unsigned long ulSize, HANDLE* phShmHandle, void** ppUserSpace)
//{
//	HANDLE	hProcess;
//	unsigned long lMinimumWorkingSetSize;
//	unsigned long lMaximumWorkingSetSize;
//	// �H�U���p�ݭn����...�G
//	// - �B��ɧ@���t�ΪA�ȹB��]plcwinnt ���q�{�]�m�^
//	// - �B��ɷQ�n�P���b�t�αb��U�B�檺�i�{�q�H
//	// -> �|�]�����P���R�W�Ŷ��ӥ��ѡA�ܤ֦b Windows Vista ���I
//	// �q Vista �U���A�ȱҰʪ� Targetvisu �X�{���D
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