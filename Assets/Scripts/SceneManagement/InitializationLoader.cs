using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Tempname.SceneManagement
{
    /// <summary>
    /// This class is responsible for starting the game by loading the persistent managers scene 
    /// and raising the event to load the Main Menu
    /// </summary>
    public class InitializationLoader : MonoBehaviour
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [Header("Persistent managers Scene")]
        [SerializeField] private GameSceneSO persistentManagersScene;

        [Header("Loading settings")]
        [SerializeField] private bool showLoadScreen;

        private void Start()
        {
            logger.Info("Starting to load persistent managers scene");
            persistentManagersScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, showLoadScreen).Completed += LoadEventChannel;
        }

        private void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
        {
            logger.Info("Successfully loaded persistent managers scene removing init scene");
            SceneManager.UnloadSceneAsync(0);
        }
    }
}
