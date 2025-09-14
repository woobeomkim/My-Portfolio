#pragma once
#include "wbComponent.h"
#include "wbTexture.h"

namespace wb
{
    class TilemapRenderer : public Component
    {
	public:
		TilemapRenderer();
		~TilemapRenderer();
		virtual void Initialize() override;
		virtual void Update() override;
		virtual void LateUpdate() override;
		virtual void Render(HDC hdc) override;

		void SetTexture(Texture* texture) { mTexture = texture; }
		void SetSize(Vector2 size) { mSize = size; }
		Vector2 GetIndex() { return mIndex; }
		void SetIndex(Vector2 index) { mIndex = index; }

	public:
		static Vector2 TileSize;
		static Vector2 OriginalTileSize;
		static Vector2 SelectedIndex;
	private:
		Vector2 mTileSize;
		Texture* mTexture;
		Vector2 mSize;
		Vector2 mIndex;
    };
}
