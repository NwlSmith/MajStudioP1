#if (UNITY_WSA && !UNITY_EDITOR) && ENABLE_WINMD_SUPPORT //|| CT_DEVELOP
using UnityEngine;
using System.Collections;
using System.Linq;

namespace Crosstales.RTVoice.Provider
{
   /// <summary>WSA (UWP) voice provider.</summary>
   public class VoiceProviderWSA : BaseVoiceProvider
   {
      #region Variables

      private static VoiceProviderWSA instance;

      private static RTVoiceUWPBridge ttsHandler;

      private bool isLoading;

      #endregion


      #region Properties

      /// <summary>Returns the singleton instance of this class.</summary>
      /// <returns>Singleton instance of this class.</returns>
      public static VoiceProviderWSA Instance
      {
         get
         {
            if (instance == null)
               instance = new VoiceProviderWSA();

            return instance;
         }
      }

      public override string AudioFileExtension => ".wav";

      public override AudioType AudioFileType => AudioType.WAV;

      public override string DefaultVoiceName => "Microsoft David";

      public override bool isWorkingInEditor => false;

      public override bool isWorkingInPlaymode => false;

      public override int MaxTextLength => 64000;

      public override bool isSpeakNativeSupported => false;

      public override bool isSpeakSupported => true;

      public override bool isPlatformSupported => Util.Helper.isWSABasedPlatform;

      public override bool isSSMLSupported => true;

      public override bool isOnlineService => false;

      public override bool hasCoRoutines => true;

      public override bool isIL2CPPSupported => true;

      public override bool hasVoicesInEditor => false;

      #endregion


      #region Constructor

      /// <summary>Constructor for VoiceProviderWSA.</summary>
      private VoiceProviderWSA()
      {
         //Util.Config.DEBUG = true; //only for tests

         Load();
      }

      #endregion


      #region Implemented methods

      public override void Load(bool forceReload = false)
      {
         if (cachedVoices?.Count == 0 || forceReload)
         {
            if (!isLoading)
            {
               isLoading = true;

               if (ttsHandler == null)
               {
                  if (Util.Constants.DEV_DEBUG)
                     Debug.Log("Initializing TTS...");

                  ttsHandler = new RTVoiceUWPBridge();
                  ttsHandler.DEBUG = Util.Config.DEBUG;
               }

               Speaker.Instance.StartCoroutine(getVoices());
            }
         }
         else
         {
            onVoicesReady();
         }
      }

      public override IEnumerator SpeakNative(Model.Wrapper wrapper)
      {
         yield return speak(wrapper, true);
      }

      public override IEnumerator Speak(Model.Wrapper wrapper)
      {
         yield return speak(wrapper, false);
      }

      public override IEnumerator Generate(Model.Wrapper wrapper)
      {
         if (wrapper == null)
         {
            Debug.LogWarning("'wrapper' is null!");
         }
         else
         {
            if (string.IsNullOrEmpty(wrapper.Text))
            {
               Debug.LogWarning("'wrapper.Text' is null or empty: " + wrapper);
               yield return null;
            }
            else
            {
               yield return null; //return to the main process (uid)

               ttsHandler.isBusy = true;

               string voiceName = getVoiceName(wrapper);
               string outputFile = getOutputFile(wrapper.Uid, true);
               string path = Application.persistentDataPath.Replace('/', '\\');

               //ttsHandler.SynthesizeToFile(prepareText(wrapper), Application.persistentDataPath.Replace('/', '\\'), Util.Constants.AUDIOFILE_PREFIX + wrapper.Uid + AudioFileExtension, voiceName);
               UnityEngine.WSA.Application.InvokeOnUIThread(() => { ttsHandler.SynthesizeToFile(prepareText(wrapper), path, Util.Constants.AUDIOFILE_PREFIX + wrapper.Uid + AudioFileExtension, voiceName); }, false);
               //UnityEngine.WSA.Application.InvokeOnAppThread(() => { ttsHandler.SynthesizeToFile(prepareText(wrapper), path, Util.Constants.AUDIOFILE_PREFIX + wrapper.Uid + AudioFileExtension, voiceName); }, false);

               silence = false;

               onSpeakAudioGenerationStart(wrapper);

               do
               {
                  yield return null;
               } while (!silence && ttsHandler.isBusy);

               //Debug.Log("FILE: " + "file://" + outputFile + "/" + wrapper.Uid + extension);

               processAudioFile(wrapper, outputFile);
            }
         }
      }

      public override void Silence()
      {
         UnityEngine.WSA.Application.InvokeOnUIThread(() => { ttsHandler.StopNative(); }, false);
         //UnityEngine.WSA.Application.InvokeOnAppThread(() => { ttsHandler.StopNative(); }, false);
         base.Silence();
      }

      #endregion


      #region Private methods

      private IEnumerator getVoices()
      {
         try
         {
            System.Collections.Generic.List<Model.Voice> voices = new System.Collections.Generic.List<Model.Voice>(70);
            string[] myStringVoices = ttsHandler.Voices;
            string name;

            foreach (string voice in myStringVoices)
            {
               string[] currentVoiceData = voice.Split(';');
               name = currentVoiceData[0];
               Model.Voice newVoice = new Model.Voice(name, "UWP voice: " + voice, Util.Helper.WSAVoiceNameToGender(name), "unknown", currentVoiceData[1]);
               voices.Add(newVoice);
            }

            cachedVoices = voices.OrderBy(s => s.Name).ToList();

            if (Util.Constants.DEV_DEBUG)
               Debug.Log("Voices read: " + cachedVoices.CTDump());
         }
         catch (System.Exception ex)
         {
            string errorMessage = "Could not get any voices!" + System.Environment.NewLine + ex;
            Debug.LogError(errorMessage);
            onErrorInfo(null, errorMessage);
         }

         yield return null;

         isLoading = false;

         onVoicesReady();
      }

