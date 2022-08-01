using Tempname.Events;
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
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        [SerializeField] private GameSceneSO persistentManagersScene;
        [SerializeField] private GameSceneSO lobbyScene;

        [SerializeField] private LoadEventChannelSO loadSceneEvent;
        
        [Header("Loading settings")]
        [SerializeField] private bool showLoadScreen;

        private void Start()
        {
            Logger.Info("Starting to load persistent managers scene");
            persistentManagersScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, showLoadScreen).Completed += LoadEventChannel;
        }

        private void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
        {
            Logger.Info("Successfully loaded persistent managers scene removing init scene");
            loadSceneEvent.RaiseEvent(lobbyScene, true);
            SceneManager.UnloadSceneAsync(0);
        }
    }
}
