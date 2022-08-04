using UnityEngine;

namespace HorrorGame
{
    [CreateAssetMenu(fileName = "Skill check", menuName = "Game/Skill check")]
    public class SkillCheck : ScriptableObject
    {
        public float fullDuration;
        public float goodDuration;
        public float excellentDuration;

        private void Awake()
        {
            
        }

        public void StartSkillCheck()
        {
            
        }
    }
}