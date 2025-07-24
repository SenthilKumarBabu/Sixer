
#import "ShareThisGame.h"
#import "InternetReachability.h"
#import <CoreTelephony/CTTelephonyNetworkInfo.h>


@implementation ShareThisGame : UIViewController
#define SYSTEM_VERSION_GREATER_THAN(v) ([[[UIDevice currentDevice] systemVersion] compare:v options:NSNumericSearch] == NSOrderedDescending)
#define SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(v) ([[[UIDevice currentDevice] systemVersion] compare:v options:NSNumericSearch] != NSOrderedAscending)

-(void) shareMethod: (const char *) path: (const char *) shareMessage
{
    NSString *imagePath = [NSString stringWithUTF8String:path];
    
    UIImage *image = [UIImage imageWithContentsOfFile:imagePath];
    NSString *message   = [NSString stringWithUTF8String:shareMessage];
    NSArray *postItems  = @[message,image];
    
    UIActivityViewController *activityVc = [[UIActivityViewController alloc]initWithActivityItems:postItems applicationActivities:nil];
    NSMutableArray *excludeActivities = [[NSMutableArray alloc] initWithObjects:UIActivityTypePrint,UIActivityTypeAssignToContact,UIActivityTypeSaveToCameraRoll,UIActivityTypeCopyToPasteboard, nil];
    
    if(SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(@"7.0"))
    {
        [excludeActivities addObject:UIActivityTypeAirDrop];
        [excludeActivities addObject:UIActivityTypeAddToReadingList];
        [excludeActivities addObject:UIActivityTypePostToFlickr];
        [excludeActivities addObject:UIActivityTypePostToVimeo];
    }
    activityVc.excludedActivityTypes = excludeActivities;
        
    if ( [activityVc respondsToSelector:@selector(popoverPresentationController)] ) {
        
        UIPopoverController *popup = [[UIPopoverController alloc] initWithContentViewController:activityVc];
        
        [popup presentPopoverFromRect:CGRectMake(self.view.frame.size.width/2, self.view.frame.size.height/4, 0, 0)
                               inView:[UIApplication sharedApplication].keyWindow.rootViewController.view permittedArrowDirections:UIPopoverArrowDirectionAny animated:YES];
    }
    else
        [[UIApplication sharedApplication].keyWindow.rootViewController presentViewController:activityVc animated:YES completion:nil];
//    [activityVc release];
}

-(void) sendRemoteNotification:(NSString *) message
{
    NSLog(@"Share this game : sendRemoteNotification()");
    UnitySendMessage("OnNewIntent","onNewIntent","test");
}

