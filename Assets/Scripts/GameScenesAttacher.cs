using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    [Serializable]
    public enum GameScene
    {
        MainMenu = 1,
        RuinedBloodCity = 2
    }

    public static class GameScenesAttacher
    {
        public static GameScene LastAttachedGameScene = GameScene.MainMenu;

        public static AsyncOperation AttachNewSceneAsync(GameScene scene)
        {
            var result = SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive);
            result.completed += operation =>
            {
                GameTime.ResumeTime();
                LastAttachedGameScene = scene;
            };

            return result;
        }

        public static AsyncOperation DetachLastAttachedSceneAsync(GameScene scene)
        {
            return SceneManager.UnloadSceneAsync((int)scene);
        }

        public static void AttachNewSceneWithDetachmentOfLast(
            GameScene scene,
            Action<AsyncOperation> onLastSceneUnloadCompleted = null,
            Action<AsyncOperation> onLastSceneReloadCompleted = null)
        {
            DetachLastAttachedSceneAsync(scene).completed += operation =>
            {
                onLastSceneUnloadCompleted?.Invoke(operation);
                AttachNewSceneAsync(scene).completed
                    += operation => onLastSceneReloadCompleted?.Invoke(operation);
            };
        }

        public static void ReloadLastAttachedSceneAsync(
            Action<AsyncOperation> onLastSceneUnloadCompleted = null,
            Action<AsyncOperation> onLastSceneReloadCompleted = null)
        {
            AttachNewSceneWithDetachmentOfLast(
                LastAttachedGameScene,
                onLastSceneUnloadCompleted,
                onLastSceneReloadCompleted
            );
        }
    }
}