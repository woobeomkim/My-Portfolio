#pragma once
#include "CommonInclude.h"

namespace wb
{
	class GameObject;
	class MoveBase
	{
	public:
		MoveBase(std::wstring moveName, int moveCost, int movePower,int moveRange);
		~MoveBase();

		virtual void Execute(GameObject* sourceUnit,GameObject* targetUnit) = 0;

		void SetName(const std::wstring& name)
		{
			mName = name;
		}

		std::wstring& GetName()
		{
			return mName;
		}
		int GetPower()
		{
			return mPower;

		}

		void SetPower(int power)
		{
			mPower = power;
		}

		void SetEntire(bool isEntire)
		{
			mbEntire = isEntire;
		}

		bool GetEnitre()
		{
			return mbEntire;
		}

		void SetAccuracy(int accuracy)
		{
			mAccuracy = accuracy;
		}

		int GetAccuracy()
		{
			return mAccuracy;
		}

		void SetPriority(int priority)
		{
			mPriority = priority;
		}

		int GetPriority()
		{
			return mPriority;
		}


	protected:
		std::wstring mName;
		int mCost;
		int mPower;
		bool mbEntire;
		int mAccuracy;
		int mPriority;

	};
}
