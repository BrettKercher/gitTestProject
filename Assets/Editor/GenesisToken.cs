using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GenesisToken : MonoBehaviour
{
        [MenuItem("DevTools/Show Access Token", false, 21)]
        internal static void Print()
        {
            UnityEngine.Debug.Log(CloudProjectSettings.accessToken);
        }
}
