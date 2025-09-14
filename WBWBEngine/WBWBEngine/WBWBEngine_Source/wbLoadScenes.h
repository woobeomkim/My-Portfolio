#pragma once

#include "wbPlayScene.h"
#include "wbSceneManager.h"
#include "wbTitleScene.h"
#include "..\\WBWBEngine_Window\\wbBattleScene.h"
#include "..\\WBWBEngine_Window\\wbStudyScene.h"
#include "..\\WBWBEngine_Window\\wbToolScene.h"
namespace wb
{
	void LoadScenes()
	{
		SceneManager::CreateScene<PlayScene>(L"PlayScene");
		SceneManager::CreateScene<TitleScene>(L"TitleScene");
		SceneManager::CreateScene<StudyScene>(L"StudyScene");
		SceneManager::CreateScene<BattleScene>(L"BattleScene");
		SceneManager::CreateScene<ToolScene>(L"ToolScene");


		//SceneManager::LoadScene(L"TitleScene");
		SceneManager::LoadScene(L"ToolScene");
		//SceneManager::LoadScene(L"PlayScene");
	}
}