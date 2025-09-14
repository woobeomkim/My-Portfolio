#pragma once
#include "CommonInclude.h"

namespace wb
{
	class Move;
	class BattleBase
	{
	public:
		struct Stat
		{
			int maxHp;
			int attack;
			int defense;
			int spAttack;
			int speed;

			Stat()
			{
				maxHp = 0;
				attack = 0;
				defense = 0;
				spAttack= 0;
				speed = 0;
			}
		};

		BattleBase();
		~BattleBase();

		void AddMove(std::wstring moveName , Move* move)
		{
			mLearnalbeMove.insert(std::make_pair(moveName, move));
		}

		Move* GetMove(std::wstring moveName)
		{
			return mLearnalbeMove[moveName];
		}

	private:
		std::wstring mName;
		std::map<std::wstring,Move*> mLearnalbeMove;
		Stat mStat;
	};
}

