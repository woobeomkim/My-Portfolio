#pragma once
#include "wbPlayerScript.h"

namespace wb
{
	class RinShan : public PlayerScript
	{
	public:
		RinShan();
		~RinShan();
		void Initialize() override;
		void Update() override;
		void LateUpdate() override;
		void Render(HDC hdc) override;

		void PlayAnimation(eDir dir);

	private:
		void idle();
		void move();
	};
}
