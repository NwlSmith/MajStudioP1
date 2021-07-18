#if UNITY_EDITOR
using UnityEditor;
using Crosstales.RTVoice.EditorUtil;

namespace Crosstales.RTVoice.EditorIntegration
{
   /// <summary>Editor component for the "Tools"-menu.</summary>
   public static class RTVoiceMenu
   {
      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Prefabs/" + Util.Constants.RTVOICE_SCENE_OBJECT_NAME, false, EditorHelper.MENU_ID + 20)]
      private static void AddRTVoice()
      {
         EditorHelper.InstantiatePrefab(Util.Constants.RTVOICE_SCENE_OBJECT_NAME);
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Prefabs/" + Util.Constants.RTVOICE_SCENE_OBJECT_NAME, true)]
      private static bool AddRTVoiceValidator()
      {
         return !EditorHelper.isRTVoiceInScene;
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Prefabs/" + Util.Constants.GLOBALCACHE_SCENE_OBJECT_NAME, false, EditorHelper.MENU_ID + 40)]
      private static void AddGlobalCache()
      {
         EditorHelper.InstantiatePrefab(Util.Constants.GLOBALCACHE_SCENE_OBJECT_NAME);
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Prefabs/" + Util.Constants.GLOBALCACHE_SCENE_OBJECT_NAME, true)]
      private static bool AddGlobalCacheValidator()
      {
         return !EditorHelper.isGlobalCacheInScene;
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Prefabs/AudioFileGenerator", false, EditorHelper.MENU_ID + 60)]
      private static void AddAudioFileGenerator()
      {
         EditorHelper.InstantiatePrefab("AudioFileGenerator");
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Prefabs/Paralanguage", false, EditorHelper.MENU_ID + 70)]
      private static void AddParalanguage()
      {
         EditorHelper.InstantiatePrefab("Paralanguage");
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Prefabs/Sequencer", false, EditorHelper.MENU_ID + 80)]
      private static void AddSequencer()
      {
         EditorHelper.InstantiatePrefab("Sequencer");
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Prefabs/SpeechText", false, EditorHelper.MENU_ID + 90)]
      private static void AddSpeechText()
      {
         EditorHelper.InstantiatePrefab("SpeechText");
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Prefabs/TextFileSpeaker", false, EditorHelper.MENU_ID + 100)]
      private static void AddTextFileSpeaker()
      {
         EditorHelper.InstantiatePrefab("TextFileSpeaker");
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Prefabs/Loudspeaker", false, EditorHelper.MENU_ID + 120)]
      private static void AddLoudspeaker()
      {
         EditorHelper.InstantiatePrefab("Loudspeaker");
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Prefabs/VoiceInitializer", false, EditorHelper.MENU_ID + 140)]
      private static void AddVoiceInitializer()
      {
         EditorHelper.InstantiatePrefab("VoiceInitializer");
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Help/Manual", false, EditorHelper.MENU_ID + 600)]
      private static void ShowManual()
      {
         Util.Helper.OpenURL(Util.Constants.ASSET_MANUAL_URL);
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Help/API", false, EditorHelper.MENU_ID + 610)]
      private static void ShowAPI()
      {
         Util.Helper.OpenURL(Util.Constants.ASSET_API_URL);
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Help/Forum", false, EditorHelper.MENU_ID + 620)]
      private static void ShowForum()
      {
         Util.Helper.OpenURL(Util.Constants.ASSET_FORUM_URL);
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Help/Product", false, EditorHelper.MENU_ID + 630)]
      private static void ShowProduct()
      {
         Util.Helper.OpenURL(Util.Constants.ASSET_WEB_URL);
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Help/Videos/Promo", false, EditorHelper.MENU_ID + 650)]
      private static void ShowVideoPromo()
      {
         Util.Helper.OpenURL(Util.Constants.ASSET_VIDEO_PROMO);
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Help/Videos/Tutorial", false, EditorHelper.MENU_ID + 660)]
      private static void ShowVideoTutorial()
      {
         Util.Helper.OpenURL(Util.Constants.ASSET_VIDEO_TUTORIAL);
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Help/Videos/All Videos", false, EditorHelper.MENU_ID + 680)]
      private static void ShowAllVideos()
      {
         Util.Helper.OpenURL(Util.Constants.ASSET_SOCIAL_YOUTUBE);
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/Help/3rd Party Assets", false, EditorHelper.MENU_ID + 700)]
      private static void Show3rdPartyAV()
      {
         Util.Helper.OpenURL(Util.Constants.ASSET_3P_URL);
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/About/Unity AssetStore", false, EditorHelper.MENU_ID + 800)]
      private static void ShowUAS()
      {
         Util.Helper.OpenURL(EditorConstants.ASSET_URL);
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/About/" + Util.Constants.ASSET_AUTHOR, false, EditorHelper.MENU_ID + 820)]
      private static void ShowCT()
      {
         Util.Helper.OpenURL(Util.Constants.ASSET_AUTHOR_URL);
      }

      [MenuItem("Tools/" + Util.Constants.ASSET_NAME + "/About/Info", false, EditorHelper.MENU_ID + 840)]
      private static void ShowInfo()
      {
         EditorUtility.DisplayDialog(Util.Constants.ASSET_NAME + " - About",
            "Version: " + Util.Constants.ASSET_VERSION +
            System.Environment.NewLine +
            System.Environment.NewLine +
            "© 2015-2020 by " + Util.Constants.ASSET_AUTHOR +
            System.Environment.NewLine +
            System.Environment.NewLine +
            Util.Constants.ASSET_AUTHOR_URL +
            System.Environment.NewLine, "Ok");
      }
   }
}
#endif
// © 2015-2020 crosstales LLC (https://www.crosstales.com)