#if (UNITY_WSA && !UNITY_EDITOR) && ENABLE_WINMD_SUPPORT //|| CT_DEVELOP
using System;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Crosstales.RTVoice
{
   /// <summary>WSA (UWP) TTS bridge.</summary>
   public sealed class RTVoiceUWPBridge : IDisposable
   {
      #region Variables

      private static SpeechSynthesizer TTS = new SpeechSynthesizer();
      private static MediaElement mediaElement;

      private static StorageFolder targetFolder = ApplicationData.Current.LocalFolder;
      //private static StorageFolder logFolder = ApplicationData.Current.LocalFolder;
      //private static StorageFile logFile;
      //private System.Collections.Generic.List<MediaElement> mediaElements = new System.Collections.Generic.List<MediaElement>();

      #endregion


      #region Constructor

      public RTVoiceUWPBridge()
      {
         initializeTTS();
      }

      #endregion


      #region Properties

      /// <summary>
      /// Indicates if the TTS-Engine is currently busy.
      /// </summary>
      /// <returns>True if the TTS-Engine is currently busy.</returns>
      public bool isBusy { get; set; }

      /// <summary>
      /// Indicates if the TTS-Engine is currently busy in native mode.
      /// </summary>
      /// <returns>True if the TTS-Engine is currently busy in native mode.</returns>
      public bool isBusyNative { get; set; }

      /// <summary>
      /// Returns the target Folder of the last Speak call.
      /// If there hasn't been a Speak call so far, returns ApplicationData.Current.LocalFolder.
      /// </summary>
      /// <returns>The target Folder of the last Speak call.</returns>
      public static string TargetFolder
      {
         get
         {
            /*
            if (targetFolder == null)
            {
                targetFolder = ApplicationData.Current.LocalFolder;
            }
            */

            return targetFolder.Path;
         }
      }

      /// <summary>
      /// Returns the available voices.
      /// </summary>
      /// <returns>Available voices as string-array. Format: DisplayName;Language</string></returns>
      public string[] Voices
      {
         get
         {
            System.Collections.Generic.List<string> result = new System.Collections.Generic.List<string>();

            foreach (VoiceInformation Voice in SpeechSynthesizer.AllVoices)
            {
               result.Add(Voice.DisplayName + ";" + Voice.Language);
            }

            return result.ToArray();
         }
      }

      /// <summary>
      /// DEBUG mode to on/off
      /// </summary>
      public bool DEBUG { get; set; }

      #endregion


      #region Public Methods

      public async void SpeakNative(string text, /* double rate, */ string voice)
      {
         if (mediaElement != null)
            mediaElement.Stop();

         isBusyNative = true;

         if (mediaElement == null)
         {
            log("INFO", "Creating MediaElement...");
            mediaElement = new MediaElement();
            //mediaElement.MediaEnded += onMediaEnded;
         }

         //MediaElement mediaElement = new MediaElement();
         //mediaElements.Add(mediaElement);

         log("INFO", "Starting the synthesizing process...");

         try
         {
            SpeechSynthesisStream stream = await synthesizeText(text, voice);

            //log("INFO", "Setting rate...");
            //mediaElement.DefaultPlaybackRate = rate;

            log("INFO", "Setting stream as source...");
            mediaElement.SetSource(stream, stream.ContentType);

            log("INFO", "Playing the MediaElement");
            mediaElement.Play();

            bool result = await waitWhileSpeaking();

            log("INFO", "Done!");
         }
         catch (Exception ex)
         {
            log("ERROR", "Could not speak native: " + ex);
         }

         isBusyNative = false;

         //mediaElements.Remove(mediaElement);
      }

      public void StopNative()
      {
         /*
         log("INFO", "Stopping all MediaElements...");

         foreach (MediaElement mediaElement in mediaElements)
         {
             mediaElement.Stop();
         }
         mediaElements.Clear();
         */

         if (mediaElement != null)
         {
            log("INFO", "Stopping MediaElement...");
            mediaElement.Stop();
         }

         isBusyNative = false;
      }

      /// <summary>
      /// Use the TTS engine to write the voice clip into a pre-defined Folder.
      /// </summary>
      /// <param name="text">Spoken text</param>
      /// <param name="path">Target folder</param>
      /// <param name="fileName">File name</param>
      /// <param name="voice">Desired voice</param>
      public async void SynthesizeToFile(string text, string path, string fileName, string voice)
      {
         isBusy = true;

         log("INFO", "SynthesizeToFile: " + path);

         try
         {
            targetFolder = await StorageFolder.GetFolderFromPathAsync(path);

            log("INFO", "Starting the synthesizing process...");
            SpeechSynthesisStream stream = await synthesizeText(text, voice);

            log("INFO", "Creating empty Wave file...");
            StorageFile outputFile = await targetFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            log("INFO", "Putting the stream in a DataReader...");
            using (var reader = new DataReader(stream))
            {
               log("INFO", "Loading the stream...");
               await reader.LoadAsync((uint)stream.Size);

               log("INFO", "Buffering...");
               IBuffer buffer = reader.ReadBuffer((uint)stream.Size);

               log("INFO", "Writing buffer into file...");
               await FileIO.WriteBufferAsync(outputFile, buffer);

               log("INFO", "Done!");
            }
         }
         catch (Exception ex)
         {
            log("ERROR", ex.ToString());
         }

         isBusy = false;
      }

      public void Dispose()
      {
         TTS.Dispose();
      }

      #endregion


      #region Private Methods

      /*
      private static void onMediaEnded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
      {
          log("DEBUG", "onMediaEnded: " + mediaElement.CurrentState);
          isBusyNative = false;
      }
      */
      private async Task<bool> waitWhileSpeaking()
      {
         do
         {
            //log("DEBUG", "waitWhileSpeaking: " + mediaElement.CurrentState);
            //await Task.Yield();
            await Task.Delay(50);
         } while (mediaElement.CurrentState == MediaElementState.Playing || mediaElement.CurrentState == MediaElementState.Buffering || mediaElement.CurrentState == MediaElementState.Opening);

         log("DEBUG", "waitWhileSpeaking: " + mediaElement.CurrentState);

         return true;
      }

      private async Task<SpeechSynthesisStream> synthesizeText(string inputText, string inputVoice)
      {
         if (!TTS.Voice.DisplayName.Equals(inputVoice))
         {
            log("INFO", "Search for voice...");
            foreach (VoiceInformation Voice in SpeechSynthesizer.AllVoices)
            {
               if (Voice.DisplayName.Equals(inputVoice))
               {
                  log("INFO", "Found Voice!");
                  TTS.Voice = Voice;
                  break;
               }
               else
               {
                  TTS.Voice = SpeechSynthesizer.AllVoices[0];
               }
            }
         }

         log("INFO", "Calling SynthesizeTextToStreamAsync() to create stream...");

         if (inputText.Contains("</speak>"))
         {
            return await TTS.SynthesizeSsmlToStreamAsync(inputText);
         }

         return await TTS.SynthesizeTextToStreamAsync(inputText);
      }

      private void initializeTTS()
      {
         //log("INFO", "Initializing TTS...");
         TTS = new SpeechSynthesizer();
      }

      //private async void log(string type, string text)
      private void log(string type, string text)
      {
         if (DEBUG || type.Equals("Error", StringComparison.OrdinalIgnoreCase))
         {
            /*
            if (logFile == null)
            {
                logFile = await logFolder.CreateFileAsync("RTVoiceUWPBridge.log", CreationCollisionOption.GenerateUniqueName);
            }

            await FileIO.AppendTextAsync(logFile, type + ": " + text + Environment.NewLine);
            */

            UnityEngine.Debug.Log("RTVoiceUWPBridge - " + type + ": " + text);
         }
      }

      #endregion
   }
}
#endif
// © 2016-2020 crosstales LLC (https://www.crosstales.com)