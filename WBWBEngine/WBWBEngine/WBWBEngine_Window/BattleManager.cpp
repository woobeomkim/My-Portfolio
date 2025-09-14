#include "BattleManager.h"
#include "wbMonster.h"
#include "wbTexture.h"
#include "wbAnimation.h"
#include "wbAnimator.h"
#include "wbObject.h"
#include "wbResources.h"
#include "wbResource.h"
#include "wbPlayer.h"
#include "MoveBase.h"
#include "BattleBase.h"
#include "wbInput.h"
#include "wbTime.h"

namespace wb
{
	std::queue<std::function<void()>> BattleManager::mActionQueue = {};
	std::map<std::wstring, Monster*> BattleManager::mMonster = {};

	BattleManager::BattleState BattleManager::mBattleState;
	BattleManager::BattleAction BattleManager::mBattleAction;
	int BattleManager::currentAction;
	int BattleManager::currentMove;
	bool BattleManager::mIsturn;

	Animator* BattleManager::atahoAnimator = nullptr;
	Animator* BattleManager::monsterAnimator = nullptr;

	Player* BattleManager::mAtaho = nullptr;
	Player* BattleManager::mRinShan = nullptr;
	Player* BattleManager::mSmisu = nullptr;

	BattleManager::BattleManager()
	{
	}
	BattleManager::~BattleManager()
	{
	}
	void BattleManager::Initialize()
	{
		atahoAnimator = mAtaho->GetComponent<Animator>();
		monsterAnimator = mMonster[L"MONKEY"]->GetComponent<Animator>();
	}
	void BattleManager::Update()
	{
		switch (mBattleState)
		{
		case wb::BattleManager::BattleState::Start:
			startBattle();
			break;
		case wb::BattleManager::BattleState::ActionSelection:
			actionSelection();
			break;
		case wb::BattleManager::BattleState::MoveSelection:
			moveSelection();
			break;
		case wb::BattleManager::BattleState::RunningTurn:
			RunTurns();
			break;
		case wb::BattleManager::BattleState::Busy:
			break;
		case wb::BattleManager::BattleState::BattleOver:
			break;
		default:
			break;
		}
	}
	void BattleManager::LateUpdate()
	{
	}
	void BattleManager::Destroy()
	{
	}
	void BattleManager::Release()
	{
	}
	void BattleManager::startBattle()
	{

		mBattleState = BattleState::ActionSelection;
	}
	void BattleManager::actionSelection()
	{
		if (Input::GetKeyDown(eKeyCode::Right))
			currentAction++;
		else if (Input::GetKeyDown(eKeyCode::Left))
			currentAction--;


		if(Input::GetKeyDown(eKeyCode::Z))
			mBattleState = BattleState::MoveSelection;
	}
	void BattleManager::moveSelection()
	{
		if (Input::GetKeyDown(eKeyCode::Down))
			currentMove++;
		else if (Input::GetKeyDown(eKeyCode::Up))
			currentMove--;

		if(Input::GetKeyDown(eKeyCode::Z))
			mBattleState = BattleState::RunningTurn;
		else if(Input::GetKeyDown(eKeyCode::X))
			mBattleState = BattleState::ActionSelection;

	}
	void BattleManager::RunTurns()
	{
		atahoAnimator->PlayAnimation(L"ATAHO_BATTLE_PUNCH", false);
		
		atahoAnimator->PlayAnimation(L"ATAHO_BATTLE_IDLE",false);

			enemyMove();
			if(Input::GetKeyDown(eKeyCode::Z))
			mBattleState = BattleState::Start;
		
	}
	void BattleManager::enemyMove()
	{
		BattleManager::mMonster[L"MONKEY"]->GetComponent<Animator>()->PlayAnimation(L"MONKEY_ATTACK");
	}
	void BattleManager::runMove()
	{
	}
	void BattleManager::battleOver()
	{
	}

}