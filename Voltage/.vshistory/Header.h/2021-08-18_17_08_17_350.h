#pragma once
#include <wtypes.h>
#include <minwinbase.h>
#include <atlbase.h>
#include <winnt.h>
#include <atlstr.h>
#include <WinBase.h>
#include <WtsApi32.h>
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
