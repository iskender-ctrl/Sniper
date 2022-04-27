using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.IO;
using System;

public class Sanal : MonoBehaviour
{
    const string BundleFolder = "https://ndgstudio.com.tr/berkcan/StreamingAssets/";
    public List<string> LoadingObjectValue = new List<string>();
    public AssetBundle bundle;
    string bundleURL;
    public UnityWebRequest www;
    public List<Hash128> listOfCachedVersions = new List<Hash128>();
    bool isDownloaded;

    private void Start() {
        
    }
    public IEnumerator GetDisplayBundleRoutine(UnityAction<GameObject> callback,
Transform bundleParent)
    {
        for (int i = 0; i < listOfCachedVersions.Count; i++)
        {
            bundleURL = BundleFolder + LoadingObjectValue[i];

/*#if UNITY_ANDROID
            bundleURL += "Android";
#else
 bundleURL += "IOS";
#endif*/

            www = UnityWebRequestAssetBundle.GetAssetBundle(bundleURL);
            yield return www.SendWebRequest();

            bundle = DownloadHandlerAssetBundle.GetContent(www);
            Debug.Log("Requesting bundle at " + bundleURL);
            isDownloaded = true;
            //}
            Debug.Log("BundleUrl : " + bundleURL);

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {

                if (Caching.IsVersionCached(bundleURL,
            listOfCachedVersions[i]))
                {
                    print(i);
                    print("obje cachede var");
                    string rootAssetPath =
                    bundle.GetAllAssetNames()[0];
                    GameObject arObject =
                   Instantiate(bundle.LoadAsset(rootAssetPath) as GameObject,
                   bundleParent); 
                   callback(arObject);
                }
                // Eğer bu version numarası cache de yoksa buraya giriyor
                else if (!Caching.IsVersionCached(bundleURL,
               listOfCachedVersions[i]))
                {
                    // bundle unload yapıyoruz.
                    if (bundle != null)
                    {
                        bundle.Unload(false);
                    }
                    print("Obje cachede yok indir");
                    while (!Caching.ready)
                        yield return null;

                    using (var wwww =
WWW.LoadFromCacheOrDownload(bundleURL, listOfCachedVersions[i]))
                    {
                        yield return wwww;
                        if (!string.IsNullOrEmpty(wwww.error))
                        {
                            Debug.Log(wwww.error);
                            yield return null;
                        }
                        bundle = wwww.assetBundle;
                        string asset = bundle.GetAllAssetNames()[0];
                        GameObject game =
                        Instantiate(bundle.LoadAsset(asset) as GameObject, new Vector3(0, 0,
                        0), Quaternion.identity);
                        //bundle.Unload(false);
                        callback(game);
                    }
                }

            }

        }
    }
}
