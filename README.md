nui4map.net
==========
A tool set of .NET libraries to ease creation of WPF applications with **maps controlled by Natural User Interfaces (NUI) sensors**.   
Currently supported sensors are:
* Microsoft Kinect
* Leap Motion 

Supported WPF map controls are:
* ESRI ArcGIS Runtime for WPF
* Telerik RadMap Control

Sensors
----------
**Kinect** - You can use a Microsoft Kinect for Windows sensor, or event your Kinect for XBox 360 sensor (with an adapter cable) for testing (MS Kinect SDK 1.5)  
![Kinect4Map Sample App screenshot](https://raw.github.com/gabrielspmoreira/nui4map.net/master/resources/leapmotion.jpg) 

**Leap Motion** - It is currently in pre-release phase, and according (Leap)[http://www.leapmotion.com/] will be available on June 2013.  
![LeapMotion](https://raw.github.com/gabrielspmoreira/nui4map.net/master/resources/screenshot_demoapp.png) 

Runtime
----------
**Kinect**  
- Browse - Move hands in any direction and see hands moving on screen
- Pan - Put one hand in front of your body, with streched arms, to move (pan) tha map
- Zoom - Put both hands in front of your body, with streched arms, and expand or contract them to zoom the map

**Leap**  
- Browse - Move on finger of one hand in any direction and see hands moving on screen
- Pan - Move two fingers (like a "V") to move (pan) tha map
- Zoom - Expand or contract all fingers of a hand to zoom the map

Structure
----------
* **NUI4Map** - Main library to interact with a map using a Natural User Interface (NUI) sensor
* **Kinect4Map** - Interfaces and generic implemantations for Kinect and map controls interface
* **Kinect4EsriMap** - Kinect4Map implementation for ArcGIS Runtime for WPF
* **Kinect4TelerikMap** - Kinect4Map implementation for Telerik Map Control
* **Leap4Map** - Interfaces and generic implemantations for Leap and map controls interface
* **Leap4EsriMap** - Leap4Map implementation for ArcGIS Runtime for WPF
* **Leap4TelerikMap** - Leap4Map implementation for Telerik Map Control
* **MapUtils** - Generic utilities for maps coordinates conversion and distance
* **SampleWPFMapApp** - Sample WPF application to demonstrate the usage of NUI4Map
* **EsriMapCommons** - Classes relativas ao controle de mapas da ESRI
* **TelerikMapCommons** - Classes relativas ao controle de mapas da Telerik

Dependencies
This .NET 4.0 project depends on some external libraries:

* **Microsoft Kinect SDK 1.5** or higher    
IMPORTANT - From SDK 1.6 and beyond, MS does not allow usage of XBox Kinect sensor with SDK, only Kinect for Windows Sensor. So use MS Kinect SDK 1.5 if you need to test with your XBox Kinect (not licensed for production by MS!).
* **KinectToolbox** - An open-source Kinect toolkit for .NET (downloadable from NuGet)
* **Leap SDK 0.9** or higher - Leap Official SDK (https://www.leapmotion.com/)
* **Autofac** - An open-source Dependency Injection framework (downloadable from NuGet)

The are also specific dependencies, depending on which map control you will use:
* ESRI map - Depends on **ArcGIS Runtime for WPF** (tested against version 1.0)    
For development, it requires you to be enrolled to ESRI Developer Network program and this library has a distribution-based commercial license
* Telerik Map - Depends on **Telerik Rad Controls for WPF** (tested against Q2 2012 version)

Setting Up
----------
To set which map control will be used in the sample WPF app, go to **SampleWPFMapApp\DI\DiHelper.cs** and set the desired SensorType and MapType, as follows:

```csharp
public static MapControlType MapControlType = MapControlType.EsriArcGISRuntime; //MapControlType.TelerikRadControl
public static SensorType SensorType = SensorType.Leap; //SensorType.Kinect
```

When running, you will need an internet connection for base maps retrieval (from ESRI (ArcGIS Online) or Telerik (OpenStreetBase) and a Kinect Sensor.

Roadmap
----------
We intend to expand this project in terms of new types of integration NUI sensors \and map controls, and also in the support of other map controls.   
You are welcome to contribute!
