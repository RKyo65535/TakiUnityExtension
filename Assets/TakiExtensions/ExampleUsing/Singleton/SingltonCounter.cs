using System.Collections;
using TakiExtensions.TakiExtension.Singleton;
using UnityEngine;

namespace Assets.ExampleUsing.Singleton
{
    public class SingltonCounter : MonoBehaviourSingleton<SingltonCounter>
    {
        protected override void LateAwake()
        {
            //何もしない
        }

        int count=0;

        public void AddCount()
        {
            count += 1;
        } 

        public int GetCount()
        {
            return count;
        }


    }
}