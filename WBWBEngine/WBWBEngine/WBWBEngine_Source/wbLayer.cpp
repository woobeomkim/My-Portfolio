#include "wbLayer.h"
#include "wbLayer.h"
#include "wbLayer.h"
#include "wbLayer.h"

namespace wb
{
	Layer::Layer()
		:mGameObjects{}
	{
	}
	Layer::~Layer()
	{
	}
	void Layer::Initialize()
	{
		for (GameObject* gameObj : mGameObjects)
		{
			if (gameObj == nullptr)
				continue;

			gameObj->Initialize();
		}
	}
	void Layer::Update()
	{
		for (GameObject* gameObj : mGameObjects)
		{
			if (gameObj == nullptr)
				continue;
			GameObject::eState state = gameObj->GetState();
			if (state == GameObject::eState::Paused
				|| state == GameObject::eState::Dead)
				continue;
			gameObj->Update();
		}
	}
	void Layer::LateUpdate()
	{
		for (GameObject* gameObj : mGameObjects)
		{
			if (gameObj == nullptr)
				continue;
			GameObject::eState state = gameObj->GetState();
			if (state == GameObject::eState::Paused
				|| state == GameObject::eState::Dead)
				continue;
			gameObj->LateUpdate();
		}
	}
	void Layer::Render(HDC hdc)
	{
		for (GameObject* gameObj : mGameObjects)
		{
			if (gameObj == nullptr)
				continue;
			GameObject::eState state = gameObj->GetState();
			if (state == GameObject::eState::Paused
				|| state == GameObject::eState::Dead)
				continue;
			gameObj->Render(hdc);
		}
	}

	void Layer::Destroy()
	{
		std::vector<GameObject*> deleteObjects;
		findDeadGameObjects(deleteObjects);
		eraseDeadGameObject();
		deleteGameObjects(deleteObjects);
	}

	void Layer::AddGameObject(GameObject* gameObject)
	{
		if (gameObject == nullptr)
			return;
		mGameObjects.push_back(gameObject);
	}
	void Layer::EraseGameObject(GameObject* eraseObject)
	{
		std::erase_if(mGameObjects,
			[=](GameObject* gameObj)
			{
				return gameObj == eraseObject;
			});
	}

	void Layer::findDeadGameObjects(OUT std::vector<GameObject*>& gameObjs)
	{
		for (GameObject* gameObj : mGameObjects)
		{
			GameObject::eState state = gameObj->GetState();
			if (state == GameObject::eState::Dead)
				gameObjs.push_back(gameObj);
		}
	}
	void Layer::deleteGameObjects(std::vector<GameObject*> deleteObjs)
	{
		for (GameObject* gameObj : deleteObjs)
		{
			delete gameObj;
			gameObj = nullptr;
		}
	}
	void Layer::eraseDeadGameObject()
	{
		std::erase_if(mGameObjects,
			[](GameObject* gameObj)
			{
				return (gameObj)->IsDead();
			});
	}
}