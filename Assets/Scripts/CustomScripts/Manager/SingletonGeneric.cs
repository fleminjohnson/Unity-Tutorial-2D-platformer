using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperPlatformer
{
    public class SingletonGeneric<T> : MonoBehaviour where T : SingletonGeneric<T>
    {
        private static T instance;
        public static T Instance { get => instance; }

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = (T)this;
            }
        }
    }
}
