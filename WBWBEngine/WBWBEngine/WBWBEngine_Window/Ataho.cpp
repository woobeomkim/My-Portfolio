#include "Ataho.h"
#include "wbAnimator.h"
#include "wbGameObject.h"
#include "wbInput.h"
#include "wbTransform.h"
#include "wbTime.h"

namespace wb
{
	Ataho::Ataho()
	{
	}
	Ataho::~Ataho()
	{
	}
	void Ataho::Initialize()
	{
	}
	void Ataho::Update()
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
	void Ataho::LateUpdate()
	{
	}
	void Ataho::Render(HDC hdc)
	{
	}
	void Ataho::idle()
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
	void Ataho::move()
	{
		Transform* tr = GetOwner()->GetComponent<Transform>();
		Vector2 pos = tr->GetPosition();
		float newX = pos.x;
		float newY = pos.y;
		moveDeltaTime = Time::DeltaTime();
		if (Input::GetKey(eKeyCode::Left))
		{
			mDir = eDir::Left;
			newX -= mSpeed * moveDeltaTime;
			mIsMoving = true;
		}
		else if (Input::GetKey(eKeyCode::Right))
		{
			mDir = eDir::Right;
			newX += mSpeed * moveDeltaTime;
			mIsMoving = true;
		}
		else if (Input::GetKey(eKeyCode::Up))
		{
			mDir = eDir::Up;
			newY -= mSpeed * moveDeltaTime;
			mIsMoving = true;
		}
		else if (Input::GetKey(eKeyCode::Down))
		{
			mDir = eDir::Down;
			newY += mSpeed * moveDeltaTime;
			mIsMoving = true;
		}
		else
		{
			mIsMoving = false;
		}

		
		moveCharacter(newX, newY);


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
	void Ataho::moveCharacter(float newX, float newY)
	{
		Transform* tr = GetOwner()->GetComponent<Transform>();
		Vector2 pos = tr->GetPosition();
		tr->SetPosition(Vector2(newX, newY));

		if (mTrailFrame >= 1200)
			mTrailFrame = 0;

		static float time = 0.0f;
		time += Time::DeltaTime();

		prevAtahoPositions.push({pos, mDir});
		if (prevAtahoPositions.size() > delayFrames)
			prevAtahoPositions.pop();
	}

	void Ataho::updateFollowerPosition(Vector2 target)
	{
		float speed = 0.1f;
		Transform* tr = GetOwner()->GetComponent<Transform>();
		Vector2 pos = tr->GetPosition();
		pos.x += (target.x - pos.x) * speed;
		pos.y += (target.y - pos.y) * speed;
		
		tr->SetPosition(pos);
	}
}