#pragma once
#include "CommonInclude.h"

namespace wb
{
	class Player;
	class Animator;
	class BattleManager
	{
	public:
		enum class BattleState
		{
			Start,
			ActionSelection,
			MoveSelection,
			RunningTurn,
			Busy,
			BattleOver
		};

		enum class BattleAction
		{
			Move,
			Run
		};
		BattleManager();
		~BattleManager();

		static void Initialize();
		static void Update();
		static void LateUpdate();
		static void Destroy();
		static void Release();

		static std::queue<std::function<void()>> mActionQueue;

		static Player* GetAtaho() { return mAtaho; }

	private:
		static void startBattle();
		static void actionSelection();
		static void moveSelection();
		static void RunTurns();
		static void enemyMove();
		static void runMove();
		static void battleOver();

	public:
		static BattleState mBattleState;
		static BattleAction mBattleAction;
		static int currentAction;
		static int currentMove;
		static bool mIsturn;
		static std::map<std::wstring, class Monster*> mMonster;
		static Animator* atahoAnimator;
		static Animator* monsterAnimator;

		static Player* mAtaho;
		static Player* mRinShan;
		static Player* mSmisu;
	};
}
