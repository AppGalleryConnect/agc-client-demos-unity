# AGC_Unity_Client_SDK_Demo
[![license](https://img.shields.io/badge/license-Apache--2.0-green)](./LICENCE)

This repository contains the source code for AppGallery Connect Unity demos, which are developed by the AppGallery Connect team.

These demos enable access to using specific APIs. For more demos and how to use them, please refer to the [AppGallery Connect documentation](https://developer.huawei.com/consumer/en/doc/development/AppGallery-connect-Guides/unity-agcsdk-getting-started-0000001322913745).


## Introduction
These are the available plugins in this repository.

| Demo             | Version   | Documentation |
|------------------|-----------|---------------|
|[authservice](https://github.com/AppGalleryConnect/agc-client-demos-unity/tree/main/auth)| [![version](https://img.shields.io/badge/Release-1.4.0.300-yellow)](./)|[Getting Started](https://developer.huawei.com/consumer/en/doc/development/AppGallery-connect-Guides/agc-get-started-unity-0000001292077664) [API Reference](https://developer.huawei.com/consumer/en/doc/development/AppGallery-connect-References/unity-api-auth-overview-0000001344616785) |
|[cloudfunction](./cloud-functions/)|[![version](https://img.shields.io/badge/Release-1.4.0.300-yellow)](./)|[Getting Started](https://developer.huawei.com/consumer/en/doc/development/AppGallery-connect-Guides/agc-get-started-unity-0000001292077664) [API Reference](https://developer.huawei.com/consumer/en/doc/development/AppGallery-connect-References/agc-cloudfunctions-crossframework-api-0000001172879083) |
|[clouddb](./clouddb/)|[![version](https://img.shields.io/badge/Release-1.4.0.300-yellow)](./)|[Getting Started](https://developer.huawei.com/consumer/en/doc/development/AppGallery-connect-Guides/agc-get-started-unity-0000001292077664) [API Reference](https://developer.huawei.com/consumer/en/doc/development/AppGallery-connect-Guides/agc-clouddb-introduction-0000001054212760) |
|[storage](./storage/)|[![version](https://img.shields.io/badge/Release-1.4.0.300-yellow)](./)|[Getting Started](https://developer.huawei.com/consumer/en/doc/development/AppGallery-connect-Guides/agc-get-started-unity-0000001292077664) [API Reference](https://developer.huawei.com/consumer/en/doc/development/AppGallery-connect-Guides/agc-cloudstorage-introduction-0000001054847259) |
|[crash](./crash/)|[![version](https://img.shields.io/badge/Release-1.4.0.300-yellow)](./)|[Getting Started](https://developer.huawei.com/consumer/en/doc/development/AppGallery-connect-Guides/agc-get-started-unity-0000001292077664) [API Reference](https://developer.huawei.com/consumer/en/doc/development/AppGallery-connect-Guides/agc-crash-introduction-0000001055732708) |


## Third Party Login Provider Implementation

Unity Client SDK can help you authenticate users with third-party accounts when they sign in to your app. For further information you can check [Auth Service Introduction](https://developer.huawei.com/consumer/en/doc/development/AppGallery-connect-Guides/agc-auth-introduction-0000001053732605).

The Auth Service Demo Project Provides sample codes for Third Party Login for Facebook SDK. For Integration process you can follow the steps in [Facebook Unity SDK](https://developers.facebook.com/docs/unity/gettingstarted#addsdk)

After you complete these steps. You can use following Facebook Auth Provider class inside the demo project:

`Assets/Resources/Scripts/ThirdPartyProvider.cs`


## Precautions
Unity IDE 2020 or above should be installed on your environment. 

After downloading the code, you can load the project to the Unity IDE as required.

In order to successfully run this demo project you need to add your **agconnect-services.json** configuration file of your app on AppGallery Connect to the project file on `Assets/Resources/agconnect-services.json`.

## Technical Support
If you have any questions about the sample code, try the following:
- Visit [Stack Overflow](https://stackoverflow.com/questions/tagged/appgallery), submit your questions, and tag them with `appgallery` or `appgallery connect`.
- Go to **AppGallery** in the [HUAWEI Developer Forum](https://forums.developer.huawei.com/forumPortal/en/home?fid=0101188387844930001) and communicate with other developers.
- [Submit a ticket online](https://developer.huawei.com/consumer/en/support/feedback/#/) to contact Huawei technical support if you encounter a serious or an urgent problem.

