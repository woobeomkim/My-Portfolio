#pragma once
#include "wbGameObject.h"
#include "CommonInclude.h"

namespace wb
{
	class Monster : public GameObject
	{
	public:
		Monster();
		~Monster();

		void Initialize();
		void Update();
		void Render(HDC hdc);
		void LateUpdate();

	private:
		class Animator* mAnimator;
	};
}
