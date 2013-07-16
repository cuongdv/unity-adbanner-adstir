#import <Foundation/Foundation.h>

#import "AdstirView.h"

@interface AdBunnerViewController : UIViewController <AdstirViewDelegate>
@property (nonatomic, retain) AdstirView* adview;

- (void)InitView:(NSString*)media spotNo:(int)spotNo interval:(int)interval;
- (void)ShowView;
- (void)HideView;
@end

@implementation AdBunnerViewController

- (void)InitView:(NSString*)media spotNo:(int)spotNo interval:(int)interval
{
	self.adview = [[[AdstirView alloc]initWithOrigin:CGPointMake(0, 0)]autorelease];
	self.adview.media = media;
	self.adview.spot = spotNo;
	self.adview.rootViewController = self;
    
    self.adview.delegate = self;
    
}
- (void)viewWillAppear:(BOOL)animated
{
	[super viewWillAppear:animated];
}

- (void)viewWillDisappear:(BOOL)animated
{
	[super viewWillDisappear:animated];
}

- (void)ShowView
{
	[self.adview start];
	[self.view addSubview:self.adview];
}
- (void)HideView
{
	[self.adview stop];
	[self.adview removeFromSuperview];
	self.adview.rootViewController = nil;
    self.adview.delegate = nil;
	self.adview = nil;
}


- (void)adstirDidReceiveAd:(AdstirView*)adstirview{
	UnitySendMessage("AdBannerObserver", "AdSuccess", "");
}
- (void)adstirDidFailToReceiveAd:(AdstirView*)adstirview{
    
	UnitySendMessage("AdBannerObserver", "Adfield", "");
}


@end


static AdBunnerViewController* adBunnerContoller;

extern "C" void _tryCreateBanner(const char* url, int spotNo, int refreshTime ) {
    
    if( adBunnerContoller == nil)
    {
        NSString* media =  [NSString stringWithCString:url encoding:NSUTF8StringEncoding];
        
        adBunnerContoller = [[AdBunnerViewController alloc]init];
        [adBunnerContoller InitView:media spotNo:spotNo interval:refreshTime];
        [adBunnerContoller ShowView];
    }
}

extern "C" bool _showBanner( ) {
    if( adBunnerContoller == nil)
    {
        return false;
    }
    [adBunnerContoller ShowView];
    return true;
}

extern "C" void _hideBanner( ) {
    [adBunnerContoller HideView];
}