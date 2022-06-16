// Voltage.cpp : 此檔案包含 'main' 函式。程式會於該處開始執行及結束執行。
//

#include <iostream>
#include "Header.h"

int main()
{
    std::cout << "Hello World!\n";
    HANDLE hLmsensor;
    hLmsensor = CreateFile(_T("\\\\.\\AdvLmDev"),
        GENERIC_WRITE,
        FILE_SHARE_WRITE,
        NULL,
        OPEN_EXISTING,
        0,
        NULL);
    ULONG retSize = 0;
    CString displayStr;
    ULONG vCount = 0;

    PULONG pVoltage = GetValues(hLmsensor, ResourceTypeVoltage, &vCount);
}
// 執行程式: Ctrl + F5 或 [偵錯] > [啟動但不偵錯] 功能表
// 偵錯程式: F5 或 [偵錯] > [啟動偵錯] 功能表

// 開始使用的提示: 
//   1. 使用 [方案總管] 視窗，新增/管理檔案
//   2. 使用 [Team Explorer] 視窗，連線到原始檔控制
//   3. 使用 [輸出] 視窗，參閱組建輸出與其他訊息
//   4. 使用 [錯誤清單] 視窗，檢視錯誤
//   5. 前往 [專案] > [新增項目]，建立新的程式碼檔案，或是前往 [專案] > [新增現有項目]，將現有程式碼檔案新增至專案
//   6. 之後要再次開啟此專案時，請前往 [檔案] > [開啟] > [專案]，然後選取 .sln 檔案


static PULONG GetValues(HANDLE hLmsensor, ULONG ResourceTypeID, PULONG ObjectCount)
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