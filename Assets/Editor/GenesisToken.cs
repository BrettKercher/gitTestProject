using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public class GenesisToken : MonoBehaviour
{
        [MenuItem("DevTools/Show Access Token", false, 21)]
        internal static void Print()
        {
            Debug.Log(CloudProjectSettings.accessToken);
        }
        
        [MenuItem("DevTools/TestStuff")]
        internal static void TestDLL()
        {
            NamedBuildTarget namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            Debug.Log(PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget));
        }
}
