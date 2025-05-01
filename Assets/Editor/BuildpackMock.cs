using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class BuildpackMock
{
    const string k_PreExportMethod = "BuildScripts.PreExport";

    [MenuItem("UBA/Run Pre-Export")]
    private static void RunUserScript()
    {
        if (!TryRunUserExportMethod(k_PreExportMethod))
        {
            Debug.Log("ERROR: preExportMethod '{0}' failed, aborting.");
        }

        Debug.Log("Finished running Pre-Export");
    }

    private static bool TryRunUserExportMethod(string fullName)
    {
        MethodInfo methodInfo;
        object[] parameters = null;

        if (TryLoadStaticEditorMethodInfo(fullName, true, out methodInfo))
        {
            parameters = new object[] { };
        }
        else
        {
            // Could not find the specified method
            Debug.Log("ERROR: export method '{0}' not found, aborting. Please make sure you are using the correct function signature.");
            return false;
        }

        if (!CallStaticEditorMethod(methodInfo, true, parameters))
        {
            return false;
        }

        return true;
    }

    public static bool TryLoadStaticEditorMethodInfo(string fullName, bool logErrors, out MethodInfo method, params Type[] parameterTypes)
    {
        var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            if (!TryLoadMethodInfo(assembly, fullName, bindingFlags, out method, parameterTypes))
            {
                continue;
            }

            return true;
        }

        method = null;
        return false;
    }

    private static bool TryLoadMethodInfo(Assembly assembly, string fullName, BindingFlags flags, out MethodInfo methodInfo, params Type[] types)
    {
        if (assembly == null)
        {
            methodInfo = null;
            return false;
        }

        int index = fullName.LastIndexOf(".");
        string className = fullName.Substring(0, index);
        string methodName = fullName.Substring(index + 1);
        Type type = assembly.GetType(className);
        if (type == null)
        {
            methodInfo = null;
            return false;
        }

        methodInfo = type.GetMethod(methodName, flags, null, types, null);

        return methodInfo != null;
    }

    public static bool CallStaticEditorMethod(MethodInfo methodInfo, bool logErrors, params object[] parameters)
    {
        try
        {
            methodInfo.Invoke(null, parameters);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        return false;
    }
}
