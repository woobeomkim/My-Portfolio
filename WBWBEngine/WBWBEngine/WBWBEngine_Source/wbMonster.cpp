#include "wbMonster.h"
#include "..\\WBWBEngine_Window\\BattleBase.h"
#include "wbTexture.h"
#include "wbResources.h"
#include "wbAnimator.h"
namespace wb
{
	Monster::Monster()
	{
		Texture* tex = Resources::Find<Texture>(L"MONKEY");
		mAnimator = AddComponent<Animator>();
		mAnimator->CreateAnimation(L"MONKEY_IDLE", tex, Vector2(0, 0), Vector2(40.0f, 64.0f), Vector2::Zero, 1, 0.1f);
		mAnimator->CreateAnimation(L"MONKEY_ATTACK", tex, Vector2(41.0f, 0), Vector2(70.0f, 64.0f), Vector2::Zero, 1, 0.1f);
		mAnimator->CreateAnimation(L"MONKEY_BE_ATTACK", tex, Vector2(110.0f, 0), Vector2(40.0f, 64.0f), Vector2::Zero, 1, 0.1f);
	}
	Monster::~Monster()
	{
	}
	void Monster::Initialize()
	{
		GameObject::Initialize();
	}
	void Monster::Update()
	{
		GameObject::Update();
	}
	void Monster::Render(HDC hdc)
	{
		GameObject::Render(hdc);
	}
	void Monster::LateUpdate()
	{
		GameObject::LateUpdate();
	}
}