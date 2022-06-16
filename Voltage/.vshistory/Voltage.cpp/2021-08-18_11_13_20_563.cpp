﻿// Voltage.cpp : 此檔案包含 'main' 函式。程式會於該處開始執行及結束執行。
//

#include <iostream>
#include "Header.h"
#include "Header1.h"
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

    if (pVoltage)
    {
        for (int i = 0; i < (int)vCount; i++)
        {
            if (i < 7) // The example only support 7 voltages
            {
                std::cout << pVoltage[i] / 100.00;
            }
            else
            {
                break;
            }

        }
        free(pVoltage);
}


