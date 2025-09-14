#include "BattleAtaho.h"
#include "wbTexture.h"
#include "wbAnimator.h"
#include "wbResources.h"

namespace wb
{
	BattleAtaho::BattleAtaho() : mAnimator(nullptr)
	{
		Texture* atahoTex = Resources::Find<Texture>(L"BATTLE_AT");
		mAnimator = AddComponent<Animator>();

		atahoTex = Resources::Find<Texture>(L"ATAHO_BATTLE_IDLE");
		mAnimator->CreateAnimation(L"ATAHO_BATTLE_IDLE", atahoTex, Vector2(0.0f, 0.0f), Vector2(45.0f, 63.0f), Vector2::Zero, 1, 1.0f);


		atahoTex = Resources::Find<Texture>(L"ATAHO_BATTLE_PUNCH");
		mAnimator->CreateAnimation(L"ATAHO_BATTLE_PUNCH", atahoTex, Vector2(0.0f, 0.0f), Vector2(67.5f, 61.0f), Vector2::Zero, 2, 1.0f);

		atahoTex = Resources::Find<Texture>(L"ATAHO_BATTLE_ROTATEKICK");
		mAnimator->CreateAnimation(L"ATAHO_BATTLE_ROTATEKICK", atahoTex, Vector2(0.0f, 0.0f), Vector2(70.0f, 101.0f), Vector2::Zero, 4, 1.0f);

		atahoTex = Resources::Find<Texture>(L"ATAHO_BATTLE_SLEEP");
		mAnimator->CreateAnimation(L"ATAHO_BATTLE_SLEEP", atahoTex, Vector2(0.0f, 0.0f), Vector2(62.0f, 49.0f), Vector2::Zero, 3, 1.0f);

		atahoTex = Resources::Find<Texture>(L"ATAHO_BATTLE_GUARD");
		mAnimator->CreateAnimation(L"ATAHO_BATTLE_GUARD", atahoTex, Vector2(0.0f, 0.0f), Vector2(48.0f, 64.0f), Vector2::Zero, 1, 1.0f);

		atahoTex = Resources::Find<Texture>(L"ATAHO_BATTLE_BEATTACKED");
		mAnimator->CreateAnimation(L"ATAHO_BATTLE_BEATTACKED", atahoTex, Vector2(0.0f, 0.0f), Vector2(45.0f, 65.0f), Vector2::Zero, 1, 1.0f);

		atahoTex = Resources::Find<Texture>(L"ATAHO_BATTLE_FLYINGKICK");
		mAnimator->CreateAnimation(L"ATAHO_BATTLE_FLYINGKICK", atahoTex, Vector2(0.0f, 0.0f), Vector2(68.0f, 65.0f), Vector2::Zero, 1, 1.0f);

		atahoTex = Resources::Find<Texture>(L"ATAHO_BATTLE_FAINTING");
		mAnimator->CreateAnimation(L"ATAHO_BATTLE_FAINTING", atahoTex, Vector2(0.0f, 0.0f), Vector2(100.0f, 81.0f), Vector2::Zero, 3, 1.0f);

		atahoTex = Resources::Find<Texture>(L"ATAHO_BATTLE_DRINKING");
		mAnimator->CreateAnimation(L"ATAHO_BATTLE_DRINKING", atahoTex, Vector2(0.0f, 0.0f), Vector2(75.0f, 95.0f), Vector2::Zero, 4, 1.0f);

		atahoTex = Resources::Find<Texture>(L"ATAHO_BATTLE_DRUNKSTATE");
		mAnimator->CreateAnimation(L"ATAHO_BATTLE_DRUNKSTATE", atahoTex, Vector2(0.0f, 0.0f), Vector2(50.0f, 66.0f), Vector2::Zero, 1, 1.0f);

		atahoTex = Resources::Find<Texture>(L"ATAHO_BATTLE_WIN");
		mAnimator->CreateAnimation(L"ATAHO_BATTLE_WIN", atahoTex, Vector2(0.0f, 0.0f), Vector2(69.2f, 186.0f), Vector2::Zero, 13, 1.0f);

		atahoTex = Resources::Find<Texture>(L"ATAHO_BATTLE_LEGKICK");
		mAnimator->CreateAnimation(L"ATAHO_BATTLE_LEGKICK", atahoTex, Vector2(0.0f, 0.0f), Vector2(61.0f, 66.0f), Vector2::Zero, 2, 1.0f);

		atahoTex = Resources::Find<Texture>(L"ATAHO_BATTLE_ROAR");
		mAnimator->CreateAnimation(L"ATAHO_BATTLE_ROAR", atahoTex, Vector2(0.0f, 0.0f), Vector2(100.0f, 144.0f), Vector2::Zero, 4, 1.0f);

		atahoTex = Resources::Find<Texture>(L"ATAHO_BATTLE_HOGYUKGWON");
		mAnimator->CreateAnimation(L"ATAHO_BATTLE_HOGYUKGWON", atahoTex, Vector2(0.0f, 0.0f), Vector2(58.0f, 64.0f), Vector2::Zero, 4, 1.0f);

		atahoTex = Resources::Find<Texture>(L"ATAHO_BATTLE_HOPOGWON");
		mAnimator->CreateAnimation(L"ATAHO_BATTLE_HOPOGWON", atahoTex, Vector2(0.0f, 0.0f), Vector2(80.0f, 91.0f), Vector2::Zero, 3, 1.0f);
	}
	BattleAtaho::~BattleAtaho()
	{
	}
	void BattleAtaho::Initialize()
	{
		GameObject::Initialize();
	}
	void BattleAtaho::Update()
	{
		GameObject::Update();
	}
	void BattleAtaho::LateUpdate()
	{
		GameObject::LateUpdate();
	}
	void BattleAtaho::Render(HDC hdc)
	{
		GameObject::Render(hdc);
	}
}