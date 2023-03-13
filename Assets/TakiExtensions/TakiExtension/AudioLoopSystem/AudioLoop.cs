using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class AudioLoop : MonoBehaviour
{

    AudioSource audioSource;
    [SerializeField,Header("ループ開始地点のサンプリング数")] int loopStartPoint;
    [SerializeField, Header("ループ終了地点のサンプリング数")] int loopEndPoint;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource.timeSamples > loopEndPoint)
        {
            audioSource.timeSamples -= (loopEndPoint - loopStartPoint);
        }
    }
}
