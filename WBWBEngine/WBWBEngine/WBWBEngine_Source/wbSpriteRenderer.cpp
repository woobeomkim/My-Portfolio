#include "wbSpriteRenderer.h"
#include "wbTransform.h"
#include "wbGameObject.h"
#include "wbTexture.h"
#include "wbRenderer.h"
namespace wb
{
	SpriteRenderer::SpriteRenderer()
		:
		Component(eComponentType::SpriteRenderer)
		,mTexture(nullptr)
		,mSize(Vector2::One)
	{
	}
	SpriteRenderer::~SpriteRenderer()
	{
	}
	void SpriteRenderer::Initialize()
	{
	}
	void SpriteRenderer::Update()
	{

	}
	void SpriteRenderer::LateUpdate()
	{
	}
	void SpriteRenderer::Render(HDC hdc)
	{
		if (mTexture == nullptr)
			return;

		Transform* tr = GetOwner()->GetComponent<Transform>();
		Vector2 pos = tr->GetPosition();
		float rot = tr->GetRotation();
		Vector2 scale = tr->GetScale();


		Texture::eTextureType type = mTexture->GetTextureType();

		if(mainCamera)
			pos = mainCamera->CalculatePosition(pos);

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
					, pos.x
					, pos.y
					, mTexture->GetWidth() * scale.x
					, mTexture->GetHeight() * scale.y
					, mTexture->GetHdc()
					, 0
					, 0
					, mTexture->GetWidth()
					, mTexture->GetHeight()
					, func);
			}
			else
			{
				TransparentBlt(hdc, pos.x, pos.y
					, mTexture->GetWidth() * mSize.x * scale.x
					, mTexture->GetHeight() * mSize.y * scale.y
					, mTexture->GetHdc()
					, 0, 0
					, mTexture->GetWidth()
					, mTexture->GetHeight()
					, RGB(255, 0, 255));
			}
		}
		else if (type == Texture::eTextureType::Png)
		{
			Gdiplus::ImageAttributes imgAtt = {};
			imgAtt.SetColorKey(Gdiplus::Color(50, 50, 50), Gdiplus::Color(100, 100, 100));
			
			Gdiplus::Graphics graphics(hdc);
		
			graphics.TranslateTransform(pos.x, pos.y);
			graphics.RotateTransform(rot);
			graphics.TranslateTransform(-pos.x, -pos.y);
			
			graphics.DrawImage(mTexture->GetImage()
				, Gdiplus::Rect
				(
					pos.x, pos.y
					, mTexture->GetWidth() * mSize.x * scale.x
					, mTexture->GetHeight() * mSize.y * scale.y
				)
				, 0, 0
				, mTexture->GetWidth(), mTexture->GetHeight()
				, Gdiplus::UnitPixel
				, &imgAtt);
		}

	}
}