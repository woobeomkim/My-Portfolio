#pragma once
#include "CommonInclude.h"

namespace wb
{
	class Application
	{
	public:
		Application();
		~Application();

		static Application& GetInstance()
		{
			static Application instance;
			return instance;
		}
		void Initialize(HWND hwnd, UINT width, UINT height);
		void Update();
		void Render();
		void LateUpdate();
		
		void Destroy();
		void Release();

		__forceinline UINT GetWidth() { return mWidth; }
		__forceinline UINT GetHeight() { return mHeight; }

		HDC GetHdc() { return mHdc; }
		HWND GetHwnd() { return mHwnd; }

		void Run();

	private:
		void adjustWindowRect(HWND hwnd,UINT width, UINT height);
		void createBackBuffer();
	private:
		HWND mHwnd;
		HDC mHdc;

		HDC mBackHdc;
		HBITMAP mBackBitmap;

		UINT mWidth;
		UINT mHeight;
	};

}