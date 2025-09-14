#pragma once

namespace wb
{
	enum class ePlayerOrder
	{
		First,
		Second,
		Third,
		End,
	};

	enum class eLayerType
	{
		None,
		BackGround,
		Tile,
		Player,
		Particle,
		Max = 16,
	};

	enum class eComponentType
	{
		Transform,
		Collider,
		Script,
		SpriteRenderer,
		Animator,
		Ataho,
		Smisu,
		RinShan,
		BattleAtaho,
		BattleRinShan,
		BattleSmisu,
		Stat,
		Camera,
		End,
	};

	enum class eResourceType
	{
		Texture,
		AudioClip,
		Animation,
		Prefab,
		End,
	};

	enum class eColliderType
	{
		Circle2D,
		Rect2D,
		End,
	};
}