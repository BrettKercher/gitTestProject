using System;

using System.Threading.Tasks;

using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

using UnityEditor;
using UnityEngine;

using System.Collections.Generic;


[InitializeOnLoad]
public static class BuildScripts
{
    // States
    private enum OperationState
    {
        Idle,
        ListingPackages,
        PackageOperations,
        CustomOperation_1,
        CustomOperation_2,
        Complete
    }

    // Persistent state across domain reloads
    private static OperationState CurrentState
    {
        get => (OperationState)SessionState.GetInt("BuildScripts_currentState", (int)OperationState.Idle);
        set => SessionState.SetInt("BuildScripts_currentState", (int)value);
    }

    private static List<string> packagesToAdd = new List<string>();
    private static List<string> packagesToRemove = new List<string>();
    private static Request currentRequest;
    private static List<string> currentPackageList = new List<string>();

    // Hooking into the editor update loop
    static BuildScripts()
    {
        Debug.Log($"BUILD: BuildScripts: Initialized. Current state: {CurrentState}" );
        EditorApplication.update += StateMachineUpdate;
    }

    private static void StateMachineUpdate()
    {
        switch (CurrentState)
        {
            case OperationState.Idle:
                break;

            case OperationState.ListingPackages:
                ProcessListOperation();
                break;

            case OperationState.PackageOperations:
                ProcessPackageOperations();
                break;

            case OperationState.CustomOperation_1:
                ProcessCustomOperation_1();
                break;

            case OperationState.CustomOperation_2:
                ProcessCustomOperation_2();
                break;

            case OperationState.Complete:
                FinishOperations();
                break;
        }
    }

    private static void CheckRequestStatus(Request request, string operationType)
    {
        if (request.Status == StatusCode.Success)
        {
            Debug.Log($"BUILD: {operationType} operation completed successfully");
        }
        else if (request.Status >= StatusCode.Failure)
        {
            Debug.LogError($"BUILD: {operationType} operation failed: {request.Error.message}");
        }
    }

    private static void FinishOperations()
    {
        Debug.Log("BUILD: Finished all operations");
        // EditorApplication.Exit(0);
    }

    private static void ProcessListOperation()
    {
        if (currentRequest == null)
        {
            Debug.Log("BUILD: Requesting package list...");
            currentRequest = Client.List(true, true);
            return;
        }

        if (currentRequest.IsCompleted)
        {
            if (currentRequest.Status == StatusCode.Success)
            {
                currentPackageList.Clear();
                foreach (var package in ((ListRequest)currentRequest).Result)
                {
                    currentPackageList.Add(package.name);
                    Debug.Log($"BUILD: Found package: {package.name} @ {package.version}");
                }
                Debug.Log($"BUILD: Total packages installed: {currentPackageList.Count}");
            }
            else if (currentRequest.Status >= StatusCode.Failure)
            {
                Debug.LogError($"BUILD: Failed to list packages: {currentRequest.Error.message}");
            }

            // Decide and enqueue Ops, e.g.
            packagesToAdd.Add("com.unity.2d.animation");
            packagesToAdd.Add("com.unity.2d.tilemap.extras");
            packagesToRemove.Add("com.unity.2d.pixel-perfect");

            currentRequest = null;
            CurrentState = OperationState.PackageOperations;
        }
    }

    private static void ProcessPackageOperations()
    {
        if (currentRequest != null)
        {
            if (currentRequest.IsCompleted)
            {
                CheckRequestStatus(currentRequest, "AddAndRemove");
                currentRequest = null;
                packagesToAdd.Clear();
                packagesToRemove.Clear();
                Debug.Log("BUILD: Package operations completed");

                CurrentState = OperationState.CustomOperation_1;
            }
            return;
        }

        if ((packagesToAdd.Count+packagesToRemove.Count) > 0)
        {
            Debug.Log($"BUILD: Adding {packagesToAdd.Count} packages, Removing {packagesToRemove.Count} packages...");
            currentRequest = Client.AddAndRemove(packagesToAdd.ToArray(), packagesToRemove.ToArray());
        }
        else
        {
            CurrentState = OperationState.CustomOperation_1;
        }
    }

    private static void ProcessCustomOperation_1()
    {
        Debug.Log("BUILD: Custom operation 1");

        var task = Task.Run(() => RunAsyncBuildMethod());

        // Wait for it manually — but with timeout or check for hangs
        if (!task.Wait(TimeSpan.FromMinutes(10)))
        {
            Debug.Log("BUILD: Custom operation 1 timed out");
            // EditorApplication.Exit(1);
            return;
        }

        Debug.Log("BUILD: Custom operation 1 completed");

        CurrentState = OperationState.CustomOperation_2;
    }

    private static void ProcessCustomOperation_2()
    {
        Debug.Log("BUILD: Custom operation 2");

        CurrentState = OperationState.Complete;
    }

    // [MenuItem("BUILD/Run Build")]
    // public static void MenuBuildMethod()
    // {
    //     Debug.Log("BUILD: method started. (main)");
    //
    //     BuildMethod();
    //
    //     Debug.Log("BUILD: async task started, returning to editor. (main)");
    // }

    public static void PreExport()
    {
        BuildMethod();
    }

    private static async Task RunAsyncBuildMethod()
    {
        Debug.Log("BUILD: Custom op - Task Started. (task)");

        await Task.Delay(1000); // or any other async operation

        Debug.Log("BUILD: Custom op - Task Finished. (task)");
    }

    public static void BuildMethod()
    {
        Console.WriteLine("BUILD: method started. (main)");

        // Go! Listing packages sets the packagesToAdd and packagesToRemove lists
        CurrentState = OperationState.ListingPackages;

        Debug.Log("BUILD: State machine running. (main)");
    }

}