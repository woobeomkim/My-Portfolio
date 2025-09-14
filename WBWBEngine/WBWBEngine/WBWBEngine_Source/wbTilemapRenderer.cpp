#include "wbTilemapRenderer.h"
#include "wbTransform.h"
#include "wbGameObject.h"
#include "wbRenderer.h"
#include "wbTexture.h"

namespace wb
{
	Vector2 TilemapRenderer::TileSize = Vector2::One;
	Vector2 TilemapRenderer::OriginalTileSize = Vector2::One;
	Vector2 TilemapRenderer::SelectedIndex = Vector2::One;

	TilemapRenderer::TilemapRenderer()
		: Component(eComponentType::SpriteRenderer)
		, mTexture(nullptr)
		, mSize(1.0f,1.0f)
		, mIndex(0,0)
		, mTileSize(16.0f,16.0f)
	{
		TileSize = mTileSize * mSize;
		OriginalTileSize = mTileSize;
	}
	TilemapRenderer::~TilemapRenderer()
	{
	}
	void TilemapRenderer::Initialize()
	{
		Component::Initialize();
	}
	void TilemapRenderer::Update()
	{
		Component::Update();
	}
	void TilemapRenderer::LateUpdate()
	{
		Component::LateUpdate();
	}
	void TilemapRenderer::Render(HDC hdc)
	{

		if (mTexture == nullptr)
			return;

		Transform* tr = GetOwner()->GetComponent<Transform>();
		Vector2 pos = tr->GetPosition();
		float rot = tr->GetRotation();
		Vector2 scale = tr->GetScale();

		pos = mainCamera->CalculatePosition(pos);
		
		Texture::eTextureType type = mTexture->GetTextureType();
		if (type == Texture::eTextureType::Bmp)
		{
			if (mTexture->IsAlpha())
			{
				BLENDFUNCTION func = {};
				func.BlendOp = AC_SRC_OVER;
				func.BlendFlags = 0;
				func.AlphaFormat = AC_SRC_ALPHA;
				func.SourceConstantAlpha = 255;

				AlphaBlend(hdc
					, pos.x,pos.y
					, mTileSize.x *mSize.x*scale.x
					, mTileSize.y * mSize.y * scale.y
					, mTexture->GetHdc()
					, mIndex.x * mTileSize.x, mIndex.y * mTileSize.y
					, mTileSize.x
					, mTileSize.y
					, func);
			}
			else
			{
				TransparentBlt(hdc
					, pos.x, pos.y
					, mTileSize.x * mSize.x * scale.x
					, mTileSize.y * mSize.y * scale.y
					, mTexture->GetHdc()
					, mIndex.x * mTileSize.x, mIndex.y * mTileSize.y
					, mTileSize.x
					, mTileSize.y
					, RGB(255, 0, 255));
			}
		}
	}
}