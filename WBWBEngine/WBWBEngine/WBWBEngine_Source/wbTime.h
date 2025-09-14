#pragma once
#include "CommonInclude.h"

namespace wb
{
	class Time
	{
	public:
		Time();
		~Time();

		static void Initialize();
		static void Update();
		static void Render(HDC hdc);

		static void WaitForSeconds(float seconds);

		__forceinline static float DeltaTime() { return deltaTime; }
		static float GetFPS() { return FPS; }

	private:
		static LARGE_INTEGER cpuFrequency;
		static LARGE_INTEGER prevCounter;
		static LARGE_INTEGER curCounter;
		static float deltaTime;
		static int FPS;
	};
}
