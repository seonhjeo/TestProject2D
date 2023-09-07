
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;
    
    [Header("Video Player")]
    [SerializeField] private VideoPlayer videoPlayer;
    [Header("Video Play Target")]
    [SerializeField] private RawImage rawImage;

    [Header("Resolution Textures")]
    [SerializeField] private RenderTexture[] renderTextures;

    [Header("Control Parts")]
    [SerializeField] private Button startPauseButton;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Dropdown resolutionDropDown;
    [SerializeField] private Toggle loopToggle;
    [SerializeField] private Slider videoSlider;


    // private bool _isPlaying;
    // private float _currentSpeed;
    // private bool _loop;

    #region Unity Methods

    private void Start()
    {
        // add event to parts
        startPauseButton.onClick.AddListener(PlayPause);
        speedSlider.onValueChanged.AddListener(SetVideoSpeed);
        volumeSlider.onValueChanged.AddListener(SetVideoVolume);

        List<string> temp = new List<string>();
        foreach (var v in renderTextures)
        {
            temp.Add(v.name);
        }
        resolutionDropDown.AddOptions(temp);
        temp.Clear();
        resolutionDropDown.onValueChanged.AddListener(SetResolution);
        
        loopToggle.onValueChanged.AddListener(SetLoop);
        videoSlider.onValueChanged.AddListener(SetPlayTime);
        
        
        // Set VideoPlayer default values
        videoPlayer.SetTargetAudioSource(0, audioSource);
        volumeSlider.value = videoPlayer.GetDirectAudioVolume(0);
        
        SetVideoSpeed(1f);
        SetVideoVolume(1f);
        SetLoop(false);
        
        videoPlayer.Prepare();
    }

    private void Update()
    {
        videoSlider.SetValueWithoutNotify((float)(videoPlayer.time / videoPlayer.length));
    }

    private void OnDestroy()
    {
        startPauseButton.onClick.RemoveAllListeners();
        speedSlider.onValueChanged.RemoveAllListeners();
        volumeSlider.onValueChanged.RemoveAllListeners();
        
        resolutionDropDown.ClearOptions();
        resolutionDropDown.onValueChanged.RemoveAllListeners();
        
        loopToggle.onValueChanged.RemoveAllListeners();
        videoSlider.onValueChanged.RemoveAllListeners();
    }

    #endregion
    
    


    public void PlayPause()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
        else
        {
            if (!videoPlayer.isPrepared)
            {
                videoPlayer.Prepare();
                return;
            }
            videoPlayer.Play();
        }
    }

    public void SetVideoVolume(float value)
    {
        videoPlayer.SetDirectAudioVolume(0, value);
        volumeSlider.SetValueWithoutNotify(value);
    }

    public void SetVideoSpeed(float value)
    {
        videoPlayer.playbackSpeed = value;
        speedSlider.SetValueWithoutNotify(value);
        
    }

    public void SetResolution(int value)
    {
        switch (value)
        {
            case 0 : SetImageSize(0);
                break;
            case 1 : SetImageSize(1);
                break;
            case 2 : SetImageSize(2);
                break;
            default: SetImageSize(0);
                break;
        }
    }

    private void SetImageSize(int value)
    {
        videoPlayer.targetTexture = renderTextures[value];
        rawImage.texture = renderTextures[value];
    }

    public void SetLoop(bool value)
    {
        videoPlayer.isLooping = value;
        loopToggle.isOn = value;
    }

    public void SetPlayTime(float time)
    {
        var duration = videoPlayer.frameCount / (ulong)videoPlayer.frameRate;
        videoPlayer.time = time * duration;
    }
}
