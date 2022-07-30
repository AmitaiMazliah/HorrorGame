using System.Collections;
using Tempname.Events;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Tempname.SceneManagement
{
    /// <summary>
    /// This class manages the scene loading and unloading.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        [Header("Load Events")]
        [SerializeField] private LoadEventChannelSO loadScene;

        [Header("Broadcasting on")]
        [SerializeField] private BoolEventChannelSO toggleLoadingScreen;
        [SerializeField] private VoidEventChannelSO onSceneReady;

        private AsyncOperationHandle<SceneInstance> loadingOperationHandle;

        private GameSceneSO sceneToLoad;
        private GameSceneSO currentlyLoadedScene;
        private bool showLoadingScreen;

        private bool isLoading;

        private void OnEnable()
        {
            loadScene.OnLoadingRequested += LoadScene;
        }

        private void OnDisable()
        {
            loadScene.OnLoadingRequested -= LoadScene;
        }

        /// <summary>
        /// This function loads the location scenes passed as array parameter 
        /// </summary>
        /// <param name="sceneToLoad"></param>
        /// <param name="showLoadingScreen"></param>
        private void LoadScene(GameSceneSO sceneToLoad, bool showLoadingScreen)
        {
            if (!isLoading)
            {
                this.sceneToLoad = sceneToLoad;
                this.showLoadingScreen = showLoadingScreen;
                isLoading = true;

                StartCoroutine(UnloadPreviousScene());
            }
        }

        /// <summary>
        /// In both Location and Menu loading, this function takes care of removing previously loaded scenes.
        /// </summary>
        private IEnumerator UnloadPreviousScene()
        {
            // _inputReader.DisableAllInput();
            // _fadeRequestChannel.FadeOut(_fadeDuration);

            yield return new WaitForSeconds(0);

            if (currentlyLoadedScene != null)
            {
                if (currentlyLoadedScene.sceneReference.OperationHandle.IsValid())
                {
                    currentlyLoadedScene.sceneReference.UnLoadScene();
                }
#if UNITY_EDITOR
                else
                {
                    //Only used when, after a "cold start", the player moves to a new scene
                    //Since the AsyncOperationHandle has not been used (the scene was already open in the editor),
                    //the scene needs to be unloaded using regular SceneManager instead of as an Addressable
                    SceneManager.UnloadSceneAsync(currentlyLoadedScene.sceneReference.editorAsset.name);
                }
#endif
            }

            LoadNewScene();
        }

        /// <summary>
        /// Kicks off the asynchronous loading of a scene, either menu or Location.
        /// </summary>
        private void LoadNewScene()
        {
            if (showLoadingScreen)
            {
                toggleLoadingScreen.RaiseEvent(true);
            }

            loadingOperationHandle = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
            loadingOperationHandle.Completed += OnNewSceneLoaded;
        }

        private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
            currentlyLoadedScene = sceneToLoad;

            Scene s = obj.Result.Scene;
            SceneManager.SetActiveScene(s);
            LightProbes.TetrahedralizeAsync();

            isLoading = false;

            if (showLoadingScreen) toggleLoadingScreen.RaiseEvent(false);

            // _fadeRequestChannel.FadeIn(_fadeDuration);

            // StartGameplay();
            onSceneReady.RaiseEvent();
        }

        public void ExitGame()
        {
            Application.Quit();
            Debug.Log("Exit!");
        }
    }
}
