#include "wbSceneManager.h"
#include "wbScene.h"
#include "wbDontDestroyOnLoad.h"

namespace wb
{
	std::map<const std::wstring, Scene*> SceneManager::mScene = {};
	Scene* SceneManager::mActiveScene = nullptr;
	Scene* SceneManager::mDontDestroyOnLoad = nullptr;

	std::vector<GameObject*> SceneManager::GetGameObjects(eLayerType layer)
	{
		std::vector<GameObject*> gameObjects
			= mActiveScene->GetLayer(layer)->GetGameObjects();
		std::vector<GameObject*> dontDestroyOnLoad
			= mDontDestroyOnLoad->GetLayer(layer)->GetGameObjects();

		gameObjects.insert(gameObjects.end(), dontDestroyOnLoad.begin(), dontDestroyOnLoad.end());

		return gameObjects;
	}

	Scene* SceneManager::LoadScene(const std::wstring& name)
	{
		if (mActiveScene)
			mActiveScene->OnExit();

		std::map<std::wstring, Scene*>::iterator iter = mScene.find(name);
		
		if (iter == mScene.end())
			return nullptr;
		
		mActiveScene = iter->second;
		mActiveScene->OnEnter();


		return iter->second;
	}

	SceneManager::SceneManager()
	{
	}
	SceneManager::~SceneManager()
	{
	}
	void SceneManager::Initialize()
	{
		mDontDestroyOnLoad = CreateScene<DontDestroyOnLoad>(L"DontDestroyOnLoad");
		mActiveScene->Initialize();
	}
	void SceneManager::Update()
	{
		mActiveScene->Update();
		mDontDestroyOnLoad->Update();
	}
	void SceneManager::LateUpdate()
	{
		mActiveScene->LateUpdate();
		mDontDestroyOnLoad->LateUpdate();
	}
	void SceneManager::Render(HDC hdc)
	{
		mActiveScene->Render(hdc);
		mDontDestroyOnLoad->Render(hdc);
	}

	void SceneManager::Destroy()
	{
		mActiveScene->Destroy();
		mDontDestroyOnLoad->Destroy();
	}

	void SceneManager::Release()
	{
		for (auto& iter : mScene)
		{
			delete iter.second;
			iter.second = nullptr;
		}
	}
}