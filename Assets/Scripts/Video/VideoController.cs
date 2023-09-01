
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage rawImage;

    [SerializeField] private RenderTexture[] renderTextures;

    [SerializeField] private Button startPauseButton;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Dropdown resolutionDropDown;
    [SerializeField] private Toggle loopToggle;
    [SerializeField] private Slider videoSlider;


    private bool _isPlaying;
    private float _currentSpeed;
    private bool _loop;
    
    private void Start()
    {
        videoPlayer.SetTargetAudioSource(0, audioSource);
        
        volumeSlider.value = videoPlayer.GetDirectAudioVolume(0);
        _currentSpeed = 1f;
        speedSlider.value = _currentSpeed;
        
        SetVideoVolume(1f);
        volumeSlider.value = 1f;
        
        _loop = false;
        videoPlayer.isLooping = _loop;
        loopToggle.isOn = _loop;
        
        _isPlaying = true;
        videoPlayer.Play();
    }

    private void Update()
    {
        videoSlider.value = (float)(videoPlayer.time / videoPlayer.length);
    }


    public void PlayPause()
    {
        if (_isPlaying)
        {
            videoPlayer.playbackSpeed = 0f;
            _isPlaying = false;
        }
        else
        {
            videoPlayer.playbackSpeed = _currentSpeed;
            _isPlaying = true;
        }
    }

    public void SetVideoVolume(float value)
    {
        videoPlayer.SetDirectAudioVolume(0, value);
    }

    public void SetVideoSpeed(float value)
    {
        _currentSpeed = value;
        if (_isPlaying)
        {
            videoPlayer.playbackSpeed = _currentSpeed;
        }
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
        Debug.Log(value.ToString());
        _loop = value;
        videoPlayer.isLooping = _loop;
    }

    public void SetPlayTime(float time)
    {
        videoPlayer.Pause();
        videoPlayer.playbackSpeed = 0f;
        
        videoPlayer.time = videoPlayer.length * time;

        videoPlayer.playbackSpeed = _currentSpeed;
        videoPlayer.Play();
    }
}
