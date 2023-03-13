using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Assets.TakiExtension.SoundAdjustment
{
    public class SoundAdjustment : MonoBehaviour
    {
        [SerializeField] AudioMixer mixer;
        [SerializeField] string mixerParameter_BGMVolume;
        [SerializeField] string mixerParameter_SEVolume;

        [SerializeField] Slider BGMSlider;
        [SerializeField] Slider SESlider;
        [SerializeField,] AudioSource SETestAudio;

        private void Awake()
        {
            BGMSlider.onValueChanged.AddListener(BGMChanged);
            SESlider.onValueChanged.AddListener(SEChanged);

            //ここ修正ポイント、値が変わるたびに何回も何回も鳴る
            SESlider. onValueChanged.AddListener(_ =>
            {
                SETestAudio?.Play();
            });
        }


        void BGMChanged(float slider)
        {

            mixer.SetFloat("BGMVolume", Liner2dB(slider));
        }

        void SEChanged(float slider)
        {
            mixer.SetFloat("SEVolume", Liner2dB(slider));

        }


        float Liner2dB(float liner)
        {
            if (liner < 0.001f)
            {
                liner = 0.001f;
            }
            return 20 * Mathf.Log10(liner);
        }


    }

}