using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using UnityEngine;
using System.Reflection;
using System.IO;
using BlissScrap.MonoBehaviours;

namespace BlissScrap
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    [BepInDependency(LethalLib.Plugin.ModGUID)]
    [BepInDependency("evaisa.lethalthings", BepInDependency.DependencyFlags.SoftDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public const string ModGUID = "faejr.blissscrap";
        public const string ModName = "BlissScrap";
        public const string ModVersion = "0.5.0";

        public static ManualLogSource logger;
        public static ConfigFile config;

        public static AssetBundle BlissScrapAssets;

        private void Awake()
        {
            logger = Logger;
            config = Config;

            // Plugin startup logic

            BlissScrap.Config.Load();

            BlissScrapAssets = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "blissscrap"));

            var vai5000 = SetupAsset("assets/custom/scrap/vai5000/vai5000.asset", BlissScrap.Config.vai5000SpawnWeight.Value);
            if (vai5000) {
                logger.LogInfo("Vai5000 has infiltrated your facilities...");
            }

            SetupAsset("assets/custom/scrap/eggspray/eggspray.asset", BlissScrap.Config.eggSpraySpawnWeight.Value);
            SetupAsset("assets/custom/scrap/blissy/blissy.asset", BlissScrap.Config.blissySpawnWeight.Value);
            SetupAsset("assets/custom/scrap/alem/alem.asset", BlissScrap.Config.alemSpawnWeight.Value);
            SetupAsset("assets/custom/scrap/gba/gba.asset", BlissScrap.Config.gbaSpawnWeight.Value);
            SetupAsset("assets/custom/scrap/swallowbug/swallowbug.asset", BlissScrap.Config.swallowbugSpawnWeight.Value);
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("evaisa.lethalthings")) {
                SetupAsset("assets/custom/scrap/dani ball/TennisBall.asset", BlissScrap.Config.swallowbugSpawnWeight.Value);
            }

            Logger.LogInfo($"Plugin {ModName} is loaded!");
        }

        private Item SetupAsset(string assetName, int spawnWeight)
        {
            logger.LogInfo($"Loading {assetName}");
            Item item = BlissScrapAssets.LoadAsset<Item>(assetName);
            if (item == null)
            {
                logger.LogError($"Failed to load {assetName}");
                return null;
            }
            else
            {
                LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(item.spawnPrefab);
                LethalLib.Modules.Items.RegisterScrap(item, spawnWeight, LethalLib.Modules.Levels.LevelTypes.All);
            }

            // get all AudioSources
            var audioSources = item.spawnPrefab.GetComponentsInChildren<AudioSource>();
            var prefabName = item.spawnPrefab.name;

            // if has any AudioSources
            if (audioSources.Length > 0)
            {
                var configValue = BlissScrap.Config.VolumeConfig.Bind<float>("Volume", $"{prefabName}", 100f, $"Audio volume for {prefabName} (0 - 100)");

                // loop through AudioSources, adjust volume by multiplier
                foreach (var audioSource in audioSources)
                {
                    audioSource.volume *= (configValue.Value / 100);
                }
            }

            return item;
        }
    }
}
