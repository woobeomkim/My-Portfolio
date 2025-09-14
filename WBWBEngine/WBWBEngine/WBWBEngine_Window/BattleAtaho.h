#pragma once
#include "wbGameObject.h"
namespace wb
{
	class Animator;

	class BattleAtaho : public GameObject
	{
	public:
		BattleAtaho();
		~BattleAtaho();
		void Initialize() override;
		void Update() override;
		void LateUpdate() override;
		void Render(HDC hdc) override;

	private:
		Animator* mAnimator;
	};
}

