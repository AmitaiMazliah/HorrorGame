using System;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorGame
{
    [CreateAssetMenu(fileName = "MatchManager", menuName = "Game/Match Manager")]
    public class MatchManager : ScriptableObject
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        
        public Dictionary<string, Survivor> survivors = new(4);
        public Killer killer;

        public void RegisterSurvivor(Survivor survivor)
        {
            survivors.Add(Guid.NewGuid().ToString(), survivor);
        }

        public void RegisterKiller(Killer killer)
        {
            if (this.killer) Logger.Error("Killer already registered");
            else this.killer = killer;
        }
    }
}