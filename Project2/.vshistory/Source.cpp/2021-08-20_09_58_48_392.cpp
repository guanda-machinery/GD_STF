#define CSHARP_MMF_API __declspec(dllexport) //�Ъ`�N�I���T���OExport�n�G�_


extern "C" CSHARP_MMF_API int Add(int a , int b) {
	return a + b;
}