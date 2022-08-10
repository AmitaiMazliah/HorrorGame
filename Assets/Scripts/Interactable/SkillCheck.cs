using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HorrorGame
{
    [Serializable]
    public class SkillCheck
    {
        public float fullDuration;
        public float startSafeTime;
        public float goodDuration;
        public float excellentDuration;

        private float startTime;

        [HideLabel, ShowInInspector]
        [ProgressBar(0, nameof(ProgressBarMax), ColorGetter = nameof(GetProgressBarColor))]
        private float ProgressBar => Time.time - startTime;

        private float ProgressBarMax => startTime + fullDuration;

        private Color GetProgressBarColor()
        {
            if (Time.time >= startTime + startSafeTime && Time.time <= startTime + startSafeTime + excellentDuration)
            {
                return Color.green;
            }

            if (Time.time >= startTime + startSafeTime && Time.time <= startTime + startSafeTime + goodDuration)
            {
                return Color.blue;
            }

            return Color.red;
        }

        [Button]
        private void ResetStartTime()
        {
            startTime = Time.time;
        }
    }
}