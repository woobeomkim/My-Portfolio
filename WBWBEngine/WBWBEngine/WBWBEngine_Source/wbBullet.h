#pragma once
#include "CommonInclude.h"

namespace wb
{
	class Bullet
	{
	public:
		Bullet();
		~Bullet();

		void Initialize();
		void Update();
		void Render(HDC hdc);
		void LateUpdate();

		void Dead()
		{
			mbDead = true;
		}

		bool IsDead()
		{
			return mbDead;
		}

		void SetPosition(float x, float y)
		{
			mX = x;
			mY = y;
		}

		float GetPositionX() { return mX; }
		float GetPositionY() { return mY; }

	private:
		float mSpeed;
		float mX;
		float mY;
		float mSize;
		bool mbDead;
	};
}
