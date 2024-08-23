using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperPlatformer
{
    public class EventManager : SingletonGeneric<EventManager>
    {
        public event Action PlayerRespawn;


        public void TriggerPlayerRespawn()
        {
            PlayerRespawn?.Invoke();
        }
    }
}
