#include "wbCollider.h"

namespace wb
{
	UINT Collider::CollisionID = 1;

	Collider::Collider(eColliderType type)
		: Component(eComponentType::Collider)
		, mType(type)
		, mID(CollisionID++)
		, mSize(Vector2::One)
	{
	}
	Collider::~Collider()
	{
	}
	void Collider::Initialize()
	{
	}
	void Collider::Update()
	{
	}
	void Collider::LateUpdate()
	{
	}
	void Collider::Render(HDC hdc)
	{
	}
	void Collider::OnCollisionEnter(Collider* other)
	{
	}
	void Collider::OnCollisionStay(Collider* other)
	{
	}
	void Collider::OnCollisionExit(Collider* other)
	{
	}
}