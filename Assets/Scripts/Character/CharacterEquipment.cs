using System;
using Mirror;
using Tempname.Input;
using UnityEngine;

namespace HorrorGame
{
    public class CharacterEquipment : NetworkBehaviour
    {
        [SerializeField] private InputReader inputReader;

        private void OnEnable()
        {
            inputReader.useItemEvent += OnUseItem;
        }

        private void OnDisable()
        {
            inputReader.useItemEvent -= OnUseItem;
        }

        private void OnUseItem()
        {
            
        }
    }
}