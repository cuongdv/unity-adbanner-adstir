package net.oira_project.adstirunityplugin;

import com.ad_stir.AdstirView;

import android.app.Activity;
import android.util.Log;
import android.view.Gravity;
import android.view.ViewGroup.LayoutParams;
import android.widget.RelativeLayout;

public class AdBannerController {
    static final int bannerViewId = 0x661ad306; // "unique ID"
    
    static public void tryCreateBanner(final Activity activity, final String mediaId, final int spotNo) {
        activity.runOnUiThread(new Runnable() {
            public void run() {
            	AdstirView adstirView = (AdstirView)activity.findViewById(bannerViewId);
            	if (adstirView == null) {
            		Log.d("adstir Plugin", "creates an ad banner.");
            		RelativeLayout layout = new RelativeLayout(activity);
            		activity.addContentView(layout, new LayoutParams(LayoutParams.FILL_PARENT, LayoutParams.FILL_PARENT));
            		layout.setGravity(Gravity.BOTTOM);
            		// Make a banner
            		adstirView = new AdstirView(activity, mediaId, spotNo);
            		
            		adstirView.setId(bannerViewId);
            		layout.addView(adstirView);
            	}
            }
        });
    }
}
