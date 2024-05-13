using System.IO;

using UnityEditor;
using UnityEngine;

using Codice.Client.BaseCommands;
using Codice.Client.Common;
using Codice.CM.Common;
using PlasticGui;

[InitializeOnLoad]
public static class Test
{
    static Test()
    {
        ClientHandlers.Register();

        var wkInfo = FindWorkspace.InfoForApplicationPath(
            Application.dataPath, mPlasticApi);

        var selector = mPlasticApi.GetSelectorUserInformation(wkInfo);

        Debug.Log("Workspace selector: " + 
                  selector.GetPrefixedObjectSpec() + "...@" +
                  selector.RepSpec);
    }

    static class FindWorkspace
    {
        internal static bool HasWorkspace(string path)
        {
            string wkPath = PathForApplicationPath(path);

            return !string.IsNullOrEmpty(wkPath);
        }

        internal static string PathForApplicationPath(string path)
        {
            try
            {
                return FindWorkspacePath(path, ClientConfig.Get().GetWkConfigDir());
            }
            catch (NotConfiguredClientException)
            {
                return null;
            }
        }

        internal static WorkspaceInfo InfoForApplicationPath(string path, IPlasticAPI plasticApi)
        {
            string wkPath = PathForApplicationPath(path);

            if (string.IsNullOrEmpty(wkPath))
                return null;

            return plasticApi.GetWorkspaceFromPath(wkPath);
        }

        static string FindWorkspacePath(string path, string wkConfigDir)
        {
            while (!string.IsNullOrEmpty(path))
            {
                if (Directory.Exists(Path.Combine(path, wkConfigDir)))
                    return path;

                path = Path.GetDirectoryName(path);
            }

            return null;
        }
    }

    static IPlasticAPI mPlasticApi = new PlasticAPI();
}