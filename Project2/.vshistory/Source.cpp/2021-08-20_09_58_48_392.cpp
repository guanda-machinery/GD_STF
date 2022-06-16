#define CSHARP_MMF_API __declspec(dllexport) //請注意！正確的是Export要亮起


extern "C" CSHARP_MMF_API int Add(int a , int b) {
	return a + b;
}