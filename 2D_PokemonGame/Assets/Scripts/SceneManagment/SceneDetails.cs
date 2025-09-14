using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class SceneDetails : MonoBehaviour
{
    [SerializeField] List<SceneDetails> connectedScenes;
    [SerializeField] AudioClip sceneMusic;

    public AudioClip SceneMusic => sceneMusic;

    public bool IsLoaded { get; private set; }
    List<SavableEntity> savableEntities;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log($"PlayerEntered {gameObject.name}");

            LoadScene();
            GameController.i.SetCurrentScene(this);

            if (sceneMusic != null)
                AudioManager.i.PlayMusic(sceneMusic, fade: true);

            foreach(var scene in connectedScenes)
            {
                scene.LoadScene();
            }

            // Unload the scenes that ar no longer connted
            var prevScene = GameController.i.PrevScene;
            if(prevScene != null)
            {
                var previoslyLoadedScenes = prevScene.connectedScenes;

                foreach(var scene in previoslyLoadedScenes)
                {
                    if (!connectedScenes.Contains(scene) && scene != this)
                        scene.UnLoadScene();
                }

                if(!connectedScenes.Contains(prevScene))
                    prevScene.UnLoadScene();
            }
        }
    }

    public void LoadScene()
    {
        if(!IsLoaded)
        {
            var operation = SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            IsLoaded = true;

            operation.completed += (AsyncOperation op) =>
            {
                savableEntities = GetSavableEntitiesInScene();
                SavingSystem.i.RestoreEntityStates(savableEntities);
            };
        }
    }

    public void UnLoadScene()
    {
        if (IsLoaded)
        {
            SavingSystem.i.CaptureEntityStates(savableEntities);

            SceneManager.UnloadSceneAsync(gameObject.name);
            IsLoaded = false;
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Open Scene")]
    public void OpenSceneInEditor()
    {
        if(!EditorSceneManager.GetSceneByName(gameObject.name).isLoaded)
        {
            string path = $"Assets/Scenes/{gameObject.name}.unity";
            EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
        }
    }

    [ContextMenu("Close Scene")]
    public void CloseSceneInEditor()
    {
        var scene = EditorSceneManager.GetSceneByName(gameObject.name);
        if (scene.isLoaded)
        {
            EditorSceneManager.CloseScene(scene, true);
        }
    }
#endif
    public List<SavableEntity> GetSavableEntitiesInScene()
    {
        var currScene = SceneManager.GetSceneByName(gameObject.name);
        var savableEntities = FindObjectsOfType<SavableEntity>().Where(x => x.gameObject.scene == currScene).ToList();
        return savableEntities;
    }
}
