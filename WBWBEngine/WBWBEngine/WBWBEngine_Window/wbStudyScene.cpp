#include "wbStudyScene.h"
#include "wbGameObject.h"
#include "wbObject.h"
#include "wbTransform.h"
#include "wbSceneManager.h"
#include "wbRect.h"

namespace wb
{
    StudyScene::StudyScene()
    {
    }
    StudyScene::~StudyScene()
    {
    }
    void StudyScene::Initialize()
    {
        Vector2 center = Vector2(640.0f / 2, 480.0f / 2);
        int numOfObjects = 20;
        float radius = 100.0f;

        for (int i = 0; i < numOfObjects; i++)
        {
            float angle = i *2 * 3.14f / numOfObjects;
            GameObject* gameObj = Instantiate<wbRect>(eLayerType::Player, Vector2(center.x + radius * cos(angle), center.y + radius * sin(angle)));
        }

        Scene::Initialize();
    }
    void StudyScene::Update()
    {
        Scene::Update();
    }
    void StudyScene::LateUpdate()
    {
        Scene::LateUpdate();
    }
    void StudyScene::Render(HDC hdc)
    {
        Scene::Render(hdc);
    }
    void StudyScene::OnEnter()
    {
    }
    void StudyScene::OnExit()
    {
    }
}