using Sirenix.OdinInspector;
using Tempname.Input;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace HorrorGame
{
    public class CharacterFlashLight : MonoBehaviour
    {
        [SerializeField] protected InputReader inputReader;

        [ChildGameObjectsOnly]
        [SerializeField] private Light2D flashLight;
        [SerializeField] protected float flashLightLength = 3f;
        [SerializeField] private float flashLightAngle = 70f;
        [ColorUsage(true, true)]
        [SerializeField] private Color flashLightColor = Color.white;

        private Camera cam;

        private void OnEnable()
        {
            inputReader.lookEvent += OnLook;
        }

        private void OnDisable()
        {
            inputReader.lookEvent -= OnLook;
        }

        private void Awake()
        {
            cam = GetComponentInChildren<Camera>();
        }

        private void Start()
        {
            flashLight.pointLightOuterRadius = flashLightLength;
            flashLight.pointLightOuterAngle = flashLightAngle;
            flashLight.color = flashLightColor;
        }

        private void OnLook(Vector2 look)
        {
            var difference = cam.ScreenToWorldPoint(look) - flashLight.transform.position;
            difference.Normalize();
            var angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            flashLight.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }
}