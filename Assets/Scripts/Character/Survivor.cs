using System;
using UnityEngine;

namespace HorrorGame
{
    public class Survivor : Character
    {
        private void OnEnable()
        {
            inputReader.moveEvent += OnMovement;
        }

        private void OnMovement(Vector2 movement)
        {
            logger.Info($"moving {movement}");
            rb.velocity = (movement * movementSpeed);
        }
    }
}