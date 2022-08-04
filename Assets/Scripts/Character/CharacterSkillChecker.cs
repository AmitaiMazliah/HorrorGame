using Mirror;
using UnityEngine;

namespace HorrorGame
{
    public class CharacterSkillChecker : NetworkBehaviour
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        
        [TargetRpc]
        public void TargetStartSkillCheck(NetworkConnection target, SkillCheck skillCheck)
        {
            Logger.Info("Started skill check");
        }
    }
}