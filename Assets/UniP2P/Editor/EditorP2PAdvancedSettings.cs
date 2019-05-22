using System;
using System.Collections.Generic;
using System.Reflection;
using UniP2P.HLAPI;
using UnityEditor;
using UnityEngine;

namespace UniP2P
{
    public class EditorP2PAdvancedSettings : Editor
    {
        public static AdvancedSettings AdvancedSettings = AssetDatabase.LoadAssetAtPath<AdvancedSettings>(UniP2PManager.DefaultAdvancedSettingsPath);

        [MenuItem("UniP2P/Settings/Open AdvancedSettings",false,1)]
        static void Open()
        {
            Load();
            if (AdvancedSettings == null)
            {
                Create();
                Debug.Debugger.Log("Not Found NetworkSetting. Create NetworkSetting.");
            }
            Selection.activeInstanceID = AdvancedSettings.GetInstanceID();
            EditorGUIUtility.PingObject(Selection.activeObject);
        }

        [MenuItem("UniP2P/Settings/Create AdvancedSettings", false, 3)]
        static void Create()
        {
            Load();
            if (AdvancedSettings != null)
            {
                Open();
                Debug.Debugger.Log("Already exist NetworkSetting.");
            }
            else
            {
                AdvancedSettings = CreateInstance<AdvancedSettings>();
                AssetDatabase.CreateAsset(AdvancedSettings, UniP2PManager.DefaultAdvancedSettingsPath);
                AssetDatabase.Refresh();
            }
        }

        static void Load()
        {
            AdvancedSettings = AssetDatabase.LoadAssetAtPath<AdvancedSettings>(UniP2PManager.DefaultAdvancedSettingsPath);
        }

    }

}