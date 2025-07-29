using ExitGames.Client.Photon;
using GorillaNetworking;
using iiMenu.Classes;
using iiMenu.Menu;
using iiMenu.Notifications;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using static iiMenu.Classes.RigManager;
using static iiMenu.Menu.Main;

namespace iiMenu.Mods
{
    public class Experimental
    {
        public static void FixDuplicateButtons()
        {
            int duplicateButtons = 0;
            List<string> previousNames = new List<string> { };
            foreach (ButtonInfo[] buttonn in Buttons.buttons)
            {
                foreach (ButtonInfo button in buttonn)
                {
                    if (previousNames.Contains(button.buttonText))
                    {
                        string buttonText = button.overlapText ?? button.buttonText;
                        button.overlapText = buttonText;
                        button.buttonText += "X";
                        duplicateButtons++;
                    }
                    previousNames.Add(button.buttonText);
                }
            }
            NotifiLib.SendNotification("<color=grey>[</color><color=green>SUCCESS</color><color=grey>]</color> Successfully fixed " + duplicateButtons.ToString() + " broken buttons.");
        }

        private static Dictionary<Renderer, Material> oldMats = new Dictionary<Renderer, Material> { };
        public static void BetterFPSBoost()
        {
            foreach (Renderer v in Resources.FindObjectsOfTypeAll<Renderer>())
            {
                try
                {
                    if (v.material.shader.name == "GorillaTag/UberShader")
                    {
                        oldMats.Add(v, v.material);
                        Material replacement = new Material(Shader.Find("GorillaTag/UberShader"))
                        {
                            color = v.material.color
                        };
                        v.material = replacement;
                    }
                } catch (Exception exception) { LogManager.LogError(string.Format("mat error {1} - {0}", exception.Message, exception.StackTrace)); }
            }
        }
        public static void DisableBetterFPSBoost()
        {
            foreach (KeyValuePair<Renderer, Material> v in oldMats)
                v.Key.material = v.Value;
        }

        public static void DumpSoundData()
        {
            string text = "Handtap Sound Data\n(from GorillaLocomotion.GTPlayer.Instance.materialData)";
            int i = 0;
            foreach (GorillaLocomotion.GTPlayer.MaterialData oneshot in GorillaLocomotion.GTPlayer.Instance.materialData)
            {
                try
                {
                    text += "\n====================================\n";
                    text += i.ToString() + " ; " + oneshot.matName + " ; " + oneshot.slidePercent.ToString() + "% ; " + (oneshot.audio == null ? "none" : oneshot.audio.name);
                }
                catch { LogManager.Log("Failed to log sound"); }
                i++;
            }
            text += "\n====================================\n";
            text += "Text file generated with ii's Stupid Menu";
            string fileName = $"{PluginInfo.BaseDirectory}/SoundData.txt";

            File.WriteAllText(fileName, text);

            string filePath = Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().Location, fileName);
            filePath = filePath.Split("BepInEx\\")[0] + fileName;

            Process.Start(filePath);
        }

        public static void DumpCosmeticData()
        {
            string text = "Cosmetic Data\n(from GorillaNetworking.CosmeticsController.allCosmeticsDict)";
            int i = 0;
            foreach (CosmeticsController.CosmeticItem hat in CosmeticsController.instance.allCosmetics)
            {
                try
                {
                    text += "\n====================================\n";
                    text += hat.itemName + " ; " + hat.displayName + " (override " + hat.overrideDisplayName + ") ; " + hat.cost.ToString() + "SR ; canTryOn = " + hat.canTryOn.ToString();
                }
                catch { LogManager.Log("Failed to log hat"); }
                i++;
            }
            text += "\n====================================\n";
            text += "Text file generated with ii's Stupid Menu";
            string fileName = $"{PluginInfo.BaseDirectory}/CosmeticData.txt";

            File.WriteAllText(fileName, text);

            string filePath = Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().Location, fileName);
            filePath = filePath.Split("BepInEx\\")[0] + fileName;

            Process.Start(filePath);
        }

        public static void DecryptableCosmeticData()
        {
            string text = "";
            int i = 0;
            foreach (CosmeticsController.CosmeticItem hat in CosmeticsController.instance.allCosmetics)
            {
                try
                {
                    text += hat.itemName + ";;" + hat.overrideDisplayName + ";;" + hat.cost.ToString() + "\n";
                }
                catch { LogManager.Log("Failed to log hat"); }
                i++;
            }
            string fileName = $"{PluginInfo.BaseDirectory}/DecryptableCosmeticData.txt";

            File.WriteAllText(fileName, text);

            string filePath = Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().Location, fileName);
            filePath = filePath.Split("BepInEx\\")[0] + fileName;

            Process.Start(filePath);
        }

        public static void DumpRPCData()
        {
            string text = "RPC Data\n(from PhotonNetwork.PhotonServerSettings.RpcList)";
            int i = 0;
            foreach (string name in PhotonNetwork.PhotonServerSettings.RpcList)
            {
                try
                {
                    text += "\n====================================\n";
                    text += i.ToString() + " ; " + name;
                }
                catch { LogManager.Log("Failed to log RPC"); }
                i++;
            }
            text += "\n====================================\n";
            text += "Text file generated with ii's Stupid Menu";
            string fileName = $"{PluginInfo.BaseDirectory}/RPCData.txt";

            File.WriteAllText(fileName, text);

            string filePath = Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().Location, fileName);
            filePath = filePath.Split("BepInEx\\")[0] + fileName;
            
            Process.Start(filePath);
        }

        public static void CopyCustomGamemodeScript() =>
            GUIUtility.systemCopyBuffer = CustomGameMode.LuaScript;
    }
}
