#pragma once
#include "CommonInclude.h"
#include "wbComponent.h"

namespace wb
{
	class GameObject
	{
	public:
		friend void Destroy(GameObject* gameObject);
		
		enum class eState
		{
			Active,
			Paused,
			Dead,
			End
		};

		GameObject();
		~GameObject();

		virtual void Initialize();
		virtual void Update();
		virtual void LateUpdate();
		virtual void Render(HDC hdc);

		template<typename T>
		T* AddComponent()
		{
			T* comp = new T();
			comp->Initialize();
			comp->SetOwner(this);
			mComponents.push_back(comp);

			return comp;
		}

		template<typename T>
		T* GetComponent()
		{
			T* component = nullptr;
			for (Component* comp : mComponents)
			{
				component = dynamic_cast<T*>(comp);

				if (component)
					break;
			}

			return component;
		}

		eState GetState() { return mState; }
		void SetActive(bool power)
		{
			if (power) mState = eState::Active;
			else mState = eState::Paused;
		}


		bool IsActive() { return mState == eState::Active; }
		bool IsDead() { return mState == eState::Dead; }

		void SetSpriteSize(Vector2 size) { mSpriteSize = size; }
		Vector2 GetSpriteSize() { return mSpriteSize; }

		void SetLayerType(eLayerType layerType) { mLayerType = layerType; }
		eLayerType GetLayerType() { return mLayerType; }
	private:
		void initializeTransform();
		void death() { mState = GameObject::eState::Dead; }

	private:
		std::vector<Component*> mComponents = {};
		eState mState;
		Vector2 mSpriteSize;
		eLayerType mLayerType;
	};

	void Destroy(GameObject* gameObject);
}