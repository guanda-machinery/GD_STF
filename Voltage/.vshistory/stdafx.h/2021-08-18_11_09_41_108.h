#pragma once
#include "Header.h"

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