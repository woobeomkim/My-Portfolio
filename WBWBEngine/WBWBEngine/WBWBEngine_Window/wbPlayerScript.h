#pragma once
#include "wbScript.h"
#include "CommonInclude.h"

namespace wb
{
	class Player;
	class PlayerScript : public Script
	{
	public:
		enum class eState
		{
			Idle,
			Walk,
			Die,
			Attack
		};

		enum class eDir
		{
			Left,
			Right,
			Up,
			Down,
			None,
		};
		PlayerScript();
		~PlayerScript();

		void Initialize() override;
		void Update() override;
		void LateUpdate() override;
		void Render(HDC hdc) override;

		void SetOrder(ePlayerOrder order)
		{
			mOrder = order;
		}

		ePlayerOrder GetOrder()
		{
			return mOrder;
		}

		eDir GetDir()
		{
			return mDir;
		}

	private:
		void idle();
		void move();
		void moveCharacter(float newX, float newY);
		void updateFollowerPosition(Vector2 target);
	protected:
		ePlayerOrder mOrder;
		float mAccTime;
		static float moveDeltaTime;
		const float mDiffHeight = 64.0f;
		const float mDiffWidth = 48.0f;
		const float delayFrames = 3000.0f;
		float mSpeed;
		eState mState;
		class Animator* mAnimator;
		bool mIsMoving;
		eDir mDir;
		static std::queue<std::pair<Vector2,eDir>> prevAtahoPositions;
		static std::queue<std::pair<Vector2,eDir>> prevRinshanPositions;
		static std::queue<std::pair<Vector2,eDir>> prevSmisuPositions;


	};
}
