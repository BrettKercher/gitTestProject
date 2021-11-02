using System.Reflection;
using Ludiq;
using UnityEngine;

public class PreBuildScript : MonoBehaviour {
    public static void PreExport() { 
        var init = typeof(PluginContainer).GetMethod("Initialize", BindingFlags.NonPublic | BindingFlags.Static);
        init?.Invoke(null,null); // Force PluginContainer to initialize, since it won't in this context due to a Bolt bug
        AotPreBuilder.PreCloudBuild(); 
    }
}
