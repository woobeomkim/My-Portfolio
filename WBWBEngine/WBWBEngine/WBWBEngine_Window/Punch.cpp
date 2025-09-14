#include "Punch.h"
#include "wbGameObject.h"
#include "wbAnimator.h"

namespace wb
{
	Punch::Punch(std::wstring moveName, int moveCost, int movePower, int moveRange)
		:MoveBase(moveName,moveCost,movePower,moveRange)
	{
	}
	void Punch::Execute(GameObject* sourceUnit, GameObject* targetUnit)
	{
		Animator* sourceAnimator = sourceUnit->GetComponent<Animator>();
		sourceAnimator->PlayAnimation(L"ATAHO_BATTLE_PUNCH", false);
	}
}