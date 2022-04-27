using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class CreateAssetBundle
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        /*AssetBundle'ın adını Unity içinde Assets'in altında
        StreamingAssets Adında bir dosya oluşturacak ve burda prefablara
       verdiğiniz assetBundle isimlerini görebileceksiniz.*/
        string assetBundleDirectory = "Assets/StreamingAssets";
        // eğer dizin yoksa dizini oluşturacaktır.
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        /*burda bir hat oluşturuyor buildassetsbundlestan ve oluşabilecek bütün
        hedef olan platformlarda çekebileceksiniz. Yani android, ios vb gibi*/
        BuildPipeline.
        BuildAssetBundles(assetBundleDirectory,
BuildAssetBundleOptions.None,
EditorUserBuildSettings.activeBuildTarget);
    }
}