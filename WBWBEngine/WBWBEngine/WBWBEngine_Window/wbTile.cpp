#include "wbTile.h"
#include "wbTransform.h"
#include "wbTilemapRenderer.h"


namespace wb
{
	void Tile::Initialize()
	{
		GameObject::Initialize();
	}
	void Tile::Update()
	{
		GameObject::Update();
	}
	void Tile::LateUpdate()
	{
		GameObject::LateUpdate();
	}
	void Tile::Render(HDC hdc)
	{
		GameObject::Render(hdc);
	}
	void Tile::SetIndexPosition(int x, int y)
	{
		Transform* tr = GetComponent<Transform>();
		Vector2 pos = tr->GetPosition();
		pos.x = TilemapRenderer::TileSize.x * x;
		pos.y = TilemapRenderer::TileSize.y * y;
		tr->SetPosition(pos);
	}
}