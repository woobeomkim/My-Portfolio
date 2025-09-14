#include "wbTime.h"

namespace wb
{
	LARGE_INTEGER Time::cpuFrequency = {};
	LARGE_INTEGER Time::prevCounter = {};
	LARGE_INTEGER Time::curCounter = {};
	float Time::deltaTime = 0.0f;
	int Time::FPS = 0;

	Time::Time()
	{
	}
	Time::~Time()
	{
	}
	void Time::Initialize()
	{
		QueryPerformanceFrequency(&cpuFrequency);
		QueryPerformanceCounter(&prevCounter);
	}
	void Time::Update()
	{
		QueryPerformanceCounter(&curCounter);

		float diff = static_cast<float>(curCounter.QuadPart - prevCounter.QuadPart);

		deltaTime = diff / static_cast<float>(cpuFrequency.QuadPart);

		FPS = 1.0f / deltaTime;
		prevCounter = curCounter;
	}

	void Time::Render(HDC hdc)
	{
		wchar_t str[100] = {};
		swprintf_s(str, L"DT : %f FPS : %d", deltaTime, FPS);
		TextOut(hdc, 10, 10, str, wcslen(str));
	}
	void Time::WaitForSeconds(float seconds)
	{
		float currentTime = 0.0f;

		while (currentTime <= seconds)
		{
			currentTime += Time::DeltaTime();
		}
	}
}