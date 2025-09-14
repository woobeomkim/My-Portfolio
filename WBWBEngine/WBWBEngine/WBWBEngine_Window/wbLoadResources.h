#pragma once
#include "..\\WBWBEngine_Source\\wbResources.h"
#include "..\\WBWBEngine_Source\\wbTexture.h"

namespace wb
{
	void LoadResources()
	{
		//TitleScene
		Resources::Load<Texture>(L"COMPILE1", L"..\\Resources\\Bmp\\title\\aaa.bmp");
		Resources::Load<Texture>(L"COMPILE2", L"..\\Resources\\Bmp\\title\\compile.bmp");
		Resources::Load<Texture>(L"TITLE", L"..\\Resources\\Bmp\\title\\title.bmp");
		Resources::Load<Texture>(L"BLACK", L"..\\Resources\\Bmp\\title\\black.bmp");
		Resources::Load<Texture>(L"PLAYER_AT", L"..\\Resources\\Bmp\\AnimationSheet\\cara_at1.bmp");
		Resources::Load<Texture>(L"PLAYER_RS", L"..\\Resources\\Bmp\\AnimationSheet\\cara_rs1.bmp");
		Resources::Load<Texture>(L"PLAYER_SM", L"..\\Resources\\Bmp\\AnimationSheet\\cara_sm1.bmp");
		
		//BattleScene
		Resources::Load<Texture>(L"BOSS_1", L"..\\Resources\\Bmp\\AnimationSheet\\boss_1.bmp");
		Resources::Load<Texture>(L"BOSS_2", L"..\\Resources\\Bmp\\AnimationSheet\\boss_2.bmp");
		Resources::Load<Texture>(L"PIG", L"..\\Resources\\Bmp\\AnimationSheet\\zj_ana.bmp");
		Resources::Load<Texture>(L"MONKEY", L"..\\Resources\\Bmp\\AnimationSheet\\zsa_saru.bmp");
		Resources::Load<Texture>(L"BATTLE_AT", L"..\\Resources\\Bmp\\AnimationSheet\\btl_at.bmp");
		
		// ATAHO BATTLE
		Resources::Load<Texture>(L"ATAHO_BATTLE_IDLE",L"..\\Resources\\Bmp\\Battle_modified\\Ataho\\ATAHO_IDLE.bmp");
		Resources::Load<Texture>(L"ATAHO_BATTLE_PUNCH",L"..\\Resources\\Bmp\\Battle_modified\\Ataho\\ATAHO_PUNCH.bmp");
		Resources::Load<Texture>(L"ATAHO_BATTLE_ROTATEKICK",L"..\\Resources\\Bmp\\Battle_modified\\Ataho\\ATAHO_ROTATEKICK.bmp");
		Resources::Load<Texture>(L"ATAHO_BATTLE_SLEEP",L"..\\Resources\\Bmp\\Battle_modified\\Ataho\\ATAHO_SLEEP.bmp");
		Resources::Load<Texture>(L"ATAHO_BATTLE_WIN",L"..\\Resources\\Bmp\\Battle_modified\\Ataho\\ATAHO_WIN.bmp");
		Resources::Load<Texture>(L"ATAHO_BATTLE_GUARD",L"..\\Resources\\Bmp\\Battle_modified\\Ataho\\ATAHO_GUARD.bmp");
		Resources::Load<Texture>(L"ATAHO_BATTLE_BEATTACKED",L"..\\Resources\\Bmp\\Battle_modified\\Ataho\\ATAHO_BEATTACKED.bmp");
		Resources::Load<Texture>(L"ATAHO_BATTLE_FLYINGKICK",L"..\\Resources\\Bmp\\Battle_modified\\Ataho\\ATAHO_FLYINGKICK.bmp");
		Resources::Load<Texture>(L"ATAHO_BATTLE_FAINTING",L"..\\Resources\\Bmp\\Battle_modified\\Ataho\\ATAHO_FAINTING.bmp");
		Resources::Load<Texture>(L"ATAHO_BATTLE_DRINKING",L"..\\Resources\\Bmp\\Battle_modified\\Ataho\\ATAHO_DRINKING.bmp");
		Resources::Load<Texture>(L"ATAHO_BATTLE_DRUNKSTATE",L"..\\Resources\\Bmp\\Battle_modified\\Ataho\\ATAHO_STATE_DRUNK.bmp");
		Resources::Load<Texture>(L"ATAHO_BATTLE_LEGKICK",L"..\\Resources\\Bmp\\Battle_modified\\Ataho\\ATAHO_LEGKICK.bmp");
		Resources::Load<Texture>(L"ATAHO_BATTLE_ROAR",L"..\\Resources\\Bmp\\Battle_modified\\Ataho\\ATAHO_ROAR.bmp");
		Resources::Load<Texture>(L"ATAHO_BATTLE_HOGYUKGWON",L"..\\Resources\\Bmp\\Battle_modified\\Ataho\\ATAHO_HOGYUKGWON.bmp");
		Resources::Load<Texture>(L"ATAHO_BATTLE_HOPOGWON",L"..\\Resources\\Bmp\\Battle_modified\\Ataho\\ATAHO_HOPOGWON.bmp");
		
		//Tile

		Resources::Load<Texture>(L"MAP1",L"..\\Resources\\Bmp\\map\\map_a1.bmp");
		Resources::Load<Texture>(L"test",L"..\\Resources\\test.png");


		//Resources::Load<Texture>(L"PLAYER1_IDLE", L"..\\Resources\\Bmp\\AnimationSheet\\cara_at_idle.bmp");
	}
}