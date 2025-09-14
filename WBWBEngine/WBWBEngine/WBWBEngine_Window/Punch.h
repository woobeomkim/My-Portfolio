#pragma once
#include "MoveBase.h"

namespace wb
{
    class GameObject;
    class Punch : public MoveBase
    {
    public:
        Punch(std::wstring moveName, int moveCost, int movePower, int moveRange);
        virtual void Execute(GameObject* sourceUnit, GameObject* targetUnit);
    };
}
