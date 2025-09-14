#pragma once
#include "wbGameObject.h"

namespace wb
{
    class wbRect : public GameObject
    {
	public:
		wbRect();
		~wbRect();

		void Initialize() override;
		void Update() override;
		void LateUpdate() override;
		void Render(HDC hdc) override;

	private:
		Vector2 mSize;
    };
}

