using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    private void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
        AudioListener.volume = volume;
    }

    private void OnDestroy()
    {
        volumeSlider.onValueChanged.RemoveListener(SetVolume);
    }
}
