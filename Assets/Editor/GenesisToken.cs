using System;
using UnityEditor;
using UnityEngine;

public class GenesisToken : MonoBehaviour
{
        [MenuItem("DevTools/Show Access Token", false, 21)]
        internal static void Print()
        {
            Debug.Log(CloudProjectSettings.accessToken);
        }
        
        [MenuItem("DevTools/Buildpack-DLL Test")]
        internal static void TestDLL()
        {
            Debug.Log("Testing DLL");
            try
            {
                UnityEditor.CloudBuild.Builder.Test();
            }
            catch (Exception e)
            {
                Debug.Log($"Error Running Buildpack: {e.Message}");
            }
            
        }
}
