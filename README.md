kinect4map
==========
A tool set of .NET libraries to ease creation of WPF applications with **maps controlled by Microsoft Kinect**.   
Currently supported WPF map controls are:
* ESRI ArcGIS Runtime for WPF
* Telerik Map Control

You can use a Microsoft Kinect for Windows sensor, or event your Kinect for XBox 360 sensor (with an adapter cable) for testing (MS Kinect SDK 1.5)

![Kinect4Map Sample App screenshot](https://raw.github.com/gabrielspmoreira/kinect4map/master/resources/screenshot_demoapp.png) 

Structure
----------
* **Kinect4Map** - Interfaces and generic implemantations for Kinect and map controls interface
* **Kinect4EsriMap** - Kinect4Map implementation for ArcGIS Runtime for WPF
* **Kinect4TelerikMap** - Kinect4Map implementation for Telerik Map Control
* **MapUtils** - Generic utilities for maps coordinates conversion and distance
* **SampleWPFMapApp** - Sample WPF application to demonstrate the usage of Kinect4Map

Dependencies
----------
This .NET 4.0 project depends on some external libraries:
* **Microsoft Kinect SDK 1.5** or higher    
IMPORTANT - From SDK 1.6 and beyond, MS does not allow usage of XBox Kinect sensor with SDK, only Kinect for Windows Sensor. So use MS Kinect SDK 1.5 if you need to test with your XBox Kinect (not licensed for production by MS!).
* **KinectToolbox** - An open-source Kinect toolkit for .NET (downloadable from NuGet)
* **Autofac** - An open-source Dependency Injection framework (downloadable from NuGet)

The are also specific dependencies, depending on which map control you will use:
* Kinect4EsriMap - Depends on **ArcGIS Runtime for WPF** (tested against version 1.0)    
For development, it requires you to be enrolled to ESRI Developer Network program and this library has a distribution-based commercial license
* Kinect4TelerikMap - Depends on **Telerik Rad Controls for WPF** (tested against Q2 2012 version)

Setting Up
----------
To set which map control will be used in the sample WPF app, go to **SampleWPFMapApp\DI\DiHelper.cs** and register the module of desired control (EsriMapModule or TelerikMapModule), as bellow:

```csharp
//// SWITCH BETWEEN ARCGIS RUNTIME FOR WPF OR TELERIK MAP CONTROL   
builder.RegisterModule(new EsriMapModule());   
//builder.RegisterModule(new TelerikMapModule());   
```

When running, you will need an internet connection for base maps retrieval (from ESRI (ArcGIS Online) or Telerik (OpenStreetBase) and a Kinect Sensor.

Roadmap
----------
We intend to expand this project in terms of new types of integration of Kinect and map controls, and also in the support of other map controls.   
You are welcome to contribute!