-(NSString *) getInternetConnectionType
{
    //connection type
    CTTelephonyNetworkInfo *netinfo = [[CTTelephonyNetworkInfo alloc] init];
    NSString *networkType = @"None";
    
//    InternetReachability *reachability = [InternetReachability reachabilityForInternetConnection];
//    [reachability startNotifier];
//    
//    NetworkStatus status = [reachability currentReachabilityStatus];
    
    SCNetworkReachabilityRef reachability = SCNetworkReachabilityCreateWithName(NULL, "8.8.8.8");
    SCNetworkReachabilityFlags flags;
    BOOL success = SCNetworkReachabilityGetFlags(reachability, &flags);
    CFRelease(reachability);
    if (!success) {
        
    }
    else{
        BOOL isReachable = ((flags & kSCNetworkReachabilityFlagsReachable) != 0);
        BOOL needsConnection = ((flags & kSCNetworkReachabilityFlagsConnectionRequired) != 0);
        BOOL isNetworkReachable = (isReachable && !needsConnection);
        
        if (!isNetworkReachable) {
            //        return ConnectionTypeNone;
        }
        else if ((flags & kSCNetworkReachabilityFlagsIsWWAN) != 0) {
            //        return ConnectionType3G;
            if ([netinfo.currentRadioAccessTechnology isEqualToString:CTRadioAccessTechnologyGPRS]) {
                networkType = @"2G";
                NSLog(@"2G");
            } else if ([netinfo.currentRadioAccessTechnology isEqualToString:CTRadioAccessTechnologyEdge]) {
                networkType = @"2G";
                NSLog(@"2G");
            } else if ([netinfo.currentRadioAccessTechnology isEqualToString:CTRadioAccessTechnologyWCDMA]) {
                networkType = @"3G";
                NSLog(@"3G");
            } else if ([netinfo.currentRadioAccessTechnology isEqualToString:CTRadioAccessTechnologyHSDPA]) {
                networkType = @"3G";
                NSLog(@"3G");
            } else if ([netinfo.currentRadioAccessTechnology isEqualToString:CTRadioAccessTechnologyHSUPA]) {
                networkType = @"3G";
                NSLog(@"3G");
            } else if ([netinfo.currentRadioAccessTechnology isEqualToString:CTRadioAccessTechnologyCDMA1x]) {
                networkType = @"2G";
                NSLog(@"2G");
            } else if ([netinfo.currentRadioAccessTechnology isEqualToString:CTRadioAccessTechnologyCDMAEVDORev0]) {
                networkType = @"3G";
                NSLog(@"3G");
            } else if ([netinfo.currentRadioAccessTechnology isEqualToString:CTRadioAccessTechnologyCDMAEVDORevA]) {
                networkType = @"3G";
                NSLog(@"3G");
            } else if ([netinfo.currentRadioAccessTechnology isEqualToString:CTRadioAccessTechnologyCDMAEVDORevB]) {
                networkType = @"3G";
                NSLog(@"3G");
            } else if ([netinfo.currentRadioAccessTechnology isEqualToString:CTRadioAccessTechnologyeHRPD]) {
                networkType = @"3G";
                NSLog(@"3G");
            } else if ([netinfo.currentRadioAccessTechnology isEqualToString:CTRadioAccessTechnologyLTE]) {
                networkType = @"4G";
                NSLog(@"4G");
            }
        } else {
            networkType = @"Wifi";
            NSLog(@"Wifi");
        }
        
        
    }
    return networkType;
}
//bool splashLoaded;
//-(void) presentAppRootViewController
//{
//    if(!splashLoaded)
//    {
//        NSLog(@"splash loaded");
//        splashLoaded = YES;
//        if(self.appRootViewController == nil)
//        {
//            self.appRootViewController = [[AppRootViewController alloc] init];
//            self.appRootViewController.delegate = self;
////            [UnityGetGLViewController() presentViewController:self.appRootViewController animated:NO completion:nil];
//        }
//        [UnityGetMainWindow().rootViewController presentViewController:self.appRootViewController animated:NO completion:nil];
//        
//    }
//}
//-(void)splashAdDidLoaded
//{
//    NSLog(@"SplashAdLoaded successfully");
//    [self.appRootViewController dismissViewControllerAnimated:NO completion:nil];
//    UnitySendMessage("SplashPanel", "splashLoaded", "");
//}
-(void) shareScreenWithMessage:(const char *) message andPath:(const char *) path
{
    NSData *data = [[NSData alloc] initWithContentsOfFile:[NSString stringWithUTF8String:path]];
    UIImage *image = [[UIImage alloc] initWithData:data];
    if(image == nil)
    {
        NSLog(@"image is nil");
    }
    
    NSArray *postItems;
    if(message != nil)
        postItems = @[image, [NSString stringWithUTF8String:message]];
    else
        postItems = @[image];
        
    UIActivityViewController *controller = [[UIActivityViewController alloc] initWithActivityItems:postItems applicationActivities:nil];
    [controller setValue:@"Battle of Chepauk 2" forKey:@"subject"];
    NSMutableArray *excludeActivities = [[NSMutableArray alloc] initWithObjects:UIActivityTypePrint,UIActivityTypeAssignToContact,UIActivityTypeSaveToCameraRoll,UIActivityTypeCopyToPasteboard, nil];
    
    if(SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(@"7.0"))
    {
        [excludeActivities addObject:UIActivityTypeAirDrop];
        [excludeActivities addObject:UIActivityTypeAddToReadingList];
        [excludeActivities addObject:UIActivityTypePostToFlickr];
        [excludeActivities addObject:UIActivityTypePostToVimeo];
    }
    controller.excludedActivityTypes = excludeActivities;
    
    //if iPhone
    if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPhone)
    {
        [[UIApplication sharedApplication].keyWindow.rootViewController presentViewController:controller animated:YES completion:nil];
    }
    //if iPad
    else
    {
        // Change Rect to position Popover
        if ([controller respondsToSelector:@selector(popoverPresentationController)])
        {
            UIPopoverController *popup = [[UIPopoverController alloc] initWithContentViewController:controller];
            [popup presentPopoverFromRect:CGRectMake(self.view.frame.size.width/2, self.view.frame.size.height/4, 0, 0)
                                   inView:[UIApplication sharedApplication].keyWindow.rootViewController.view permittedArrowDirections:UIPopoverArrowDirectionAny animated:YES];
        }
        else
        {
            [[UIApplication sharedApplication].keyWindow.rootViewController presentViewController:controller animated:YES completion:nil];
        }
    }

}
-(void) shareOnlyTextMethod: (const char *) shareMessage
{
    
    NSString *message   = [NSString stringWithUTF8String:shareMessage];
    NSArray *postItems  = @[message];
    
    UIActivityViewController *controller = [[UIActivityViewController alloc] initWithActivityItems:postItems applicationActivities:nil];
	[controller setValue:@"Battle of Chepauk 2" forKey:@"subject"];
    NSMutableArray *excludeActivities = [[NSMutableArray alloc] initWithObjects:UIActivityTypePrint,UIActivityTypeAssignToContact,UIActivityTypeSaveToCameraRoll,UIActivityTypeCopyToPasteboard, nil];
    
    if(SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(@"7.0"))
    {
        [excludeActivities addObject:UIActivityTypeAirDrop];
        [excludeActivities addObject:UIActivityTypeAddToReadingList];
        [excludeActivities addObject:UIActivityTypePostToFlickr];
        [excludeActivities addObject:UIActivityTypePostToVimeo];
    }
    controller.excludedActivityTypes = excludeActivities;
        
    //if iPhone
    if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPhone)
    {
        [[UIApplication sharedApplication].keyWindow.rootViewController presentViewController:controller animated:YES completion:nil];
    }
    //if iPad
    else
    {
        // Change Rect to position Popover
        if ([controller respondsToSelector:@selector(popoverPresentationController)])
        {
        	UIPopoverController *popup = [[UIPopoverController alloc] initWithContentViewController:controller];
        	[popup presentPopoverFromRect:CGRectMake(self.view.frame.size.width/2, self.view.frame.size.height/4, 0, 0)
                               inView:[UIApplication sharedApplication].keyWindow.rootViewController.view permittedArrowDirections:UIPopoverArrowDirectionAny animated:YES];
	    }
	    else
	    {
	    	[[UIApplication sharedApplication].keyWindow.rootViewController presentViewController:controller animated:YES completion:nil];
	    }
    }
}
bool isReachable;
bool isCheckedForReachability;
-(void) checkForInternet{
    InternetReachability *reachability = [InternetReachability reachabilityWithHostname:@"www.google.com"];
    reachability.reachableBlock = ^(InternetReachability *reachability) {
        isReachable = YES;
        NSLog(@"Network is reachable.");
    };
    
    reachability.unreachableBlock = ^(InternetReachability *reachability) {
        isReachable = NO;
        NSLog(@"Network is unreachable.");
    };
    
    // Start Monitoring
    [reachability startNotifier];
    
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(reachabilityDidChange:) name:kReachabilityChangedNotification object:nil];
}
- (void)reachabilityDidChange:(NSNotification *)notification {
    InternetReachability *reachability = (InternetReachability *)[notification object];
    
    if ([reachability isReachable]) {
        isReachable = YES;
        NSLog(@"Reachable");
    } else {
        isReachable = NO;
        NSLog(@"Unreachable");
    }
}

