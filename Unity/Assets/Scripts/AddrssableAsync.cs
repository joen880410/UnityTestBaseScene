using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;


public class AddrssableAsync : MonoBehaviour
{
    public static AddrssableAsync instance;
    //addressable label的Key
    public const string AssetBundleDependicKey = "AllAsssetBundle";

    public AddrssableAsync()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void Awake()
    {
        Addressables.InitializeAsync().Completed += async (Result) =>
        {
            await InitCompletedCallback();
        };
    }
    public async Task InitCompletedCallback()
    {
        var CheckResult = Addressables.CheckForCatalogUpdates();
        await CheckCataLogsCompleted(CheckResult);
    }
    private async Task CheckCataLogsCompleted(AsyncOperationHandle<List<string>> checkCataLogsResult)
    {
        if (checkCataLogsResult.Result.Count > 0)
        {
            await UpdateCatalogs();
        }
    }



    public Task UpdateCatalogs()
    {
        return Addressables.UpdateCatalogs().Task;
    }

    #region Load
    public async Task<AsyncOperationHandle<Object>> LoadAsync(string assetName)
    {
        await Task.CompletedTask;
        try
        {
            return Addressables.LoadAssetAsync<Object>(assetName);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            return new AsyncOperationHandle<Object>();
        }

    }
    public async Task<AsyncOperationHandle<GameObject>> LoadInstantiate(string assetName, Transform parent = null, bool instantiateInWorldSpace = false, bool trackHandle = true)
    {
        await Task.CompletedTask;
        try
        {
            return Addressables.InstantiateAsync(assetName, parent, instantiateInWorldSpace, trackHandle);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            return new AsyncOperationHandle<GameObject>();
        }


    }
    public async Task<AsyncOperationHandle<SceneInstance>> LoadSceneAsync(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        await Task.CompletedTask;
        try
        {
            return Addressables.LoadSceneAsync(sceneName, loadMode);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            return new AsyncOperationHandle<SceneInstance>();
        }


    }
    #endregion

    #region Unload
    public void Unload(Object asset)
    {
        Addressables.Release(asset);
    }
    public void Unload(AsyncOperationHandle<Object> async)
    {
        if (async.IsValid())
        {
            Addressables.Release(async);
        }
    }
    public async void UnloadScene(AsyncOperationHandle<SceneInstance> async)
    {
        try
        {
            await Addressables.UnloadSceneAsync(async.Result).Task;
            return;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }
    #endregion
}
