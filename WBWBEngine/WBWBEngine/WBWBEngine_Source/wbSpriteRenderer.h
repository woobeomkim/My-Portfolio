#pragma once
#include "wbComponent.h"
#include "wbTexture.h"

namespace wb
{
    class SpriteRenderer : public Component
    {
	public:
		SpriteRenderer();
		~SpriteRenderer();

		void Initialize() override;
		void Update() override;
		void LateUpdate() override;
		void Render(HDC hdc) override;

		//void ImageLoad(const std::wstring& path);
		void SetTexture(Texture* texture) { mTexture = texture; }
		void SetSize(Vector2 size) { mSize = size; }
	private:
		Vector2 mSize;
		Texture* mTexture;
    };
}
