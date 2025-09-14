#pragma once
#include "wbScript.h"

namespace wb
{
	class CameraScript : public Script
	{
    public:
        CameraScript();
        ~CameraScript();

        void Initialize() override;
        void Update() override;
        void LateUpdate() override;
        void Render(HDC hdc) override;

    private:
	};
}
