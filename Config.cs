using BepInEx;
using BepInEx.Configuration;


namespace BlissScrap
{
    public class Config
    {
        public static ConfigEntry<int> vai5000SpawnWeight;
        public static ConfigEntry<int> eggSpraySpawnWeight;
        public static ConfigEntry<int> blissySpawnWeight;
        public static ConfigEntry<int> alemSpawnWeight;
        public static ConfigEntry<int> gbaSpawnWeight;

        public static ConfigFile VolumeConfig;

        public static void Load()
        {
            vai5000SpawnWeight = Plugin.config.Bind<int>("Scrap", "vai5000", 10, "How much does vai5000 spawn, higher = more common");
            eggSpraySpawnWeight = Plugin.config.Bind<int>("Scrap", "eggspray", 30, "How much does egg spray spawn, higher = more common");
            blissySpawnWeight = Plugin.config.Bind<int>("Scrap", "blissy", 10, "How much does blissy spawn higher = more common");
            alemSpawnWeight = Plugin.config.Bind<int>("Scrap", "alem", 10, "How much does alem spawn higher = more common");
            gbaSpawnWeight = Plugin.config.Bind<int>("Scrap", "gba", 10, "How much does GBA spawn higher = more common");

            VolumeConfig = new ConfigFile(Paths.ConfigPath + "\\BlissCorp.AudioVolume.cfg", true);
        }
    }
}
