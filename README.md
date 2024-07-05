# okapilabUIVisibilityController
A UIVisibilityController for Seth Robinson's GPTAvatar.

## Introduction
This is an extension for the wonderful software GPTAvatar created by Seth Robinson, https://github.com/SethRobinson/GPTAvatar.
GPTAvatar is a free open-source software that allows creating AI-based avatars with Unity.

## This class
The class UIVisibilityController is part of Maximilian C. Fink's OKAPILabExtensions for GPTAvatar.
The class allows to hide/show the buttons/labels with a "GUI on/off" Button.
This extension can be used in experimental settings where participant's should not see the full GUI all the time.
It should work on the desktop version of GPTAvatar, but potentially also on its WebGL version.

## Setting up this extension
To use this extension, the following steps have to be done
1) Copy this file in Unity somehwere under your assets (e.g., in a separate folder OKAPILabExtensions)
2) Click in the Unity hierarchy on "Canvas" and open the child object "Panel"
3) Click on the "Panel" component on "Add Component" and then type/select "UIVisibilityController" -> this adds the C# file
   Now everything should run as intended. No further changes should be neccessary. Try it in play mode and make builds :)
4) Click on the button "GUI on" to hide/show the UI.

## Using GPTAvatar for research and collaboration opportunities
If you use GPTAvatar for psychological/educational research, please either cite our article 
http://www.dx.doi.org/10.3389/feduc.2024.1416307 or Seth Robinson's github repository.
If you need more code modification or want to collaborate for scientific purposes, please reach out to Maximilian C. Fink (maximilian.fink@yahoo.com).
I'm always happy to help and conduct research together :)
