using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;

namespace TakiExtension
{
    public static class ObservableExtension
    {

        public static IObservable<T> GetObservableAcync<T>(this IObservable<T> inStream,Func<UniTask> func)
        {
            //入力されたストリームから、実行完了すると完了を通知する自身の新しいストリームを生成する
            IObservable<T> stream = Observable.Create<T>(observer => 
            {
                IDisposable subscription = inStream.Subscribe(async x => 
                {
                    UnityEngine.Debug.Log("stream start");
                    await func();
                    observer.OnCompleted();
                });
                return subscription;
            });

            return stream;
        }


    }
}
