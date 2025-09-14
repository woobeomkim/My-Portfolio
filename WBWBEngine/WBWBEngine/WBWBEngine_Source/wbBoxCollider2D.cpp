#include "wbBoxCollider2D.h"
#include "wbTransform.h"
#include "wbGameObject.h"
#include "wbRenderer.h"

namespace wb
{
	BoxCollider2D::BoxCollider2D()
		: Collider(eColliderType::Rect2D)
	{

	}
	BoxCollider2D::~BoxCollider2D()
	{
	}
	void BoxCollider2D::Initialize()
	{
	}
	void BoxCollider2D::Update()
	{
	}
	void BoxCollider2D::LateUpdate()
	{
	}
	void BoxCollider2D::Render(HDC hdc)
	{
		Transform* transform = GetOwner()->GetComponent<Transform>();
		Vector2 pos = transform->GetPosition();
	
		if (mainCamera)
			pos = mainCamera->CalculatePosition(pos);

		Vector2 offset = GetOffset();

		HBRUSH transparentBrush = (HBRUSH)GetStockObject(NULL_BRUSH);
		HBRUSH oldBrush = (HBRUSH)SelectObject(hdc, transparentBrush);

		HPEN greenPen = CreatePen(PS_SOLID, 2, RGB(0, 255, 0));
		HPEN oldPen = (HPEN)SelectObject(hdc, greenPen);
	
		Rectangle(hdc, pos.x + offset.x
			, pos.y + offset.y
			, pos.x + offset.x + GetSize().x * 48.0f
			, pos.y + offset.y + GetSize().y * 64.0f);

		SelectObject(hdc, oldBrush);
		SelectObject(hdc, oldPen);
		DeleteObject(greenPen);

	}
}