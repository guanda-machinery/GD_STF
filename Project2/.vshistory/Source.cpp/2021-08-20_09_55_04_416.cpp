#ifdef Project2_EXPORTS //同專案名稱，只是後面固定為_EXPORTS
#define FORCSHARPCALL_API __declspec(dllexport) //請注意！正確的是Export要亮起
#else
#define FORCSHARPCALL_API __declspec(dllimport)
#endif


