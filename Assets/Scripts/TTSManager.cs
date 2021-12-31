
using UnityEngine;

public class TTSManager : MonoBehaviour
{

    struct VoiceStats
    {
        public VoiceStats(string _name, float _rate, float _pitch)
        {
            name = _name;
            rate = _rate;
            pitch = _pitch;
        }
        public readonly string name;
        public readonly float rate;
        public readonly float pitch;
    }
    
    // singleton instance
    public static TTSManager instance = null;

    private AudioSource _audioSource;

    private StrToAudioClipDictionary _dictionary;

    [SerializeField][Tooltip("Divides the length of the string by this number. Smaller means longer speech.")] private float _speechLengthScaler = 10f;

    [SerializeField] private string _voiceName = "";

    private VoiceStats[] voices = new VoiceStats[3];


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        
        voices[0] = new VoiceStats("Microsoft David Desktop", 1, 1); // Translations
        voices[1] = new VoiceStats("Microsoft Zira Desktop", 1.15f, 1.5f); // Virtual assistant
        voices[2] = new VoiceStats("Microsoft Zira Desktop", .75f, 0f); // Roommate

        _audioSource = GetComponent<AudioSource>();
        _dictionary = FindObjectOfType<StrToAudioClipDictionary>();
        
    }

    private float Say(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
        return clip.length;
    }

    public float Say(string str, int voiceNum = 0)
    {
        //VoiceStats voice = voices[voiceNum];
        //Speaker.Instance.Speak(str, _audioSource, Speaker.Instance.VoiceForName(voice.name), true, voice.rate, voice.pitch);

        AudioClip clip = _dictionary.GetAc(str);

        _audioSource.clip = clip;
        _audioSource.Play();

        return EstimatedSpeechLength(str, 1f);
    }

    private float EstimatedSpeechLength(string str, float rate)
    {
        return str.Length / (_speechLengthScaler * rate);
    }

    public void StopSpeaking()
    {
        if (_audioSource != null)
            _audioSource.Stop();
    }
}