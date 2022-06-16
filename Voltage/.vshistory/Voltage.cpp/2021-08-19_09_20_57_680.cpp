﻿// Voltage.cpp : 此檔案包含 'main' 函式。程式會於該處開始執行及結束執行。
//

#include <iostream>
#include "Header.h"
#include "stdafx.h"
int main()
{
	HANDLE hLmsensor;
	hLmsensor = CreateFile(_T("\\\\.\\AdvLmDev"),
		GENERIC_WRITE,
		FILE_SHARE_WRITE,
		NULL,
		OPEN_EXISTING,
		0,
		NULL);
	//ULONG retSize = 0;
	ULONG vCount = 0;
	PULONG pVoltage ;
	
	while (true)
	{
		 pVoltage = GetValues(hLmsensor, ResourceTypeVoltage, &vCount);
		if (pVoltage)
		{
			printf("地址：", &pVoltage[0]);
			printf("+BAVT：%6.2f\n", pVoltage[0]/ 100.00);
			Sleep(1);
		}
	}
}
