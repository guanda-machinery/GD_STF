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
	PULONG pVoltage;
	if (!pVoltage)
	{

	}
	while (true)
	{
		GetValues(hLmsensor, ResourceTypeVoltage, &vCount, &pVoltage);
		if (pVoltage)
		{
			printf("地址：%p \n", &pVoltage);
			printf("+BAVT：%6.2f\n", pVoltage[0] / 100.00);
			Sleep(1000);
		}
	}
}
