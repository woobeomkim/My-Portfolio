#pragma once
#include "wbScene.h"

namespace wb
{
    class EndingScene : public Scene
    {
	public:
		EndingScene();
		~EndingScene();

		void Initialize() override;
		void Update() override;
		void LateUpdate() override;
		void Render(HDC hdc) override;

		virtual void OnEnter() override;
		virtual void OnExit() override;
    };
}
