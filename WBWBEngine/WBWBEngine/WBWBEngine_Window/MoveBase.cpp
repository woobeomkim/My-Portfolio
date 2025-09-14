#include "MoveBase.h"

namespace wb
{
	MoveBase::MoveBase(std::wstring moveName, int moveCost, int movePower, int moveRange)
		: mName(moveName)
		, mPower(movePower)
		, mCost(moveCost)
		
	{
	}
	MoveBase::~MoveBase()
	{
	}
}