#import <Foundation/Foundation.h>

#import "AdstirView.h"

@interface AdBunnerViewController : UIViewController <AdstirViewDelegate>
    @property (nonatomic, retain) AdstirView* adview;

- (void)InitView:(NSString*)media spotNo:(int)spotNo;
@end

@implementation AdBunnerViewController

- (void)InitView:(NSString*)media spotNo:(int)spotNo
{
	self.adview = [[[AdstirView alloc]initWithOrigin:CGPointMake(0, 0)]autorelease];
	self.adview.media = media;
	self.adview.spot = spotNo;
	self.adview.rootViewController = self;
    
    self.adview.delegate = self;
    
	[self.adview start];
	[self.view addSubview:self.adview];
}

- (void)viewWillDisappear:(BOOL)animated
{
	[self.adview stop];
	[self.adview removeFromSuperview];
	self.adview.rootViewController = nil;
    self.adview.delegate = nil;
	self.adview = nil;
	[super viewWillDisappear:animated];
}

- (void)adstirDidReceiveAd:(AdstirView*)adstirview{
    UnitySendMessage("adManager", "AdSuccess", nil);
}
- (void)adstirDidFailToReceiveAd:(AdstirView*)adstirview{
    UnitySendMessage("adManager", "Adfield", nil);
}

@end


static AdBunnerViewController* adBunnerContoller;

extern "C" void _tryCreateBanner(const char* url, int spotNo, int refreshTime ) {
    
    if( adBunnerContoller != nil)
    {
        NSString* media =  [NSString stringWithCString:url encoding:NSUTF8StringEncoding];
        adBunnerContoller = [[AdBunnerViewController alloc]init];
        [adBunnerContoller InitView:media spotNo:spotNo];
    }
}
