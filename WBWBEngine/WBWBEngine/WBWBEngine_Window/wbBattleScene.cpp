#include "wbBattleScene.h"
#include "wbGameObject.h"
#include "wbTransform.h"
#include "wbSpriteRenderer.h"
#include "wbPlayer.h"
#include "wbObject.h"
#include "wbTexture.h"
#include "wbResource.h"
#include "wbResources.h"
#include "BattleAtaho.h"
#include "wbAnimation.h"
#include "wbAnimator.h"
#include "wbMonster.h"
#include "..\\WBWBEngine_Window\\BattleAtaho.h"
#include "BattleManager.h"
#include "..\\WBWBEngine_Source\\wbTime.h"
#include "wbInput.h"
namespace wb
{
	BattleScene::BattleScene()
	{
	}
	BattleScene::~BattleScene()
	{
	}
	void BattleScene::Initialize()
	{
		Scene::Initialize();
	}
	void BattleScene::Update()
	{
		//// 1/4 ?? 160 80

		//Animator* atahoAnimator = BattleManager::GetAtaho()->GetComponent<Animator>();
		//atahoAnimator->PlayAnimation(L"ATAHO_BATTLE_FAINTING");
		//Time::WaitForSeconds(10.0f);
		////atahoAnimator->PlayAnimation(L"BATTLE_IDLE");
		//Time::WaitForSeconds(1.0f);
		//

		if (Input::GetKeyDown(eKeyCode::A))
		{
			Transform* mTr = mMonster1->GetComponent<Transform>();
			Transform* aTr = mAtaho->GetComponent<Transform>();
			Vector2 mPos = mTr->GetPosition();
			Vector2 aPos = aTr->GetPosition();
			
			Vector2 targetPos = mPos - aPos;

			targetPos += aPos;
			aTr->SetPosition(Vector2(targetPos.x - 40.0f, targetPos.y));
		
			mAtaho->GetComponent<Animator>()->PlayAnimation(L"ATAHO_BATTLE_PUNCH", true);
			mMonster1->GetComponent<Animator>()->PlayAnimation(L"MONKEY_BE_ATTACK", true);
		}

		Scene::Update();
		BattleManager::Update();
	}
	void BattleScene::LateUpdate()
	{
		BattleManager::LateUpdate();
	}
	void BattleScene::Render(HDC hdc)
	{
		Scene::Render(hdc);
	}
	void BattleScene::OnEnter()
	{
		mAtaho = Instantiate<BattleAtaho>(eLayerType::Player, Vector2(160.0f, 120.0f));
		mAtaho->GetComponent<Animator>()->PlayAnimation(L"ATAHO_BATTLE_IDLE", true);

		mMonster1 = Instantiate<Monster>(eLayerType::Player, Vector2(540.0f, 240.0f));
		mMonster1->GetComponent<Animator>()->PlayAnimation(L"MONKEY_IDLE", true);
	}
	void BattleScene::OnExit()
	{

	}
}