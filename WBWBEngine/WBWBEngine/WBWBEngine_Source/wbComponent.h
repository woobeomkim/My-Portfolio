#pragma once
#include "CommonInclude.h"
#include "wbEntity.h"

namespace wb
{
	class GameObject;
	class Component : public Entity
	{
	public:
		Component(eComponentType type);
		~Component();

		virtual void Initialize();
		virtual void Update();
		virtual void LateUpdate();
		virtual void Render(HDC hdc);

		void SetOwner(GameObject* owner) { mOwner = owner; }
		GameObject* GetOwner() { return mOwner; }
		eComponentType GetType() { return mType; }
	private:
		GameObject* mOwner;
		eComponentType mType;
	};
}
