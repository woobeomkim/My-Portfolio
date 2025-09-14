#pragma once
#include "wbResource.h"


namespace wb
{
	class Texture;
	class Animator;

	class Animation : public Resource
	{
	public:
		struct Sprite
		{
			Vector2 leftTop;
			Vector2 size;
			Vector2 offset;
			float duration;

			Sprite() : leftTop(Vector2::Zero)
				, size(Vector2::Zero)
				, offset(Vector2::Zero)
				, duration(0.0f)
			{ }
		};

		Animation();
		~Animation();

		virtual HRESULT Load(const std::wstring& path) override;
	
		void Update();
		void Render(HDC hdc);


		void CreateAnimation(const std::wstring& name
			, Texture* spriteSheet
			, Vector2 leftTop
			, Vector2 size
			, Vector2 offset
			, UINT spriteLength
			, float duration);

		bool IsComplete() { return mbComplete; }
		void SetAnimator(Animator* animator) { mAnimator = animator; }
		void Reset();
	private:
		bool mbComplete;
		Animator* mAnimator;
		Texture* mTexture;
		std::vector<Sprite> mAnimationSheet;

		int mIndex;
		float mTime;
		
	};
}
