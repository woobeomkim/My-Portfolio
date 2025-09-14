#pragma once
#include "wbPlayerScript.h"

namespace wb
{
	class Smisu : public PlayerScript
	{
	public:
		Smisu();
		~Smisu();
		void Initialize() override;
		void Update() override;
		void LateUpdate() override;
		void Render(HDC hdc) override;

	private:
		void idle();
		void move();
	};
}
