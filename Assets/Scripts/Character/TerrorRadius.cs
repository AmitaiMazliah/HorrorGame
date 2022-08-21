using System;
using Mirror;
using Sirenix.OdinInspector;
using Tempname.Audio;
using Tempname.Events;
using UnityEngine;

namespace HorrorGame
{
    public class TerrorRadius : NetworkBehaviour
    {
        [SerializeField] private float radius = 5;

        [Title("Audio")]
        [SerializeField] private SoundEmitter soundEmitter;
        [SerializeField] private AudioConfigurationSO audioConfig;
        [SerializeField] private AudioCueSO terrorRadiusSound;

        public override void OnStartClient()
        {
            // if (!isLocalPlayer)
            // {
            soundEmitter.PlayAudioClip(terrorRadiusSound.GetClips()[0], audioConfig, true);
            // }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
