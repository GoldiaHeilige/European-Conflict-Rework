using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public class SingletonSelfBehaviour<T> where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<T>();
                }
                if (instance == null)
                {
                    Debug.LogError(typeof(T).Name + " == null");
                }
                return instance;
            }
        }

        public virtual void OnDestroy()
        {
            instance = null;
        }
    }
}
