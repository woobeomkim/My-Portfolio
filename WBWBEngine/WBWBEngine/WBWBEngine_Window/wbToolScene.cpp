#include "wbToolScene.h"
#include "wbObject.h"
#include "wbGameObject.h"
#include "wbTransform.h"
#include "wbCamera.h"
#include "wbRenderer.h"
#include "wbTile.h"
#include "wbTilemapRenderer.h"
#include "wbResources.h"
#include "wbInput.h"
#include "wbApplication.h"
#include "wbCameraScript.h"
#include "wbSpriteRenderer.h"
namespace wb
{
	ToolScene::ToolScene()
	{
	}
	ToolScene::~ToolScene()
	{
	}
	void ToolScene::Initialize()
	{
		GameObject* camera = Instantiate<GameObject>(eLayerType::Particle, Vector2(Application::GetInstance().GetWidth() / 2.0f, Application::GetInstance().GetHeight() / 2.0f));
		Camera* cameraComp = camera->AddComponent<Camera>();
		camera->AddComponent<CameraScript>();
		mainCamera = cameraComp;

		auto bg = Instantiate<GameObject>(eLayerType::BackGround, Vector2(0.0f, 0.0f));
		Texture* tex = Resources::Find<Texture>(L"test");
		auto sr = bg->AddComponent<SpriteRenderer>();
		sr->SetTexture(Resources::Find<Texture>(L"test"));

		Scene::Initialize();
	}
	void ToolScene::Update()
	{
		Scene::Update();
	}
	void ToolScene::LateUpdate()
	{
		Scene::LateUpdate();

		if (Input::GetKeyDown(eKeyCode::LButton))
		{
			createTileObject();
		}

		if (Input::GetKeyDown(eKeyCode::S))
		{
			Save();
		}
		if (Input::GetKeyDown(eKeyCode::L))
		{
			Load();
		}
			
	}
	void ToolScene::Render(HDC hdc)
	{
		Scene::Render(hdc);
		renderGreed(hdc);
	}
	void ToolScene::OnEnter()
	{
		Scene::OnEnter();
	}
	void ToolScene::OnExit()
	{
		Scene::OnExit();
	}
	void ToolScene::Save()
	{
		// open a file name
		OPENFILENAME ofn = {};

		wchar_t szFilePath[256] = {};

		ZeroMemory(&ofn, sizeof(ofn));
		ofn.lStructSize = sizeof(ofn);
		ofn.hwndOwner = NULL;
		ofn.lpstrFile = szFilePath;
		ofn.lpstrFile[0] = '\0';
		ofn.nMaxFile = 256;
		ofn.lpstrFilter = L"Tile\0*.tile\0";
		ofn.nFilterIndex = 1;
		ofn.lpstrFileTitle = NULL;
		ofn.nMaxFileTitle = 0;
		ofn.lpstrInitialDir = NULL;
		ofn.Flags = OFN_PATHMUSTEXIST | OFN_FILEMUSTEXIST;

		if (false == GetSaveFileName(&ofn))
			return;

		FILE* pFile = nullptr;
		_wfopen_s(&pFile, szFilePath, L"wb");

		for (Tile* tile : mTiles)
		{
			TilemapRenderer* tmr = tile->GetComponent<TilemapRenderer>();
			Transform* tr = tile->GetComponent<Transform>();

			Vector2 sourceIndex = tmr->GetIndex();
			Vector2 position = tr->GetPosition();

			int x = sourceIndex.x;
			fwrite(&x, sizeof(int), 1, pFile);
			int y = sourceIndex.y;
			fwrite(&y, sizeof(int), 1, pFile);

			x = position.x;
			fwrite(&x, sizeof(int), 1, pFile);
			y = position.y;
			fwrite(&y, sizeof(int), 1, pFile);
		}

		fclose(pFile);


	}
	void ToolScene::Load()
	{

		OPENFILENAME ofn = {};

		wchar_t szFilePath[256] = {};

		ZeroMemory(&ofn, sizeof(ofn));
		ofn.lStructSize = sizeof(ofn);
		ofn.hwndOwner = NULL;
		ofn.lpstrFile = szFilePath;
		ofn.lpstrFile[0] = '\0';
		ofn.nMaxFile = 256;
		ofn.lpstrFilter = L"All\0*.*\0Text\0*.TXT\0";
		ofn.nFilterIndex = 1;
		ofn.lpstrFileTitle = NULL;
		ofn.nMaxFileTitle = 0;
		ofn.lpstrInitialDir = NULL;
		ofn.Flags = OFN_PATHMUSTEXIST | OFN_FILEMUSTEXIST;

		if (false == GetOpenFileName(&ofn))
			return;

		FILE* pFile = nullptr;
		_wfopen_s(&pFile, szFilePath, L"rb");

		while (true)
		{
			int idxX = 0;
			int idxY = 0;

			int posX = 0;
			int posY = 0;


			if (fread(&idxX, sizeof(int), 1, pFile) == NULL)
				break;
			if (fread(&idxY, sizeof(int), 1, pFile) == NULL)
				break;
			if (fread(&posX, sizeof(int), 1, pFile) == NULL)
				break;
			if (fread(&posY, sizeof(int), 1, pFile) == NULL)
				break;

			Tile* tile = Instantiate<Tile>(eLayerType::Tile, Vector2(posX, posY));
			TilemapRenderer* tmr = tile->AddComponent<TilemapRenderer>();
			tmr->SetTexture(Resources::Find<Texture>(L"MAP1"));
			tmr->SetIndex(Vector2(idxX, idxY));

			mTiles.push_back(tile);
		}

		fclose(pFile);
	}
	void ToolScene::renderGreed(HDC hdc)
	{
		for (int i = 0; i < 50; i++)
		{
			Vector2 pos = mainCamera->CalculatePosition(Vector2(TilemapRenderer::TileSize.x * i, 0.0f));

			MoveToEx(hdc, pos.x, 0, nullptr);
			LineTo(hdc, pos.x, 1000);
		}

		for (int i = 0; i < 50; i++)
		{
			Vector2 pos = mainCamera->CalculatePosition(Vector2(0.0f, TilemapRenderer::TileSize.y * i));

			MoveToEx(hdc, 0, pos.y, nullptr);
			LineTo(hdc, 1000, pos.y);
		}
	}
	void ToolScene::createTileObject()
	{
		Vector2 pos = Input::GetMousePosition();

		pos = mainCamera->CalculateTilePosition(pos);

		if (pos.x >= 0.0f && pos.y >= 0.0f)
		{
			int idxX = pos.x / TilemapRenderer::TileSize.x;
			int idxY = pos.y / TilemapRenderer::TileSize.y;

			Tile* tile = Instantiate<Tile>(eLayerType::Tile);
			TilemapRenderer* tmr = tile->AddComponent<TilemapRenderer>();
			tmr->SetTexture(Resources::Find<Texture>(L"MAP1"));
			tmr->SetIndex(TilemapRenderer::SelectedIndex);

			tile->SetIndexPosition(idxX, idxY);
			mTiles.push_back(tile);
		}
	}
}

