#include "wbPlayScene.h"
#include "wbGameObject.h"
#include "wbTransform.h"
#include "wbSpriteRenderer.h"
#include "wbPlayer.h"
#include "wbObject.h"
#include "wbTexture.h"
#include "wbResource.h"
#include "wbResources.h"
#include "wbAnimation.h"
#include "wbAnimator.h"
#include "..\\WBWBEngine_Window\\\wbPlayerScript.h"
#include "wbInput.h"
#include "..\\WBWBEngine_Window\\Smisu.h"
#include "..\\WBWBEngine_Window\RinShan.h"
#include "..\\WBWBEngine_Window\Ataho.h"
#include "wbBoxCollider2D.h"
#include "wbCollider.h"
#include "..\\WBWBEngine_Window\\wbTile.h"
#include "wbTilemapRenderer.h"
namespace wb
{
	PlayScene::PlayScene()
		: mCharacters{}
	{
		//mCharacters.resize(static_cast<size_t>(eCharacterType::End));
	}
	PlayScene::~PlayScene()
	{
	}
	void PlayScene::Initialize()
	{
		//FILE* pFile = nullptr;
		//_wfopen_s(&pFile, L"..\\Resources\\test", L"rb");

		//while (true)
		//{
		//	int idxX = 0;
		//	int idxY = 0;

		//	int posX = 0;
		//	int posY = 0;


		//	if (fread(&idxX, sizeof(int), 1, pFile) == NULL)
		//		break;
		//	if (fread(&idxY, sizeof(int), 1, pFile) == NULL)
		//		break;
		//	if (fread(&posX, sizeof(int), 1, pFile) == NULL)
		//		break;
		//	if (fread(&posY, sizeof(int), 1, pFile) == NULL)
		//		break;

		//	Tile* tile = Instantiate<Tile>(eLayerType::Tile, Vector2(posX, posY));
		//	TilemapRenderer* tmr = tile->AddComponent<TilemapRenderer>();
		//	tmr->SetTexture(Resources::Find<Texture>(L"MAP1"));
		//	tmr->SetIndex(Vector2(idxX, idxY));

		//	//mTiles.push_back(tile);
		//}

		//fclose(pFile);

		Player* player = Instantiate<Player>(eLayerType::Player, Vector2(320.0f, 200.0f));
		Texture* tex = Resources::Find<Texture>(L"PLAYER_AT");
		Animator* animator = player->AddComponent<Animator>();
		Ataho* at = player->AddComponent<Ataho>();
		
		player->SetLeader(nullptr);
		Transform* trAt = player->GetComponent<Transform>();
		BoxCollider2D* atCol = player->AddComponent<BoxCollider2D>();
		player->SetSpriteSize(Vector2(48.0f, 64.0f));
		Vector2 spriteSize = player->GetSpriteSize();
		atCol->SetOffset(spriteSize / 2.0f * -1.0f);

		animator->CreateAnimation(L"DOWNWALK", tex, Vector2(0, 0), Vector2(48.0f, 64.0f), Vector2::Zero, 5, 0.1f);
		
		animator->CreateAnimation(L"LEFTWALK", tex, Vector2(240.0f, 0), Vector2(48.0f, 64.0f), Vector2::Zero, 5, 0.1f);

		animator->CreateAnimation(L"UPWALK", tex, Vector2(0.0f, 64.0f), Vector2(48.0f, 64.0f), Vector2::Zero, 5, 0.1f);
		animator->CreateAnimation(L"RIGHTWALK", tex, Vector2(240.0f, 64.0f), Vector2(48.0f, 64.0f), Vector2::Zero, 5, 0.1f);
		
		animator->CreateAnimation(L"DOWNWALK_IDLE", tex, Vector2(0, 0), Vector2(48.0f, 64.0f), Vector2::Zero, 1, 0.1f);

		animator->CreateAnimation(L"LEFTWALK_IDLE", tex, Vector2(240.0f, 0), Vector2(48.0f, 64.0f), Vector2::Zero, 1, 0.1f);

		animator->CreateAnimation(L"UPWALK_IDLE", tex, Vector2(0.0f, 64.0f), Vector2(48.0f, 64.0f), Vector2::Zero, 1, 0.1f);
		animator->CreateAnimation(L"RIGHTWALK_IDLE", tex, Vector2(240.0f, 64.0f), Vector2(48.0f, 64.0f), Vector2::Zero, 1, 0.1f);
		animator->PlayAnimation(L"DOWNWALK_IDLE", false);


		Player* playerRs = Instantiate<Player>(eLayerType::Player, Vector2(320.0f, 144.0f));
		playerRs->SetLeader(trAt);
		Transform* trRs = playerRs->GetComponent<Transform>();
		tex = Resources::Find<Texture>(L"PLAYER_RS");
		Animator* animatorRs = playerRs->AddComponent<Animator>();
		RinShan* rs = playerRs->AddComponent<RinShan>();
		BoxCollider2D* rsCol = playerRs->AddComponent<BoxCollider2D>();
		playerRs->SetSpriteSize(Vector2(48.0f, 64.0f));
		spriteSize = playerRs->GetSpriteSize();
		rsCol->SetOffset(spriteSize / 2.0f * -1.0f);
		
		animatorRs->CreateAnimation(L"DOWNWALK", tex, Vector2(0, 0), Vector2(48.0f, 64.0f), Vector2::Zero, 5, 0.1f);

		animatorRs->CreateAnimation(L"LEFTWALK", tex, Vector2(240.0f, 0), Vector2(48.0f, 64.0f), Vector2::Zero, 5, 0.1f);

		animatorRs->CreateAnimation(L"UPWALK", tex, Vector2(0.0f, 64.0f), Vector2(48.0f, 64.0f), Vector2::Zero, 5, 0.1f);
		animatorRs->CreateAnimation(L"RIGHTWALK", tex, Vector2(240.0f, 64.0f), Vector2(48.0f, 64.0f), Vector2::Zero, 5, 0.1f);

		animatorRs->CreateAnimation(L"DOWNWALK_IDLE", tex, Vector2(0, 0), Vector2(48.0f, 64.0f), Vector2::Zero, 1, 0.1f);

		animatorRs->CreateAnimation(L"LEFTWALK_IDLE", tex, Vector2(240.0f, 0), Vector2(48.0f, 64.0f), Vector2::Zero, 1, 0.1f);

		animatorRs->CreateAnimation(L"UPWALK_IDLE", tex, Vector2(0.0f, 64.0f), Vector2(48.0f, 64.0f), Vector2::Zero, 1, 0.1f);
		animatorRs->CreateAnimation(L"RIGHTWALK_IDLE", tex, Vector2(240.0f, 64.0f), Vector2(48.0f, 64.0f), Vector2::Zero, 1, 0.1f);
		animatorRs->PlayAnimation(L"DOWNWALK_IDLE", false);

		//mCharacters[(UINT)eCharacterType::Rinshang] = playerRs;

		Player* playerSm = Instantiate<Player>(eLayerType::Player, Vector2(320.0f, 80.0f));
		playerSm->SetLeader(trRs);
		tex = Resources::Find<Texture>(L"PLAYER_SM");
		Animator* animatorSm = playerSm->AddComponent<Animator>();
		Smisu* sm = playerSm->AddComponent<Smisu>();
		BoxCollider2D* smCol = playerSm->AddComponent<BoxCollider2D>();
		playerSm->SetSpriteSize(Vector2(48.0f, 64.0f));
		spriteSize = playerSm->GetSpriteSize();
		smCol->SetOffset(spriteSize / 2.0f * -1.0f);

	
		animatorSm->CreateAnimation(L"DOWNWALK", tex, Vector2(0, 0), Vector2(48.0f, 64.0f), Vector2::Zero, 5, 0.1f);

		animatorSm->CreateAnimation(L"LEFTWALK", tex, Vector2(240.0f, 0), Vector2(48.0f, 64.0f), Vector2::Zero, 5, 0.1f);

		animatorSm->CreateAnimation(L"UPWALK", tex, Vector2(0.0f, 64.0f), Vector2(48.0f, 64.0f), Vector2::Zero, 5, 0.1f);
		animatorSm->CreateAnimation(L"RIGHTWALK", tex, Vector2(240.0f, 64.0f), Vector2(48.0f, 64.0f), Vector2::Zero, 5, 0.1f);

		animatorSm->CreateAnimation(L"DOWNWALK_IDLE", tex, Vector2(0, 0), Vector2(48.0f, 64.0f), Vector2::Zero, 1, 0.1f);

		animatorSm->CreateAnimation(L"LEFTWALK_IDLE", tex, Vector2(240.0f, 0), Vector2(48.0f, 64.0f), Vector2::Zero, 1, 0.1f);

		animatorSm->CreateAnimation(L"UPWALK_IDLE", tex, Vector2(0.0f, 64.0f), Vector2(48.0f, 64.0f), Vector2::Zero, 1, 0.1f);
 		animatorSm->CreateAnimation(L"RIGHTWALK_IDLE", tex, Vector2(240.0f, 64.0f), Vector2(48.0f, 64.0f), Vector2::Zero, 1, 0.1f);
		animatorSm->PlayAnimation(L"DOWNWALK_IDLE", false);

		Scene::Initialize();
		
	}
	void PlayScene::Update()
	{
		Scene::Update();
	}
	void PlayScene::LateUpdate()
	{
		if (Input::GetKey(eKeyCode::B))
			SceneManager::LoadScene(L"BattleScene");
		Scene::LateUpdate();
	}
	void PlayScene::Render(HDC hdc)
	{
		Scene::Render(hdc);
	}
	void PlayScene::OnEnter()
	{
	}
	void PlayScene::OnExit()
	{

	}
}