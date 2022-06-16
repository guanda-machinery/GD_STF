#pragma once
#include <iostream>
#include <wtypes.h>
#include <minwinbase.h>
#include <atlbase.h>
#include <winnt.h>
#include <atlstr.h>
#include <WinBase.h>
#include <WtsApi32.h>
#include <Windows.h>
#include <Windowsx.h>
#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#define AD_SYS_LMSENSOR_PATH    (_T("SYSTEM\\CurrentControlSet\\Services\\AdvLmsensor"))
#define AD_INFO_ITEM_LEN    60

#define FILE_DEVICE_LMSENSOR        FILE_DEVICE_UNKNOWN
#define IOCTL_LMSENSOR_GET_TEMP \
CTL_CODE( FILE_DEVICE_LMSENSOR, 0x900, METHOD_BUFFERED, FILE_WRITE_ACCESS )
#define IOCTL_LMSENSOR_GET_VOLT \
CTL_CODE( FILE_DEVICE_LMSENSOR, 0x901, METHOD_BUFFERED, FILE_WRITE_ACCESS )
#define IOCTL_LMSENSOR_GET_CHIPINFO \
CTL_CODE( FILE_DEVICE_LMSENSOR, 0x903, METHOD_BUFFERED, FILE_ANY_ACCESS)
#define IOCTL_LMSENSOR_GET_CHIPINFO_EX \
CTL_CODE( FILE_DEVICE_LMSENSOR, 0x904, METHOD_BUFFERED, FILE_ANY_ACCESS)
#define IOCTL_LMSENSOR_GET_CURRENT \
CTL_CODE( FILE_DEVICE_LMSENSOR, 0x906, METHOD_BUFFERED, FILE_ANY_ACCESS )
#define IOCTL_LMSENSOR_GET_POWER \
CTL_CODE( FILE_DEVICE_LMSENSOR, 0x907, METHOD_BUFFERED, FILE_ANY_ACCESS )

#define IOCTL_LMSENSOR_GET_LABELS \
CTL_CODE( FILE_DEVICE_LMSENSOR, 0x920, METHOD_BUFFERED, FILE_ANY_ACCESS )
#define IOCTL_LMSENSOR_GET_VALUES \
CTL_CODE( FILE_DEVICE_LMSENSOR, 0x921, METHOD_BUFFERED, FILE_ANY_ACCESS )

#define ResourceTypeGeneric             0   // None
#define ResourceTypeVoltage             1   // Voltages
#define ResourceTypeCPU                 2   // CPU temperature
#define ResourceTypeBoard               3   // Board temperature
#define ResourceTypeTemperature         4   // Temperatures
#define ResourceTypeFanSpeed            5   // Fan Speeds
#define ResourceTypeCurrent             6   // Current
#define ResourceTypePower               7   // Power

#define _T(x)       __T(x)

/* help functions for shared memory on windows vista */
typedef struct _COPIED_WTS_SESSION_INFO {
	DWORD SessionId;
	LPSTR pWinStationName;		/* A-function, so we have a char-string */
	DWORD dummy;					/* originally an enum */
} COPIED_WTS_SESSION_INFO, * PCOPIED_WTS_SESSION_INFO;


//#if !defined(AFX_STDAFX_H__C95373A2_61B4_46E2_9DD6_4CEB374E69A7__INCLUDED_)
//#define AFX_STDAFX_H__C95373A2_61B4_46E2_9DD6_4CEB374E69A7__INCLUDED_
//
//#if _MSC_VER > 1000
//#pragma once
//#endif // _MSC_VER > 1000
//
//#define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers
//
//#include <stdio.h>
//
//// TODO: reference additional headers your program requires here
//
////{{AFX_INSERT_LOCATION}}
//// Microsoft Visual C++ will insert additional declarations immediately before the previous line.
//
//#endif // !defined(AFX_STDAFX_H__C95373A2_61B4_46E2_9DD6_4CEB374E69A7__INCLUDED_)