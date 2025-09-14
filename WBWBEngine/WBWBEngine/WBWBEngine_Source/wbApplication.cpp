#include "wbApplication.h"
#include "wbGameObject.h"
#include "wbInput.h"
#include "wbMonster.h"
#include "wbTime.h"
#include "wbSceneManager.h"
#include "wbResources.h"
#include "wbCollisionManager.h"

namespace wb
{
	Application::Application()
		: mHwnd(nullptr)
		, mHdc(nullptr)
		, mBackHdc(nullptr)
		, mBackBitmap(nullptr)
		, mWidth(0)
		, mHeight(0)
	{
	}

	Application::~Application()
	{
	}

	void Application::Initialize(HWND hwnd, UINT width, UINT height)
	{
		// 코드정리
		adjustWindowRect(hwnd ,width, height);
		createBackBuffer();

		Input::Initialize();
		Time::Initialize();
		CollisionManager::Initialize();
		SceneManager::Initialize();
	}

	void Application::Update()
	{
		Input::Update();
		Time::Update();
		CollisionManager::Update();
		SceneManager::Update();
	}

	void Application::Render()
	{
		Rectangle(mBackHdc, -1, -1, mWidth, mHeight);
		Time::Render(mBackHdc);
		CollisionManager::Render(mBackHdc);
		SceneManager::Render(mBackHdc);

		BitBlt(mHdc, 0, 0, mWidth, mHeight, mBackHdc, 0, 0, SRCCOPY);
	}

	void Application::LateUpdate()
	{
		Input::LateUpdate();
		CollisionManager::LateUpdate();
		SceneManager::LateUpdate();
	}

	void Application::Run()
	{
		Update();
		Render();
		LateUpdate();

		Destroy();
	}

	void Application::Destroy()
	{
		SceneManager::Destroy();
	}

	void Application::Release()
	{
		SceneManager::Release();
		Resources::Release();
	}

	void Application::adjustWindowRect(HWND hwnd ,UINT width, UINT height)
	{
		mHwnd = hwnd;
		mHdc = GetDC(hwnd);

		RECT rect = { 0,0,width,height };
		AdjustWindowRect(&rect, WS_OVERLAPPEDWINDOW, false);

		mWidth = rect.right - rect.left;
		mHeight = rect.bottom - rect.top;

		SetWindowPos(mHwnd, nullptr, 0, 0, mWidth, mHeight, 0);
		ShowWindow(mHwnd, true);
	}

	void Application::createBackBuffer()
	{
		mBackBitmap = CreateCompatibleBitmap(mHdc, mWidth, mHeight);
		mBackHdc = CreateCompatibleDC(mHdc);

		HBITMAP oldBitmap = (HBITMAP)SelectObject(mBackHdc, mBackBitmap);
		DeleteObject(oldBitmap);

	}
	
}
