using UnityEngine;
using System.Collections;

public class AdBannerObserver : MonoBehaviour {
    private static AdBannerObserver sInstance;
    
    public static void Initialize() {
        Initialize(null, 0, 0.0f);
    }
    
    public static void Initialize(string mediaId, int spotId, float refresh) {
        if (sInstance == null) {
            // Make a game object for observing.
            GameObject go = new GameObject("_AdBannerObserver");
            go.hideFlags = HideFlags.HideAndDontSave;
            DontDestroyOnLoad(go);
            // Add and initialize this component.
            sInstance = go.AddComponent<AdBannerObserver>();
            sInstance.mAdStirMediaId = mediaId;
            sInstance.mAdStirSpotId = spotId;
            sInstance.mRefreshTime = refresh;
        }
    }
    
    public string mAdStirMediaId;
    public int mAdStirSpotId;
    public float mRefreshTime;
    
    IEnumerator Start () {
#if UNITY_IPHONE
        ADBannerView banner = new ADBannerView();
        banner.autoSize = true;
        banner.autoPosition = ADPosition.Bottom;
        
        while (true) {
            if (banner.error != null) {
                Debug.Log("Error: " + banner.error.description);
                break;
            } else if (banner.loaded) {
                banner.Show();
                break;
            }
            yield return null;
        }
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass plugin = new AndroidJavaClass("net.oira_project.adstirunityplugin.AdBannerController");
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        while (true) {
            plugin.CallStatic("tryCreateBanner", activity, mAdStirMediaId, mAdStirSpotId);
            yield return new WaitForSeconds(Mathf.Max(30.0f, mRefreshTime));
        }
#else
        return null;
#endif
    }
}
