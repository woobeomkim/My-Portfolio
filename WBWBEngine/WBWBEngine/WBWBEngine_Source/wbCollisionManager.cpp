#include "wbCollisionManager.h"
#include "wbScene.h"
#include "wbCollider.h"
#include "wbLayer.h"
#include "wbSceneManager.h"
#include "wbGameObject.h"
#include "wbTransform.h"
#include  <cmath>

namespace wb
{
	std::bitset<(UINT)eLayerType::Max> CollisionManager::mCollisionLayerMatrix[(UINT)eLayerType::Max] = {};
	std::unordered_map<UINT64, bool> CollisionManager::mCollisionMap = {};
	void CollisionManager::Initialize()
	{
	}

	void CollisionManager::Update()
	{
		Scene* scene = SceneManager::GetActiveScene();
		for (UINT row = 0; row < (UINT)eLayerType::Max; ++row)
		{
			for (UINT col = 0; col < (UINT)eLayerType::Max; ++col)
			{
				if (mCollisionLayerMatrix[row][col])
				{
					LayerCollision(scene, (eLayerType)row, (eLayerType)col);
				}
			}
		}
	}

	void CollisionManager::LateUpdate()
	{
	}
	void CollisionManager::Render(HDC hdc)
	{
	}
	void CollisionManager::Clear()
	{
		mCollisionMap.clear();
		mCollisionLayerMatrix->reset();
	}
	void CollisionManager::CollisionLayerCheck(eLayerType left, eLayerType right, bool enable)
	{
		int row = 0;
		int col = 0;

		if (left <= right)
		{
			row = (int)left;
			col = (int)right;
		}
		else
		{
			row = (int)right;
			col = (int)left;
		}

		mCollisionLayerMatrix[row][col] = enable;
	}
	void CollisionManager::LayerCollision(Scene* scene, eLayerType left, eLayerType right)
	{
		const std::vector<GameObject*> lefts = SceneManager::GetGameObjects(left);
		const std::vector<GameObject*> rights = SceneManager::GetGameObjects(right);

		for (GameObject* left : lefts)
		{
			if (left->IsActive() == false)
				continue;
			Collider* leftCollider = left->GetComponent<Collider>();
			if (leftCollider == nullptr)
				continue;

			for (GameObject* right : rights)
			{
				if (right->IsActive() == false)
					continue;
				Collider* rightCollider = right->GetComponent<Collider>();
				if (rightCollider == nullptr)
					continue;
				if (left == right)
					continue;

				ColliderCollision(leftCollider, rightCollider);
			}
		}
	}
	void CollisionManager::ColliderCollision(Collider* left, Collider* right)
	{
		CollisionID id = {};
		id.left = left->GetID();
		id.right = right->GetID();

		
		auto iter = mCollisionMap.find(id.id);
		if (iter == mCollisionMap.end())
		{
			mCollisionMap.insert(std::make_pair(id.id, false));
			iter = mCollisionMap.find(id.id);
		
			if (Intersect(left, right))
			{
				// 최초 충돌할때
				if (iter->second == false)
				{
					left->OnCollisionEnter(right);
					right->OnCollisionEnter(left);
					iter->second = true;
				}
				else
				{
					left->OnCollisionStay(right);
					right->OnCollisionStay(left);
				}
			}
			else
			{
				if (iter->second)
				{
					left->OnCollisionExit(right);
					right->OnCollisionExit(left);
					iter->second = false;
				}
			}
		}
		
	}
	bool CollisionManager::Intersect(Collider* left, Collider* right)
	{
		Transform* leftTransform = left->GetOwner()->GetComponent<Transform>();
		Transform* rightTransform = right->GetOwner()->GetComponent<Transform>();

		Vector2 leftPos = leftTransform->GetPosition() + left->GetOffset();
		Vector2 rightPos = rightTransform->GetPosition() + right->GetOffset();

		Vector2 leftSize = left->GetSize() * 100.0f;
		Vector2 rightSize = right->GetSize() * 100.0f;
		
		eColliderType leftType = left->GetColliderType();
		eColliderType rightType = right->GetColliderType();

		if (leftType == eColliderType::Rect2D
			&& rightType == eColliderType::Rect2D)
		{
			if (fabs(leftPos.x - rightPos.x) < fabs(leftSize.x / 2.0f + rightSize.x / 2.0f)
				&& fabs(leftPos.y - rightPos.y) < fabs(leftSize.y / 2.0f + rightSize.y / 2.0f))
				return true;
		}
		
		if (leftType == eColliderType::Circle2D
			&& rightType == eColliderType::Circle2D)
		{
			Vector2 leftCirlcePos = leftPos + (leftSize / 2.0f);
			Vector2 rightCirclePos = rightPos + (rightSize / 2.0f);

			float distance = (leftCirlcePos - rightCirclePos).length();
			if (distance <= (leftSize.x / 2.0f + rightSize.x / 2.0f))
				return true;
		}

		if ((leftType == eColliderType::Circle2D && rightType == eColliderType::Rect2D)
			|| (leftType == eColliderType::Rect2D && rightType == eColliderType::Circle2D))
		{
			Vector2 circleCenter;
			float circleRadius;

			// 1. 원의 중심과 반지름찾기
			if (leftType == eColliderType::Circle2D)
			{
				circleCenter = leftPos + (leftSize / 2.0f);
				circleRadius = leftSize.x / 2.0f;
			}
			else
			{
				circleCenter = rightPos + (rightSize / 2.0f);
				circleRadius = rightSize.x / 2.0f;
			}

			float rectLeft;
			float rectRight;
			float rectTop;
			float rectBottom;
			// 2. 사각형의 4곳찾기
			if (leftType == eColliderType::Rect2D)
			{
				rectLeft = leftPos.x;
				rectRight = leftPos.x + leftSize.x;
				rectTop = leftPos.y;
				rectBottom = leftPos.y + leftSize.y;
			}
			else
			{
				rectLeft = rightPos.x;
				rectRight = rightPos.x + rightSize.x;
				rectTop = rightPos.y;
				rectBottom = rightPos.y + rightSize.y;
			}


			// 3. 원의 중심에서 가장 가까운 사각형 내부의점 찾기
			float closestX = std::clamp(circleCenter.x, std::fmin(rectLeft,rectTop), std::fmax(rectRight,rectLeft));
			float closestY = std::clamp(circleCenter.y, std::fmin(rectTop,rectLeft), std::fmax(rectBottom,rectTop));

			// 4. 원의중심과 가장가까운 점의 거리찾기
			float distance = (circleCenter - Vector2(closestX, closestY)).length();


			
			// 5. 거리가 반지름보다 작으면 충돌
			if (distance - circleRadius <= std::numeric_limits<float>::epsilon())
				return true;
		}


		return false;
	}
}