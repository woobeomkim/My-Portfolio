#pragma once
#include "wbGameObject.h"

namespace wb
{
	class Transform;
    class Player : public GameObject
    {
	public:
		Player();
		~Player();

		void Initialize() override;
		void Update() override;
		void LateUpdate() override;
		void Render(HDC hdc) override;

		void SetLeader(Transform* tr)
		{
			mLeader = tr;
		}

		Transform* GetLeader()
		{
			return mLeader;
		}

	private:
		Transform* mLeader;
    };
}
