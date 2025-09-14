#include "wbCircleCollider2D.h"
#include "wbTransform.h"
#include "wbGameObject.h"
#include "wbRenderer.h"

namespace wb
{
	CircleCollider2D::CircleCollider2D()
		: Collider(eColliderType::Circle2D)
	{
	}
	CircleCollider2D::~CircleCollider2D()
	{
	}
	void CircleCollider2D::Initialize()
	{
	}
	void CircleCollider2D::Update()
	{
	}
	void CircleCollider2D::LateUpdate()
	{
	}
	void CircleCollider2D::Render(HDC hdc)
	{
		Transform* transform = GetOwner()->GetComponent<Transform>();
		Vector2 pos = transform->GetPosition();
		
		if (mainCamera)
			pos = mainCamera->CalculatePosition(pos);

		Vector2 offset = GetOffset();
		
		HBRUSH transparentBrush = CreateSolidBrush(NULL_BRUSH);
		HBRUSH oldBrush = (HBRUSH)SelectObject(hdc, transparentBrush);
		
		HPEN greenPen = CreatePen(PS_SOLID, 2, RGB(0, 255, 0));
		HPEN oldPen = (HPEN)SelectObject(hdc, greenPen);
		
		Ellipse(hdc, pos.x + offset.x
			, pos.y + offset.y
			, pos.x + offset.x + mRadius * GetSize().x
			, pos.y + offset.y + mRadius * GetSize().y);
		
		SelectObject(hdc, oldBrush);
		SelectObject(hdc, oldPen);
		DeleteObject(greenPen);
	}
}