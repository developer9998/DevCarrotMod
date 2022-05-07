using BepInEx;
using BepInEx.Configuration;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using UnityEngine;
using Utilla;

namespace DevCarrotMod
{
    [Description("HauntedModMenu")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [ModdedGamemode]
    public class Plugin : BaseUnityPlugin
    {
        /*Mod under the MIT license, if you reproduce please credit*/

        /*Assetloading*/
        public static readonly string assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        static GameObject Carrot; // the carrot asset
        static GameObject LeftHand; // the player's left hand
        static GameObject RightHand; // the player's right hand

        /*Config Variables*/
        public static ConfigEntry<bool> isInRightHand;

        void OnEnable()
        {
            SetConfigStuff();

            Utilla.Events.GameInitialized += OnGameInitialized;
            Carrot.gameObject.SetActive(this.enabled);
        }

        void OnDisable()
        {
            Utilla.Events.GameInitialized -= OnGameInitialized;
            Carrot.gameObject.SetActive(this.enabled);
        }

        static void SetConfigStuff()
        {
            var customFile = new ConfigFile(Path.Combine(Paths.ConfigPath, "DevCarrotModConfig.cfg"), true);
            isInRightHand = customFile.Bind("Configuration", "InRightHand", true, "Is the carrot in the player's right hand");
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("DevCarrotMod.Resources.carrot");
            AssetBundle bundle = AssetBundle.LoadFromStream(str);
            GameObject carrotGameObject = bundle.LoadAsset<GameObject>("pig_carrot");
            Carrot = Instantiate(carrotGameObject);
            LeftHand = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L/palm.01.L");
            RightHand = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R");

            if (isInRightHand.Value)
            {
                Carrot.transform.SetParent(RightHand.transform, false);
                Carrot.transform.localPosition = new Vector3(-0.015f, 0.008f, -0.2225f);
                Carrot.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
                Carrot.transform.localScale = new Vector3(0.0614f, 0.0614f, 0.0614f);
            }
            else
            if (!isInRightHand.Value)
            {
                Carrot.transform.SetParent(LeftHand.transform, false);
                Carrot.transform.localPosition = new Vector3(0.2272596f, 0.01365192f, 0.02138449f);
                Carrot.transform.localRotation = Quaternion.Euler(20.42f, 3.325f, 0.042f);
                Carrot.transform.localScale = new Vector3(0.0614f, 0.0614f, 0.0614f);
            }

            Carrot.gameObject.SetActive(this.enabled);
        }
    }
}
