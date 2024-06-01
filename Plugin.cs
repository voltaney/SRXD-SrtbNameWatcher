using BepInEx;
using HarmonyLib;
using System.IO;

namespace SrtbNameWatcher
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Harmony.CreateAndPatchAll(typeof(Patch), PluginInfo.PLUGIN_GUID);
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded");
        }
    }
    public class Patch
    {
        [HarmonyPatch(typeof(XDSelectionListItemDisplay_Track), "PlayOnCurrentDifficulty")]
        [HarmonyPrefix]
        private static void Play_Prefix(XDSelectionListItemDisplay_Track __instance)
        {
            ITrackItem trackItem = __instance.Item.item as ITrackItem;
            TrackInfoAssetReference trackInfoRef = trackItem.Metadata.TrackInfoRef;
            IMultiAssetSaveFile multiAssetSaveFile = (trackInfoRef != null) ? trackInfoRef.customFile : null;

            if (multiAssetSaveFile != null)
            {

                using (var writer = new StreamWriter("CurrentSrtbName.info", false))
                {
                    writer.WriteLine($"{multiAssetSaveFile.FileNameNoExtension}");
                }
            }
        }
    }
}
