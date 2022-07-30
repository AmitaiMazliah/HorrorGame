using UnityEngine;

public class FrameRateUpdater : MonoBehaviour
{
    [SerializeField] int targetFrameRateStandalone = 120;
    [SerializeField] int targetFrameRateMobile = 60;

    private void Update()
    {
        if (Application.isMobilePlatform && !Application.isEditor) Application.targetFrameRate = targetFrameRateMobile;
        else Application.targetFrameRate = targetFrameRateStandalone;
    }
}