LRESULT CALLBACK WndTileProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	switch (message)
	{
	case WM_PAINT:
	{
		PAINTSTRUCT ps;
		HDC hdc = BeginPaint(hWnd, &ps);

		wb::Texture* texture
			= wb::Resources::Find<wb::Texture>(L"MAP1");

		TransparentBlt(hdc
			, 0, 0
			, texture->GetWidth()
			, texture->GetHeight()
			, texture->GetHdc()
			, 0, 0
			, texture->GetWidth()
			, texture->GetHeight()
			, RGB(255, 0, 255));

		EndPaint(hWnd, &ps);
	}
	break;
	
	case WM_LBUTTONDOWN:
	{
		POINT mousePos = {};
		GetCursorPos(&mousePos);
		ScreenToClient(hWnd, &mousePos);

		wb::Vector2 mousePosition;
		mousePosition.x = mousePos.x;
		mousePosition.y = mousePos.y;

		int idxX = mousePosition.x / wb::TilemapRenderer::OriginalTileSize.x;
		int idxY = mousePosition.y / wb::TilemapRenderer::OriginalTileSize.y;

		wb::TilemapRenderer::SelectedIndex = wb::Vector2(idxX, idxY);
	}
	break;
	case WM_DESTROY:
		PostQuitMessage(0);
		break;
	default:
		return DefWindowProc(hWnd, message, wParam, lParam);
	}
	return 0;
}
