#pragma once
#include "wbScene.h"

namespace wb
{
	class Monster;
	class BattleAtaho;

    class BattleScene  : public Scene
    {
	public:
		BattleScene();
		~BattleScene();

		void Initialize() override;
		void Update() override;
		void LateUpdate() override;
		void Render(HDC hdc) override;

		virtual void OnEnter() override;
		virtual void OnExit() override;

	private:
		Monster* mMonster1;
		Monster* mMonster2;
		Monster* mMonster3;
		BattleAtaho* mAtaho;
	};
}
