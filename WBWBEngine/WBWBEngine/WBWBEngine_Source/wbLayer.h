#pragma once
#include "wbEntity.h"
#include "CommonInclude.h"
#include "wbGameObject.h"

namespace wb
{
	class Layer : public Entity
	{
	public:
		Layer();
		~Layer();

		virtual void Initialize();
		virtual void Update();
		virtual void LateUpdate();
		virtual void Render(HDC hdc);
		virtual void Destroy();

		void AddGameObject(GameObject* gameObject);

		void EraseGameObject(GameObject* eraseGameObj);

		const std::vector<GameObject*> GetGameObjects() const { return mGameObjects; }
	private:
		void findDeadGameObjects(OUT std::vector<GameObject*>& gameObjs);
		void deleteGameObjects(std::vector<GameObject*> deleteObjs);
		void eraseDeadGameObject();

	private:
		std::vector<GameObject*> mGameObjects;
	};
}
