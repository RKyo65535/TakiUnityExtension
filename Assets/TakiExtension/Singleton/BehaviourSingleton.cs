using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


//まだ未検証です
namespace Assets.TakiExtension.Singleton
{
    public abstract class BehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {

        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {

                    Type t = typeof(T);

                    instance = (T)FindObjectOfType(t);
                    if (instance == null)
                    {
                        Debug.LogError(t + " をアタッチしているGameObjectはありません");
                    }
                }

                return instance;
            }
        }

        virtual protected void Awake()
        {

            if(instance == null)
            {
                instance = GetComponent<T>();
                DontDestroyOnLoad(this.gameObject);
                LateAwake();
            }
            else if (this == Instance)
            {
                LateAwake();
                return;
            }
            else
            {
                Debug.LogError(
                    typeof(T) +
                    " は既に他のGameObjectにアタッチされているため、コンポーネントを破棄しました." +
                    " アタッチされているGameObjectは " + Instance.gameObject.name + " です.");
                Destroy(this);
            }

            // なんとかManager的なSceneを跨いでこのGameObjectを有効にしたい場合は
            // ↓コメントアウト外してください.
        }

        protected abstract void LateAwake();
    }
}