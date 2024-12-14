using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer; // Reference to the audio mixer
    public Slider muteSlider;     // Reference to the slider

    private const string VolumeKey = "AudioVolume";
    private const float MuteVolume = -80f;
    private const float FullVolume = 0f;

    private void Start()
    {
        // Load the saved volume value or default to unmuted
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);

        // Set the slider and audio volume to the saved value
        muteSlider.value = savedVolume;
        SetAudioVolume(savedVolume);

        // Add listener for slider changes
        muteSlider.onValueChanged.AddListener(SetAudioVolume);
    }

    public void SetAudioVolume(float value)
    {
        if (audioMixer != null)
        {
            // Set the audio mixer volume based on slider value
            float volume = value > 0 ? FullVolume : MuteVolume;
            audioMixer.SetFloat("MasterVolume", volume);

            // Save the new value to PlayerPrefs
            PlayerPrefs.SetFloat(VolumeKey, value);
            PlayerPrefs.Save();
        }
    }

    public void ResetAudioSettings()
    {
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
        muteSlider.value = savedVolume;
        SetAudioVolume(savedVolume);
    }
}
