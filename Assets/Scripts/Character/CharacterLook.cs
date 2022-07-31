using Tempname.Input;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace HorrorGame
{
    public class CharacterLook : MonoBehaviour
    {
        [SerializeField] protected InputReader inputReader;

        [SerializeField] protected float flashLightLength = 3f;
        [SerializeField] private float flashLightAngle = 70f;

        private Camera cam;
        private Light2D flashLight;

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
            cam = Camera.main;
            flashLight = GetComponentInChildren<Light2D>();

            flashLight.pointLightOuterRadius = flashLightLength;
            flashLight.pointLightOuterAngle = flashLightAngle;
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