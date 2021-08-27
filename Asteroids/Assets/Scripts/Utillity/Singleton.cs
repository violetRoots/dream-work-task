using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance { get; protected set; }

        public virtual void Awake()
        {
            if (FindObjectsOfType<T>().Length > 1)
            {
                Debug.LogWarningFormat("scene {0} contains duplicate {1}", SceneManager.GetActiveScene().name, name);
                DestroyImmediate(gameObject);
                return;
            }

            Instance = (T)this;
        }
    }
}