      private IEnumerator speak(Model.Wrapper wrapper, bool isNative)
      {
         if (wrapper == null)
         {
            Debug.LogWarning("'wrapper' is null!");
         }
         else
         {
            if (string.IsNullOrEmpty(wrapper.Text))
            {
               Debug.LogWarning("'wrapper.Text' is null or empty: " + wrapper);
            }
            else
            {
               if (wrapper.Source == null)
               {
                  Debug.LogWarning("'wrapper.Source' is null: " + wrapper);
               }
               else
               {
                  yield return null; //return to the main process (uid)

                  ttsHandler.isBusy = true;

                  string voiceName = getVoiceName(wrapper);
                  string outputFile = getOutputFile(wrapper.Uid, true);
                  string path = Application.persistentDataPath.Replace('/', '\\');

                  //ttsHandler.SynthesizeToFile(prepareText(wrapper), path, Util.Constants.AUDIOFILE_PREFIX + wrapper.Uid + AudioFileExtension, voiceName);
                  UnityEngine.WSA.Application.InvokeOnUIThread(() => { ttsHandler.SynthesizeToFile(prepareText(wrapper), path, Util.Constants.AUDIOFILE_PREFIX + wrapper.Uid + AudioFileExtension, voiceName); }, false);
                  //UnityEngine.WSA.Application.InvokeOnAppThread(() => { ttsHandler.SynthesizeToFile(prepareText(wrapper), path, Util.Constants.AUDIOFILE_PREFIX + wrapper.Uid + AudioFileExtension, voiceName); }, false);

                  silence = false;

                  if (!isNative)
                  {
                     onSpeakAudioGenerationStart(wrapper);
                  }

                  do
                  {
                     yield return null;
                  } while (!silence && ttsHandler.isBusy);

                  yield return playAudioFile(wrapper, Util.Constants.PREFIX_FILE + outputFile, outputFile, AudioType.WAV, isNative);
               }
            }
         }
      }

      private static string prepareText(Model.Wrapper wrapper)
      {
         //TEST
         //wrapper.ForceSSML = false;

         if (wrapper.ForceSSML && !Speaker.Instance.AutoClearTags)
         {
            System.Text.StringBuilder sbXML = new System.Text.StringBuilder();

            sbXML.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            sbXML.Append("<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"");
            sbXML.Append(wrapper.Voice == null ? "en-US" : wrapper.Voice.Culture);
            sbXML.Append("\">");

            if (Mathf.Abs(wrapper.Rate - 1f) > Common.Util.BaseConstants.FLOAT_TOLERANCE ||
                Mathf.Abs(wrapper.Pitch - 1f) > Common.Util.BaseConstants.FLOAT_TOLERANCE ||
                Mathf.Abs(wrapper.Volume - 1f) > Common.Util.BaseConstants.FLOAT_TOLERANCE)
            {
               sbXML.Append("<prosody");

               if (Mathf.Abs(wrapper.Rate - 1f) > Common.Util.BaseConstants.FLOAT_TOLERANCE)
               {
                  float _rate = wrapper.Rate > 1 ? (wrapper.Rate - 1f) * 0.5f : wrapper.Rate - 1f;

                  sbXML.Append(" rate=\"");
                  sbXML.Append(_rate >= 0f
                     ? _rate.ToString("+#0%", Util.Helper.BaseCulture)
                     : _rate.ToString("#0%", Util.Helper.BaseCulture));

                  sbXML.Append("\"");
               }

               if (Mathf.Abs(wrapper.Pitch - 1f) > Common.Util.BaseConstants.FLOAT_TOLERANCE)
               {
                  float _pitch = wrapper.Pitch - 1f;

                  sbXML.Append(" pitch=\"");
                  sbXML.Append(_pitch >= 0f
                     ? _pitch.ToString("+#0%", Util.Helper.BaseCulture)
                     : _pitch.ToString("#0%", Util.Helper.BaseCulture));

                  sbXML.Append("\"");
               }

               if (Mathf.Abs(wrapper.Volume - 1f) > Common.Util.BaseConstants.FLOAT_TOLERANCE)
               {
                  sbXML.Append(" volume=\"");
                  sbXML.Append((100 * wrapper.Volume).ToString("#0", Util.Helper.BaseCulture));

                  sbXML.Append("\"");
               }

               sbXML.Append(">");

               sbXML.Append(wrapper.Text);

               sbXML.Append("</prosody>");
            }
            else
            {
               sbXML.Append(wrapper.Text);
            }

            sbXML.Append("</speak>");

            return getValidXML(sbXML.ToString());
         }

         return wrapper.Text;
      }

      #endregion


      #region Editor-only methods

#if UNITY_EDITOR

      public override void GenerateInEditor(Model.Wrapper wrapper)
      {
         Debug.LogError("'GenerateInEditor' is not supported for UWP (WSA)!");
      }

      public override void SpeakNativeInEditor(Model.Wrapper wrapper)
      {
         Debug.LogError("'SpeakNativeInEditor' is not supported for UWP (WSA)!");
      }

#endif

      #endregion
   }
}
#endif
// © 2016-2020 crosstales LLC (https://www.crosstales.com)