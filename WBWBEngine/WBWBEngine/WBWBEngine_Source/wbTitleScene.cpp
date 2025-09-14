#include "wbTitleScene.h"
#include "wbSceneManager.h"
#include "wbInput.h"
#include "wbGameObject.h"
#include "wbObject.h"
#include "wbSpriteRenderer.h"
#include "wbResource.h"
#include "wbResources.h"
#include "wbTexture.h"
#include "wbTime.h"

namespace wb
{

	TitleScene::TitleScene()
	{
	}
	TitleScene::~TitleScene()
	{
	}
	void TitleScene::Initialize()
	{
		mBg = Instantiate<GameObject>(eLayerType::BackGround);
		mSr = mBg->AddComponent<SpriteRenderer>();
		mTex = Resources::Find<Texture>(L"COMPILE1");

		mSr->SetName(L"SR");
		mSr->SetTexture(mTex);
		Scene::Initialize();
	}
	void TitleScene::Update()
	{
		
		accTime += Time::DeltaTime();
		
		if (1.0f <= accTime && accTime < 1.5f)
		{
			mTex = Resources::Find<Texture>(L"BLACK");
			mSr->SetTexture(mTex);
			//mState = ScreenState::BlackScreen;
		}

		if (accTime >= 3.0f)
		{
			SpriteRenderer* sr = mBg->GetComponent<SpriteRenderer>();
			mTex = Resources::Find<Texture>(L"COMPILE2");
			//sr->SetName(L"SR");
			mSr->SetTexture(mTex);
		}

		if (accTime >= 4.0f)
		{
			SpriteRenderer* sr = mBg->GetComponent<SpriteRenderer>();
			mTex = Resources::Find<Texture>(L"TITLE");
			//sr->SetName(L"SR");
			mSr->SetTexture(mTex);
		}
		Scene::Update();
		
		if (Input::GetKeyDown(eKeyCode::SPACE))
		{
			SceneManager::LoadScene(L"PlayScene");
		}
	}
	void TitleScene::LateUpdate()
	{
		Scene::LateUpdate();
	}
	void TitleScene::Render(HDC hdc)
	{

		//if(mState == ScreenState::TitleScreen)
			Scene::Render(hdc);
		//else
		//{
		//		SpriteRenderer* sr = mBg->GetComponent<SpriteRenderer>();
		//		sr->SetTexture(nullptr);
		//		HBRUSH blackBrush = (HBRUSH)GetStockObject(BLACK_BRUSH);
		//		HBRUSH oldBrush = (HBRUSH)SelectObject(hdc, blackBrush);
		//		Rectangle(hdc, -1, -1, 640.0f, 480.0f);
		//		(HBRUSH)SelectObject(hdc, oldBrush);
		//		if (accTime >= 2.0f)
		//			mState = ScreenState::TitleScreen;
		//}
	}
	void TitleScene::OnEnter()
	{
	}
	void TitleScene::OnExit()
	{
	}
}
