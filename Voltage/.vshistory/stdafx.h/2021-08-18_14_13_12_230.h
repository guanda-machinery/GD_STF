#pragma once
#include "Header.h"

/// <summary>
/// 取得 DeviceIoControl
/// </summary>
/// <param name="hLmsensor"> 指標</param>
/// <param name="ResourceTypeID">來源 id</param>
/// <param name="ObjectCount">物件指標</param>
/// <returns></returns>
PULONG GetValues(HANDLE hLmsensor, ULONG ResourceTypeID, PULONG ObjectCount)
{
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

BOOL LocalSysCreateNamedShm(char* szName, unsigned long ulSize, HANDLE* phShmHandle, void** ppUserSpace) 
{
    HANDLE	hProcess;
    unsigned long lMinimumWorkingSetSize;
    unsigned long lMaximumWorkingSetSize;

    char szNameBuffer[MAX_PATH];
    char szGlobalPrefix[] = "Global\\";
    char szSessionPrefix[] = "Session\\";
    strcpy(szNameBuffer, szGlobalPrefix);
    strcat(szNameBuffer, szName);

    *phShmHandle = OpenFileMapping(FILE_MAP_WRITE, TRUE, szNameBuffer);
    if (*phShmHandle == 0)
        *phShmHandle = OpenFileMapping(FILE_MAP_WRITE, TRUE, szName);
}

