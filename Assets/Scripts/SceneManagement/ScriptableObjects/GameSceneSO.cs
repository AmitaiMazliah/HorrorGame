using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Tempname.SceneManagement
{
    /// <summary>
    /// This class is a base class which contains what is common to all game scenes (Locations or Menus)
    /// </summary>
    public class GameSceneSO : DescriptionBaseSO
    {
        [Header("Information")]
        public AssetReference sceneReference;

        [Header("Sounds")]
        public AudioClip music;
    }
}
