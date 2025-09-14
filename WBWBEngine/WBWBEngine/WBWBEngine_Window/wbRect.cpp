#include "wbRect.h"
#include "wbTransform.h"

namespace wb
{
	wbRect::wbRect() : mSize(10.0f, 10.0f)
	{
	}
	wbRect::~wbRect()
	{
	}
	void wbRect::Initialize()
	{
		GameObject::Initialize();
	}
	void wbRect::Update()
	{
		GameObject::Update();
	}
	void wbRect::LateUpdate()
	{
		GameObject::LateUpdate();
	}
	void wbRect::Render(HDC hdc)
	{
		Transform* tr = GetComponent<Transform>();
		Vector2 pos = tr->GetPosition();
		
		Rectangle(hdc, pos.x - mSize.x / 2, pos.y - mSize.y / 2, pos.x + mSize.x / 2, pos.y + mSize.y / 2);

		GameObject::Render(hdc);
	}
}