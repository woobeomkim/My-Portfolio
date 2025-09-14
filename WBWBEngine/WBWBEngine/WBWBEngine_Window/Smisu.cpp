#include "Smisu.h"
#include "wbAnimator.h"
#include "wbGameObject.h"
#include "wbInput.h"
#include "wbTransform.h"
#include "wbTime.h"

namespace wb
{
	Smisu::Smisu()
	{
	}
	Smisu::~Smisu()
	{
	}
	void Smisu::Initialize()
	{
	}
	void Smisu::Update()
	{
		if (!mAnimator)
		{
			mAnimator = GetOwner()->GetComponent<Animator>();
		}

		switch (mState)
		{
		case wb::PlayerScript::eState::Idle:
			idle();
			break;
		case wb::PlayerScript::eState::Walk:
			move();
			break;
		case wb::PlayerScript::eState::Die:
			break;
		case wb::PlayerScript::eState::Attack:
			break;
		default:
			break;
		}
	}
	void Smisu::LateUpdate()
	{
	}
	void Smisu::Render(HDC hdc)
	{
	}

	void Smisu::move()
	{
		static float moveDeltaTime = 0.0f;
		moveDeltaTime += Time::DeltaTime();
		Transform* tr = GetOwner()->GetComponent<Transform>();
		Vector2 pos = tr->GetPosition();
		if (Input::GetKey(eKeyCode::Left) || Input::GetKey(eKeyCode::Right) || Input::GetKey(eKeyCode::Up) || Input::GetKey(eKeyCode::Down)) mIsMoving = true;
		else mIsMoving = false;
		if (mIsMoving)
		{
			if (!prevRinshanPositions.empty())
			{
				moveDeltaTime = 0.0f;
				if (prevRinshanPositions.size() >= delayFrames)
				{
					Vector2 rinshanPos = prevRinshanPositions.front().first;
					mDir = prevRinshanPositions.front().second;
					prevRinshanPositions.pop();
					pos = rinshanPos;

					tr->SetPosition(pos);
					prevSmisuPositions.push(std::make_pair(pos, mDir));
				}
			}
		}
		if (!mIsMoving)
		{
			switch (mDir)
			{
			case eDir::Left:
				mAnimator->PlayAnimation(L"LEFTWALK_IDLE", true);
				break;
			case eDir::Right:
				mAnimator->PlayAnimation(L"RIGHTWALK_IDLE", true);
				break;
			case eDir::Up:
				mAnimator->PlayAnimation(L"UPWALK_IDLE", true);
				break;
			case eDir::Down:
				mAnimator->PlayAnimation(L"DOWNWALK_IDLE", true);
				break;
			}
			mState = eState::Idle;
		}
	}

	void Smisu::idle()
	{

		if (Input::GetKey(eKeyCode::Left))
		{
			mState = eState::Walk;
			mDir = eDir::Left;
			mAnimator->PlayAnimation(L"LEFTWALK", true);
		}
		else if (Input::GetKey(eKeyCode::Right))
		{
			mState = eState::Walk;
			mDir = eDir::Right;
			mAnimator->PlayAnimation(L"RIGHTWALK", true);
		}
		else if (Input::GetKey(eKeyCode::Up))
		{
			mState = eState::Walk;
			mDir = eDir::Up;
			mAnimator->PlayAnimation(L"UPWALK", true);
		}
		else if (Input::GetKey(eKeyCode::Down))
		{
			mState = eState::Walk;
			mDir = eDir::Down;
			mAnimator->PlayAnimation(L"DOWNWALK", true);
		}


	}

}