@end

extern "C"{
    void sampleMethod(const char * path, const char * message){
        ShareThisGame *vc = [[ShareThisGame alloc] init];
        [vc shareMethod: path: message];
//        [vc release];
    }
}

extern "C"{
    void sampleTextMethod(const char * message){
        ShareThisGame *vc = [[ShareThisGame alloc] init];
        [vc shareOnlyTextMethod: message];
    }
    
    void shareImageWithMessage(const char* message, const char* path)
    {
        printf("\n message : %s and path : %s",message,path);
        ShareThisGame *obj = [[ShareThisGame alloc] init];
        [obj shareScreenWithMessage:message andPath:path];
    }
    char* getNetworkConnectionType(){
        ShareThisGame *obj = [[ShareThisGame alloc] init];
        NSString *networkType = [obj getInternetConnectionType];
        NSLog(@"network type : %@",networkType);
        const char* string = [networkType UTF8String];
        if (string == NULL)
            return NULL;
        
        char* res = (char*)malloc(strlen(string) + 1);
        strcpy(res, string);
        
        return res;
        //        [vc release];
    }
    bool isConnected()
    {
        
        if(isCheckedForReachability)
            return isReachable;
        else{
            ShareThisGame *vc = [[ShareThisGame alloc] init];
            [vc checkForInternet];
            isCheckedForReachability = true;
            return isReachable;
        }
    }
    
    void checkAnyNewIntent(){
        ::printf("checkAnyNewIntent IOS native kaalirajan 000\n");
        if([[NSUserDefaults standardUserDefaults] objectForKey:@"userInfoDictInActive"])
        {
            ::printf("checkAnyNewIntent IOS native kaalirajan 11111\n");
            NSDictionary *userInfo = [[NSUserDefaults standardUserDefaults] objectForKey:@"userInfoDictInActive"];
            
            //    [UIApplication sharedApplication].applicationIconBadgeNumber = 1;
            [UIApplication sharedApplication].applicationIconBadgeNumber = 0;
            
            NSString *gameObject = @"OnNewIntent";
            NSString *methodName = @"onNewIntent";
            long long milliseconds = (long long)([[NSDate date] timeIntervalSince1970] * 1000.0);
            NSString *challengeData = [NSString stringWithFormat:@"%@ | %lld",userInfo[@"data"][@"challengedata"],milliseconds];
            NSLog(@"challenge data : %@",challengeData);
            //UnitySendMessage([gameObject UTF8String], [methodName UTF8String], [challengeData UTF8String]);
            if(userInfo[@"data"][@"challengedata"] != nil){
                NSString *challengeData = [NSString stringWithFormat:@"%@|%lld",userInfo[@"data"][@"challengedata"],milliseconds];
                NSLog(@"challenge data : %@",challengeData);
                UnitySendMessage([gameObject UTF8String], [methodName UTF8String], [challengeData UTF8String]);
            }else if(userInfo[@"data"][@"challengeReplyData"] != nil){
                NSString* challengereplyData = [NSString stringWithFormat: @"%@|%lld", userInfo[@"data"][@"challengeReplyData"], milliseconds];
                NSLog(@"challengereplyData data : %@", challengereplyData);
                UnitySendMessage([gameObject UTF8String], [methodName UTF8String], [challengereplyData UTF8String]);
            }
            [[NSUserDefaults standardUserDefaults] removeObjectForKey:@"userInfoDictInActive"];
        }else{
            ::printf("checkAnyNewIntent IOS native kaalirajan 2222222\n");
            
        }
    }
    
    void presentSplashAd()
    {
//        ShareThisGame *obj = [[ShareThisGame alloc] init];
//        [obj presentAppRootViewController];
    }
    
    void openAppReview(int appId){ 
        NSString *templateReviewURL = @"itms-apps://ax.itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id=APP_ID";
        NSString *templateReviewURLiOS7 = @"itms-apps://itunes.apple.com/app/idAPP_ID";
        NSString *templateReviewURLiOS8 = @"itms-apps://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?id=APP_ID&onlyLatestVersion=true&pageNumber=0&sortOrdering=1&type=Purple+Software";
        
        //ios7 before
        NSString *reviewURL = [[NSString alloc] init];
        reviewURL = [templateReviewURL stringByReplacingOccurrencesOfString:@"APP_ID" withString:[NSString stringWithFormat:@"%d", appId]];
        
        // iOS 7 needs a different templateReviewURL @see https://github.com/arashpayan/appirater/issues/131
        if ([[[UIDevice currentDevice] systemVersion] floatValue] >= 7.0 && [[[UIDevice currentDevice] systemVersion] floatValue] < 7.1) {
            reviewURL = [templateReviewURLiOS7 stringByReplacingOccurrencesOfString:@"APP_ID" withString:[NSString stringWithFormat:@"%d", appId]];
        }
        // iOS 8 needs a different templateReviewURL also @see https://github.com/arashpayan/appirater/issues/182
        else if ([[[UIDevice currentDevice] systemVersion] floatValue] >= 8.0)
        {
            reviewURL = [templateReviewURLiOS8 stringByReplacingOccurrencesOfString:@"APP_ID" withString:[NSString stringWithFormat:@"%d", appId]];
        }
        //UnitySendMessage([dict[@"target"] UTF8String], [dict[@"method"] UTF8String], [scoreStringBuilder UTF8String]);;
//        const char *string = [reviewURL cStringUsingEncoding:NSUTF8StringEncoding];
//        char* res = (char*)malloc(strlen(string) + 1);
//        strcpy(res, string);
//        printf("link %s",res);
        NSLog(@"review url : %@",reviewURL);
        [[UIApplication sharedApplication] openURL:[NSURL URLWithString:reviewURL]];
        
        //cStringCopy1([reviewURL UTF8String]);//[reviewURL UTF8String];
    }
}
