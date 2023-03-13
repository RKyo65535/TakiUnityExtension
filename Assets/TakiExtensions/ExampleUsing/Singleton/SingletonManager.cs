using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.ExampleUsing.Singleton
{
    public class SingletonManager : MonoBehaviour
    {

        [SerializeField] Button button;

        private void Awake()
        {
            button.onClick.AddListener(CountUp);
        }


        void CountUp()
        {
            SingltonCounter.Instance.AddCount();
            SingltonCounter2.Instance.AddCount();
            Debug.Log(SingltonCounter.Instance.GetCount());
            Debug.Log(SingltonCounter2.Instance.GetCount());

        }



    }
}