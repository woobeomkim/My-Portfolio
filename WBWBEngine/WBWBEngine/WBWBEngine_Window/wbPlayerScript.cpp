#include "wbPlayerScript.h"
#include "wbInput.h"
#include "wbTime.h"
#include "wbGameObject.h"
#include "wbTransform.h"
#include "wbAnimator.h"
#include "wbPlayer.h"
#include "wbObject.h"

namespace wb
{
    float PlayerScript::moveDeltaTime = 0.0f;

    std::queue<std::pair<Vector2,PlayerScript::eDir>> PlayerScript::prevAtahoPositions = {};
    std::queue<std::pair<Vector2,PlayerScript::eDir>> PlayerScript::prevRinshanPositions = {};
    std::queue<std::pair<Vector2,PlayerScript::eDir>> PlayerScript::prevSmisuPositions = {};
    PlayerScript::PlayerScript()
        : mState(eState::Idle)
        , mAnimator(nullptr)
        , mIsMoving(false)
        , mDir(eDir::None)
        , mSpeed(100.0f)
        , mAccTime(0.0f)
    {
    }
    PlayerScript::~PlayerScript()
    {
    }
    void PlayerScript::Initialize()
    {
    }
    void PlayerScript::Update()
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
    void PlayerScript::LateUpdate()
    {

    }
    void PlayerScript::Render(HDC hdc)
    {
    }
    void PlayerScript::idle()
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

    

    void PlayerScript::move()
    {
        Transform* tr = GetOwner()->GetComponent<Transform>();
        Vector2 pos = tr->GetPosition();
        float newX = pos.x;
		float newY = pos.y;
		moveDeltaTime = Time::DeltaTime();
        if(Input::GetKey(eKeyCode::Left))
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
    void PlayerScript::moveCharacter(float newX, float newY)
    {
		Transform* tr = GetOwner()->GetComponent<Transform>();
		Vector2 pos = tr->GetPosition();
		int order = (int)mOrder;
    }
    void PlayerScript::updateFollowerPosition(Vector2 target)
    {
   
    }

}