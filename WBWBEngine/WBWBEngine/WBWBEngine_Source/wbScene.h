#pragma once

#include "CommonInclude.h"
#include "wbEntity.h"
#include "wbLayer.h"

namespace wb
{
	class Scene : public Entity
	{
	public:
		Scene();
		~Scene();

		virtual void Initialize();
		virtual void Update();
		virtual void LateUpdate();
		virtual void Render(HDC hdc);
		virtual void Destroy();

		virtual void OnEnter();
		virtual void OnExit();

		void AddGameObject(class GameObject* obj , const eLayerType type);
		void EraseGameObject(GameObject* gameObj);
		Layer* GetLayer(eLayerType type) { return mLayers[(UINT)type]; }

	private:
		void createLayers();
	private:
		std::vector<Layer*> mLayers;
	};
}