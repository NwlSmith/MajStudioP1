// https://github.com/unitycoder/UnityRuntimeTextToSpeech

using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using System.Collections;

namespace UnityWebGLSpeechSynthesis
{

    // run before regular scripts
    [DefaultExecutionOrder(-100)]
    public class TTSManager : MonoBehaviour
    {

        // singleton isntance
        public static TTSManager instance = null;


        private ISpeechSynthesisPlugin _mSpeechSynthesisPlugin = null;
        private SpeechSynthesisUtterance _mSpeechSynthesisUtterance = null;

        private VoiceResult _mVoiceResult = null;
        private List<Voice> voiceOptions = null;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }

        // Use this for initialization
        IEnumerator Start()
        {
            Debug.Log("start!");

#if UNITY_WEBGL && !UNITY_EDITOR
            Debug.Log("In webgl!");
            // get singleton instance
            _mSpeechSynthesisPlugin = WebGLSpeechSynthesisPlugin.GetInstance();
#endif
#if UNITY_EDITOR
            Debug.Log("In Editor!");
            // get the singleton instance
            _mSpeechSynthesisPlugin = ProxySpeechSynthesisPlugin.GetInstance();

            int port = 5000;
            _mSpeechSynthesisPlugin.ManagementSetProxyPort(port);

            _mSpeechSynthesisPlugin.ManagementOpenBrowserTab();

            _mSpeechSynthesisPlugin.ManagementCloseBrowserTab();

            _mSpeechSynthesisPlugin.ManagementCloseProxy();
#endif
            if (null == _mSpeechSynthesisPlugin)
            {
                Debug.LogError("WebGL Speech Synthesis Plugin is not set!");
                yield break;
            }

            // wait for proxy to become available
            while (!_mSpeechSynthesisPlugin.IsAvailable())
            {
                yield return null;
            }

            // Create an instance of SpeechSynthesisUtterance
            _mSpeechSynthesisPlugin.CreateSpeechSynthesisUtterance((utterance) =>
            {
                //Debug.LogFormat("Utterance created: {0}", utterance._mReference);
                _mSpeechSynthesisUtterance = utterance;
            });

            _mSpeechSynthesisPlugin.GetVoices((voiceResult) =>
            {
                _mVoiceResult = voiceResult;
            });

            voiceOptions = new List<Voice>();

            if (null != _mVoiceResult && null != _mVoiceResult.voices)
            {
                for (int i = 0; i < _mVoiceResult.voices.Length; ++i)
                {
                    Voice voice = _mVoiceResult.voices[i];
                    if (null == voice)
                    {
                        continue;
                    }

                    voiceOptions.Add(voice);
                }
            }
        }

        public void Say(string text, int voiceNum)
        {
            StartCoroutine(SayEnum(text, voiceNum));
        }

        public IEnumerator SayEnum(string text, int voiceNum)
        {
            // Cancel if already speaking
            _mSpeechSynthesisPlugin.Cancel();

            if (_mSpeechSynthesisUtterance == null)
            {
                Debug.Log("Utterance not made yet");
                yield break;
            }

            if (voiceOptions != null && voiceOptions.Count > voiceNum && voiceOptions[voiceNum] != null)
                _mSpeechSynthesisPlugin.SetVoice(_mSpeechSynthesisUtterance, voiceOptions[voiceNum]);

            yield return null;

            // Set the text that will be spoken
            _mSpeechSynthesisPlugin.SetText(_mSpeechSynthesisUtterance, text);

            yield return null;

            // Use the plugin to speak the utterance
            _mSpeechSynthesisPlugin.Speak(_mSpeechSynthesisUtterance);
        }

    } // class
} // namespace