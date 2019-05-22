using System;
using System.Collections.Generic;
using System.Reflection;
using UniP2P.HLAPI;
using UnityEditor;
using UnityEngine;

namespace UniP2P
{
    public class EditorP2PNetworkSettings : Editor
    {
        public static MatchingSettings NetworkSetting = AssetDatabase.LoadAssetAtPath<MatchingSettings>(UniP2PManager.DefaultMatchingSettingsPath);

        [MenuItem("UniP2P/Settings/Open NetworkSettings", false, 0)]
        static void Open()
        {
            Load();
            if (NetworkSetting == null)
            {
                Create();
                Debug.Debugger.Log("Not Found NetworkSetting. Create NetworkSetting.");
            }
            Selection.activeInstanceID = NetworkSetting.GetInstanceID();
            EditorGUIUtility.PingObject(Selection.activeObject);
        }

        [MenuItem("UniP2P/Settings/Create NetworkSettings", false, 2)]
        static void Create()
        {
            Load();
            if (NetworkSetting != null)
            {
                Open();
                Debug.Debugger.Log("Already exist NetworkSetting.");
            }
            else
            {
                NetworkSetting = CreateInstance<MatchingSettings>();
                AssetDatabase.CreateAsset(NetworkSetting, UniP2PManager.DefaultMatchingSettingsPath);
                AssetDatabase.Refresh();
            }
        }
        
        static void Load()
        {
            NetworkSetting = AssetDatabase.LoadAssetAtPath<MatchingSettings>(UniP2PManager.DefaultMatchingSettingsPath);
        }

    }

}