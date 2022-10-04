using System.Collections;
using UnityEngine;
using Assets.TakiExtension.Singleton;

namespace Assets.ExampleUsing.Singleton
{
    public class SingltonCounter2 : MonoBehaviourSingleton<SingltonCounter2>
    {
        protected override void LateAwake()
        {
            //何もしない
        }

        int count=1;

        public void AddCount()
        {
            count *= 2;
        } 

        public int GetCount()
        {
            return count;
        }


    }
}