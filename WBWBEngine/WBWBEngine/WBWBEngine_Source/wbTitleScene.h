#pragma once
#include "wbScene.h"

namespace wb
{
    class TitleScene : public Scene
    {
    public:
        enum class ScreenState
        {
            BlackScreen,
            TitleScreen,
        };
        TitleScene();
        ~TitleScene();

        void Initialize() override;
        void Update() override;
        void LateUpdate() override;
        void Render(HDC hdc) override;

        void OnEnter() override;
        void OnExit() override;
    private:
        float accTime = 0.0f;
        ScreenState mState = ScreenState::TitleScreen;
        class Texture* mTex = nullptr;
        class GameObject* mBg = nullptr;
        class SpriteRenderer* mSr = nullptr;
    };
}

