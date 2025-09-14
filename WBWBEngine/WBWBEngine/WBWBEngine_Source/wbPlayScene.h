#pragma once
#include "wbScene.h"

namespace wb
{
	class Player;
	class PlayScene : public Scene
	{
	public:
		PlayScene();
		~PlayScene();

		void Initialize() override;
		void Update() override;
		void LateUpdate() override;
		void Render(HDC hdc) override;
	
		void OnEnter() override;
		void OnExit() override;
	
		//Player* GetAtaho()
		//{
		//	return mCharacters[(UINT)eCharacterType::Ataho];
		//}
		//Player* GetRinshang()
		//{
		//	return mCharacters[(UINT)eCharacterType::Rinshang];
		//}
		//Player* GetSmasu()
		//{
		//	return mCharacters[(UINT)eCharacterType::Smasu];
		//}
	private:
		std::vector<Player*> mCharacters;
	};
}
