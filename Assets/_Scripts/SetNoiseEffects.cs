using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace GoNatureAR
{
    public class SetNoiseEffects : MonoBehaviour
    {
        public static SetNoiseEffects Instance;
        private void Awake()
        {
            if (Instance != null)
                Destroy(this);
            else
                Instance = this;
        }

        [SerializeField] VisualEffect vfx_Noise;

        [ColorUsage(true, true)]
        [SerializeField] Color color;
        [ColorUsage(true, true)]
        [SerializeField] Color color2;

        [SerializeField] float size;
        [SerializeField] float spawnRate;
        [SerializeField] float trailSpawnRate;
        [SerializeField] float trailLifeTime;

        [SerializeField] AudioSource musicAudioSource;
        [SerializeField] AudioSource effectsAudioSource;

        [SerializeField] AudioClip musicAudioClip0;
        [SerializeField] AudioClip musicAudioClip;
        [SerializeField] AudioClip effectsAudioClip;

        [SerializeField] TextAnim textAnimator;
        [SerializeField] AudioSource companionAudioSource;
        [SerializeField] AudioClip companionAudioClip;
        [SerializeField] AudioClip companionAudioClip2;
        [SerializeField] AudioClip companionAudioClip3;


        Vector4 color_vect;
        Vector4 color2_vect;

        public void InitNoiseScene()
        {
            color_vect = new Vector4(color.r, color.g, color.b, color.a);
            color2_vect = new Vector4(color2.r, color2.g, color2.b, color2.a);

            musicAudioSource.Stop();

            StartCoroutine(InitializeScene());
        }

        IEnumerator InitializeScene()
        {
            yield return new WaitForSeconds(3);

            vfx_Noise.gameObject.SetActive(true);
            effectsAudioSource.Play();
            musicAudioSource.PlayOneShot(musicAudioClip0);

            companionAudioSource.Stop();
            companionAudioSource.PlayOneShot(companionAudioClip);
            textAnimator.TypeText("Look, this place is so nice and calm!\nCan you feel it too?");


        }
        public void NoiseTransition()
        {
            StartCoroutine(CoNoiseTransition());
        }

        IEnumerator CoNoiseTransition()
        {
            musicAudioSource.Stop();
            effectsAudioSource.Stop();

            yield return new WaitForSeconds(1.0f);

            companionAudioSource.Stop();
            companionAudioSource.PlayOneShot(companionAudioClip2);
            textAnimator.TypeText("This is not your place though...\nYour place feels more like this.");

            yield return new WaitForSeconds(0.5f);
            StartCoroutine(SetNoiseEffect());

            yield return new WaitForSeconds(5f);
            companionAudioSource.Stop();
            companionAudioSource.PlayOneShot(companionAudioClip3);
            textAnimator.TypeText("Noise pollution is a major and increasing threat to human health.\n" +
                "It can cause damage in many ways: \n" +
                "1. High blood pressure; \n" +
                "2. Headaches;\n" +
                "3. Anxiety and\n" +
                "4. Fatigue, are some among others.\n" +
                "Vegetation has been considered a mean to reduce outdoor noise pollution.\n" +
                "Well designed green spaces can effectively reduce our perception of noise.");

            yield return new WaitForSeconds(30);

            //Credits
            Credits.Instance.EnableCredits();
        }

        IEnumerator SetNoiseEffect()
        {
            vfx_Noise.SetVector4("Color", color_vect);
            vfx_Noise.SetVector4("Color02", color2_vect);

            vfx_Noise.SetFloat("Size", size);
            vfx_Noise.SetFloat("SpawnRate", spawnRate);
            vfx_Noise.SetFloat("TrailsSpawnRate", trailSpawnRate);
            vfx_Noise.SetFloat("TrailsLifetime", trailLifeTime);

            musicAudioSource.PlayOneShot(musicAudioClip);
            yield return new WaitForSeconds(2f);
            effectsAudioSource.PlayOneShot(effectsAudioClip);
        }

        private float tempSize = 1;
        IEnumerator DisableNoiseEffect()
        {
            while (tempSize >= 0)
            {
                vfx_Noise.SetFloat("Size", tempSize);
                yield return new WaitForSeconds(1f);

                tempSize = tempSize - 0.33f;
            }
        }
    }
}
