using System;
using Mirror;
using Tempname.Input;
using UnityEngine;

namespace HorrorGame
{
    public abstract class Character : NetworkBehaviour
    {
        protected static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [SerializeField] protected InputReader inputReader;

        [SerializeField] protected float movementSpeed = 5f;

        protected Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }
}