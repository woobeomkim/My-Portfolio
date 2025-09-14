#pragma once
#include "wbPlayerScript.h"

namespace wb
{
	class Ataho : public PlayerScript
	{
	public:
		Ataho();
		~Ataho();
		void Initialize() override;
		void Update() override;
		void LateUpdate() override;
		void Render(HDC hdc) override;

	private:
		void idle();
		void move();
		void moveCharacter(float newX, float newY);
		void updateFollowerPosition(Vector2 target);

		int mTrailFrame = 0;
	};
}
