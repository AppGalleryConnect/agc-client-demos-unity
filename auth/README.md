## auth quickstart

English 

## Table of Contents

 * [Introduction](#introduction)
 * [Environment Requirements](#environment-requirements)
 * [Getting Started](#getting-started)
 * [Sample Code](#sample-Code)
 * [Technical Support](#technical-support)
 * [License](#license)

## Introduction
Auth Service integrates a client SDK and accesses our cloud service to build a secure and reliable user authentication system for your app.
Auth Service supports multiple authentication modes and is closely integrated with other serverless services, effectively protecting user data by defining simple rules.

## Environment Requirements
* A computer with Unity Engine installed for app development
* A device or an emulator in Unity Engine running Android 4.2 or a later version or IOS 14.0
	
## Getting Started
1. Check whether you have a HUAWEI ID. If not, [register one](https://developer.huawei.com/consumer/en/doc/start/registration-and-verification-0000001053628148) and pass identity verification.
2. Use your account to sign in to [AppGallery Connect](https://developer.huawei.com/consumer/en/service/josp/agc/index.html#/), create an app, and set **Package type** to **APK (Android app)**.
3. Enable authentication modes.
3.1 Sign in to AppGallery Connect, click **My projects**, and click a project that you want to enable Auth Service from the project list.
3.2 Go to **Build** > **Auth Service**. If it is the first time that you use Auth Service, click **Enable now** in the upper right corner.
3.3 Click **Enable** for each authentication mode you want to enable.
3.4 Configure information required by each authentication mode by referring to the development guide.
4. Download the **agconnect-services.json** file from AppGallery Connect and copy this file to the app's Resource directory (for example, **auth/Assets/Resources/**)

## Sample Code
The quickstart app supports the following authentication modes:
1. Phone number
Sample code: Assets\Resources\Scripts\MobileNumber..cs

2. Email address
Sample code: Assets\Resources\Scripts\Test.cs

3. Facebook account
Sample code: Assets\Resources\Scripts\ThirdParty..cs

## Third Party Login Provider Implementation

Unity Client SDK can help you authenticate users with third-party accounts when they sign in to your app. For further information you can check [Auth Service Introduction](https://developer.huawei.com/consumer/en/doc/development/AppGallery-connect-Guides/agc-auth-introduction-0000001053732605).

The Auth Service Demo Project Provides sample codes for Third Party Login for Facebook SDK. For Integration process you can follow the steps in [Facebook Unity SDK](https://developers.facebook.com/docs/unity/gettingstarted#addsdk)

After you complete these steps. You can use following Facebook Auth Provider class inside the demo project:

`Assets/Resources/Scripts/ThirdPartyProvider.cs`


## Technical Support
If you have any questions about the sample code, try the following:
- Visit [Stack Overflow](https://stackoverflow.com/questions/tagged/appgallery-connect), submit your questions, and tag them with `appgallery`. Huawei experts will answer your questions.
- Go to **AppGallery** in the [HUAWEI Developer Forum](https://forums.developer.huawei.com/forumPortal/en/home?fid=0101188387844930001) and communicate with other developers.

If you encounter any issues when using the sample code, submit your [issues](https://github.com/AppGalleryConnect/agc-android-demos/issues) or submit a [pull request](https://github.com/AppGalleryConnect/agc-android-demos/pulls).

## License
The sample code is licensed under the [Apache License, version 2.0](https://www.apache.org/licenses/LICENSE-2.0).