using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class AdBannerObserver : MonoBehaviour {
    private static AdBannerObserver sInstance;
    
    [SerializeField] string adStirMediaId;
    [SerializeField] int adStirSpotId;
    [SerializeField] int refreshTime;
	
#if UNITY_IPHONE
	[DllImport("__Internal")]
	private static extern void _tryCreateBanner(string url, int spotNo, int refreshTime );
#endif
	
	void Awake(){
		gameObject.name = "AdBannerObserver";
	}
    
    IEnumerator Start () {
#if UNITY_IPHONE && !UNITY_EDITOR
		_tryCreateBanner(adStirMediaId, adStirSpotId, refreshTime);
		
		yield return null;
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass plugin = new AndroidJavaClass("net.oira_project.adstirunityplugin.AdBannerController");
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        while (true) {
            plugin.CallStatic("tryCreateBanner", activity, adStirMediaId, adStirSpotId);
            yield return new WaitForSeconds(Mathf.Max(30.0f, refreshTime));
        }
#else
		gameObject.SendMessage("AdSuccess", "");
        return null;
#endif
    }
	void AdSuccess(string str)
	{
		Debug.Log("success");
	}
	
	void Adfield(string str)
	{
		Debug.Log("failed");
	}	
}
