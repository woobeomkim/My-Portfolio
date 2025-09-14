#include "wbBullet.h"
#include "wbApplication.h"
#include "wbMonster.h"


namespace wb
{
	Bullet::Bullet()
	{
	}

	Bullet::~Bullet()
	{
	}

	void Bullet::Initialize()
	{
		mSpeed = 0.5f;
		mX = 0.0f;
		mY = 0.0f;
		mSize = 25.0f;
		mbDead = false;
	}

	void Bullet::Update()
	{
		mY -= mSpeed;

		if (mY <= 0.0f)
			Dead();
	}

	void Bullet::Render(HDC hdc)
	{
		Ellipse(hdc, mX - mSize / 2, mY - mSize / 2, mX + mSize / 2, mY + mSize / 2);
	}

	void Bullet::LateUpdate()
	{
	}
}