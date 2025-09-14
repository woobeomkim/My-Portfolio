#include "RinShan.h"
#include "wbAnimator.h"
#include "wbGameObject.h"
#include "wbInput.h"
#include "wbTransform.h"
#include "wbTime.h"

namespace wb
{
	RinShan::RinShan()
	{
	}
	RinShan::~RinShan()
	{
	}
	void RinShan::Initialize()
	{
	}
	void RinShan::Update()
	{

		//static float moveDeltaTime = 0.0f;
		//moveDeltaTime += Time::DeltaTime();

		//if (!prevAtahoPositions.empty() && moveDeltaTime > 0.5F)
		//{
		//	moveDeltaTime = 0.0f;
		//	Vector2 atahoPos = prevAtahoPositions.front().f;
		//	prevAtahoPositions.pop();
		//	Transform* tr = GetOwner()->GetComponent<Transform>();
		//	tr->SetPosition(atahoPos);
		//}

		if (!mAnimator)
		{
			mAnimator = GetOwner()->GetComponent<Animator>();
		}

		//PlayAnimation(mDir);
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
	void RinShan::LateUpdate()
	{
	}
	void RinShan::Render(HDC hdc)
	{
	}
	void RinShan::PlayAnimation(eDir dir)
	{
		switch (dir)
		{
		case eDir::Left:
			mAnimator->PlayAnimation(L"LEFTWALK", true);
			break;
		case eDir::Right:
			mAnimator->PlayAnimation(L"RIGHTWALK", true);
			break;
		case eDir::Up:
			mAnimator->PlayAnimation(L"UPWALK", true);
			break;
		case eDir::Down:
			mAnimator->PlayAnimation(L"DOWNWALK", true);
			break;
		}
	}
	void RinShan::idle()
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
		mIsMoving = true;

	}
	void RinShan::move()
	{
		static float moveDeltaTime = 0.0f;
		moveDeltaTime += Time::DeltaTime();
		Transform* tr = GetOwner()->GetComponent<Transform>();
		Vector2 pos = tr->GetPosition();

		if (Input::GetKey(eKeyCode::Left) || Input::GetKey(eKeyCode::Right) || Input::GetKey(eKeyCode::Up) || Input::GetKey(eKeyCode::Down)) mIsMoving = true;
		else mIsMoving = false;
		if (mIsMoving)
		{
			if (!prevAtahoPositions.empty())
			{
				
					moveDeltaTime = 0.0f;

					if (prevAtahoPositions.size() >= delayFrames)
					{
						Vector2 atahoPos = prevAtahoPositions.front().first;
						mDir = prevAtahoPositions.front().second;
						prevAtahoPositions.pop();
						pos = atahoPos;
						tr->SetPosition(pos);
						prevRinshanPositions.push(std::make_pair(pos, mDir));
					}
			}
		}

		if (!mIsMoving)
		{
			switch (mDir)
			{
			case eDir::Left:
				mAnimator->PlayAnimation(L"LEFTWALK_IDLE", false);
				break;
			case eDir::Right:
				mAnimator->PlayAnimation(L"RIGHTWALK_IDLE", false);
				break;
			case eDir::Up:
				mAnimator->PlayAnimation(L"UPWALK_IDLE", false);
				break;
			case eDir::Down:
				mAnimator->PlayAnimation(L"DOWNWALK_IDLE", false);
				break;
			}
			mState = eState::Idle;
		}
	}
}