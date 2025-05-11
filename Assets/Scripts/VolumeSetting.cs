using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    [Header("UI Slider")]
    public Slider volumeSlider;

    [Header("Audio Mixer")]
    public AudioMixer mixer;

    [Header("Parameter Name in Mixer")]
    public string volumeParameter = "Volume"; // ชื่อ parameter ที่ expose ใน Mixer

    private void Start()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(SetVolume);

            // โหลดค่าที่เคยตั้งไว้ (optional)
            float savedVolume = PlayerPrefs.GetFloat("volume", 0.75f);
            volumeSlider.value = savedVolume;
            SetVolume(savedVolume);
        }
    }

    public void SetVolume(float sliderValue)
    {
        // แปลงจาก 0-1 → -80 ถึง 0 dB
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1)) * 20;
        mixer.SetFloat(volumeParameter, dB);

        // บันทึกค่าลง PlayerPrefs (optional)
        PlayerPrefs.SetFloat("volume", sliderValue);
    }
}