using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;

namespace UniP2P
{
    public class PeerInfoWindow : EditorWindow
    {
        static PeerInfoWindow window;

        [MenuItem("UniP2P/Debugger/PeerInfo")]
        public static void ShowWindow()
        {
            if (window != null)
            {
                window.Close();
            }

            GetWindow<PeerInfoWindow>("PeerInfo").Show();
        }

        private PeerInfoTreeView peerInfoTreeView;

        object splitterState;
        void OnEnable()
        {
            window = this;
            splitterState = SplitterGUILayout.CreateSplitterState(new float[] { 75f, 25f }, new int[] { 32, 32 }, null);
            peerInfoTreeView = new PeerInfoTreeView();
        }

        private static bool EnableAutoReload = true;
        private int c_count;

        void OnGUI()
        {
            RenderHeadPanel();
            
            if (EnableAutoReload && UniP2PManager.GetPeerCount() != c_count)
            {
                PeerReload();
                c_count = UniP2PManager.GetPeerCount();
            }

            SplitterGUILayout.BeginVerticalSplit(this.splitterState, EmptyLayoutOption);
            {
                RenderTable();
            }
            SplitterGUILayout.EndVerticalSplit();
        }



        static readonly GUIContent EnableAutoReloadHeadContent = EditorGUIUtility.TrTextContent("Enable AutoReload", "Reload automatically.", (Texture)null);
        static readonly GUIContent ReloadHeadContent = EditorGUIUtility.TrTextContent("Reload", "Reload View.", (Texture)null);
        static readonly GUILayoutOption[] EmptyLayoutOption = new GUILayoutOption[0];

        void RenderHeadPanel()
        {
            EditorGUILayout.BeginVertical(EmptyLayoutOption);
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, EmptyLayoutOption);

            if (GUILayout.Toggle(EnableAutoReload, EnableAutoReloadHeadContent, EditorStyles.toolbarButton, EmptyLayoutOption) != EnableAutoReload)
            {
                EnableAutoReload = !EnableAutoReload;
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button(ReloadHeadContent, EditorStyles.toolbarButton, EmptyLayoutOption))
            {
                PeerReload();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        Vector2 tableScroll;
        GUIStyle tableListStyle;
        void RenderTable()
        {
            if (tableListStyle == null)
            {
                tableListStyle = new GUIStyle("CN Box");
                tableListStyle.margin.top = 0;
                tableListStyle.padding.left = 3;
            }

            EditorGUILayout.BeginVertical(tableListStyle, EmptyLayoutOption);

            this.tableScroll = EditorGUILayout.BeginScrollView(this.tableScroll, new GUILayoutOption[]
            {
                GUILayout.ExpandWidth(true),
                GUILayout.MaxWidth(2000f)
            });
            var controlRect = EditorGUILayout.GetControlRect(new GUILayoutOption[]
            {
                GUILayout.ExpandHeight(true),
                GUILayout.ExpandWidth(true)
            });

            peerInfoTreeView?.OnGUI(controlRect);

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        void PeerReload()
        {
            if (UniP2PManager.GetPeerCount() != 0)
            {
                peerInfoTreeView.Reload();
            }
            Repaint();
        }
    }

    // reflection call of UnityEditor.SplitterGUILayout
    internal static class SplitterGUILayout
    {
        static BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        static Lazy<Type> splitterStateType = new Lazy<Type>(() =>
        {
            var type = typeof(EditorWindow).Assembly.GetTypes().First(x => x.FullName == "UnityEditor.SplitterState");
            return type;
        });

        static Lazy<ConstructorInfo> splitterStateCtor = new Lazy<ConstructorInfo>(() =>
        {
            var type = splitterStateType.Value;
            return type.GetConstructor(flags, null, new Type[] { typeof(float[]), typeof(int[]), typeof(int[]) }, null);
        });

        static Lazy<Type> splitterGUILayoutType = new Lazy<Type>(() =>
        {
            var type = typeof(EditorWindow).Assembly.GetTypes().First(x => x.FullName == "UnityEditor.SplitterGUILayout");
            return type;
        });

        static Lazy<MethodInfo> beginVerticalSplit = new Lazy<MethodInfo>(() =>
        {
            var type = splitterGUILayoutType.Value;
            return type.GetMethod("BeginVerticalSplit", flags, null, new Type[] { splitterStateType.Value, typeof(GUILayoutOption[]) }, null);
        });

        static Lazy<MethodInfo> endVerticalSplit = new Lazy<MethodInfo>(() =>
        {
            var type = splitterGUILayoutType.Value;
            return type.GetMethod("EndVerticalSplit", flags, null, Type.EmptyTypes, null);
        });

        public static object CreateSplitterState(float[] relativeSizes, int[] minSizes, int[] maxSizes)
        {
            return splitterStateCtor.Value.Invoke(new object[] { relativeSizes, minSizes, maxSizes });
        }

        public static void BeginVerticalSplit(object splitterState, params GUILayoutOption[] options)
        {
            beginVerticalSplit.Value.Invoke(null, new object[] { splitterState, options });
        }

        public static void EndVerticalSplit()
        {
            endVerticalSplit.Value.Invoke(null, Type.EmptyTypes);
        }
    }

}