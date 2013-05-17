using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using MapUtils.Structs;
using Microsoft.Kinect;
using NUI4Map.SampleWPFMapApp.DI;
using NUI4Map.Structs;
using Leap;
using NUI4Map.Drawing;
using Leap4Map.Extensions;
using NUI4Map.Gestures;
using NUI4Map.Handler;
using Kinect4Map.Extensions;

namespace NUI4Map.SampleWPFMapApp.View
{
    /// <summary>
    /// Interaction logic for DemoMap.xaml
    /// </summary>
    public partial class DemoMap
    {
        #region Attributes

        private INUIHandler _nuiHandler;
        
        private IMapZoomGestureHandler _zoomGestureHandler;
        private IMapPanGestureHandler _panGestureHandler;
        private IMapClickGestureHandler _mapClickGestureHandler;
        private IControllerDrawer _handsDrawer;

        private readonly IContainer _container;

        private IMapHandler _mapHandler;

        #endregion

        public DemoMap()
        {
            InitializeComponent();
            Loaded += WindowLoaded;
            Closing += WindowClosing;

            _container = DiHelper.GetContainer();

        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            _mapHandler = _container.Resolve<IMapHandler>();
            _mapHandler.MapInitialized += MapInitialized;
            var map = _mapHandler.CreateMap();
            Grid.SetRow(map,0);
            ViewGrid.Children.Insert(0, map);

            // To alter the Handler for Kinect or Leap, change the Dependency Injection in DI\DiHelper
            _nuiHandler = _container.Resolve<INUIHandler>();

            if (_nuiHandler.SensorType == SensorType.Leap)
            {
                _nuiHandler.OnFrame += LeapHandler_OnFrame;
            }
            else if (_nuiHandler.SensorType == SensorType.Kinect)
            {
                _nuiHandler.OnFrame += KinectHandler_OnFrame;
            }
            
            _nuiHandler.Start();
            
            _zoomGestureHandler = _container.Resolve<IMapZoomGestureHandler>();
            _zoomGestureHandler.MapComponent = map;

            _panGestureHandler = _container.Resolve<IMapPanGestureHandler>();
            _panGestureHandler.MapComponent = map;

            _mapClickGestureHandler = _container.Resolve<IMapClickGestureHandler>();
            _mapClickGestureHandler.MapComponent = map;
            _mapClickGestureHandler.NUIMapClick += MapClick;
            _mapClickGestureHandler.MouseMapClick += MapClick;

            _handsDrawer = HandsDrawerHelper.GetHandsDrawer();
            // Setting the images to be presented in the place of user's hands (Kinect) / fingers (Leap)
            ViewGrid.Children.Add(_handsDrawer.RightHandImage);
            ViewGrid.Children.Add(_handsDrawer.LeftHandImage);
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _nuiHandler.Stop();
        } 

        private void MapInitialized()
        {

        }

        private void LeapHandler_OnFrame(object controller)
        {
            this.Dispatcher.BeginInvoke(
                new Action(() =>
                    {
                        StatusLabel.Text = "";
                    }));

            // Get the most recent frame and report some basic information
		    var frame = ((Controller)controller).Frame();
 
            if (!frame.Hands.Empty)
            {
                // Get the first hand
                var hand = frame.Hands[0];

                // Check if the hand has any fingers
                var fingers = hand.Fingers;
                if (!fingers.Empty)
                {
                    if (fingers.Count > 0)
                    {
                        // Necessary because it is only possible to alter UI elements in the main thread
                        this.Dispatcher.BeginInvoke(
                            new Action(() =>
                                {
                                    if (_mapClickGestureHandler.Detect(frame))
                                    {
                                        StatusLabel.Text = "Clicking";
                                    }
                                    else if (_panGestureHandler.Detect(frame))
                                    {
                                        StatusLabel.Text = "Panning";
                                        _handsDrawer.SetHandsState(ControllerState.Panning);
                                        _handsDrawer.DrawRightHand(fingers[0].TipPosition.ToVector3D(), ActualWidth,
                                                                   ActualHeight);
                                    }
                                    else if (_zoomGestureHandler.Detect(frame))
                                    {
                                        StatusLabel.Text = "Zooming";
                                        _handsDrawer.SetHandsState(ControllerState.Zooming);
                                        _handsDrawer.DrawRightHand(fingers[0].TipPosition.ToVector3D(), ActualWidth,
                                                                   ActualHeight);
                                    }
                                    else
                                    {
                                        StatusLabel.Text = "Browsing";
                                        _handsDrawer.SetHandsState(ControllerState.Browsing);
                                        _handsDrawer.DrawRightHand(fingers[0].TipPosition.ToVector3D(), ActualWidth,
                                                                   ActualHeight);
                                    }

                                }));
                    }
                }
            }
        }



        private void KinectHandler_OnFrame(object skeletonObj)
        {
            StatusLabel.Text = "";
            var skeleton = (Skeleton) skeletonObj;

            if (skeleton != null)
            {
                var rightHandJoint = skeleton.Joints.First(j => j.JointType == JointType.HandRight);
                var leftHandJoint = skeleton.Joints.First(j => j.JointType == JointType.HandLeft);

                // If panning
                if (_panGestureHandler.Detect(skeleton))
                {
                    StatusLabel.Text = "Panning";
                    _handsDrawer.SetHandsState(ControllerState.Panning);

                    if (_panGestureHandler.PanningHand == NUI4Map.Structs.Hand.Right)
                    {
                        _handsDrawer.DrawRightHand(rightHandJoint.Position.ToVector3D(), ActualWidth, ActualHeight);
                        _handsDrawer.HideLeftHand();

                        if (_mapClickGestureHandler.Detect(skeleton))
                            StatusLabel.Text = "Clicking";
                    }
                    else
                    {
                        _handsDrawer.DrawLeftHand(leftHandJoint.Position.ToVector3D(), ActualWidth, ActualHeight);
                        _handsDrawer.HideRightHand();

                        if (_mapClickGestureHandler.Detect(skeleton))
                            StatusLabel.Text = "Clicking";
                    }
                }
                    // If zooming
                else if (_zoomGestureHandler.Detect(skeleton))
                {
                    StatusLabel.Text = "Zooming";
                    _handsDrawer.SetHandsState(ControllerState.Zooming);
                    _handsDrawer.DrawHands(rightHandJoint.Position.ToVector3D(),
                                           leftHandJoint.Position.ToVector3D(), 
                                           ActualWidth, ActualHeight);
                }
                    // If browsing
                else
                {
                    StatusLabel.Text = "Browsing";
                    _handsDrawer.SetHandsState(ControllerState.Browsing);
                    _handsDrawer.DrawHands(rightHandJoint.Position.ToVector3D(),
                                           leftHandJoint.Position.ToVector3D(), 
                                           ActualWidth, ActualHeight);           
                }
            }
            else
            {
                _handsDrawer.HideRightHand();
                _handsDrawer.HideLeftHand();
            }
        }

        private void MapClick(MapCoord coord)
        {
            ShowClickedFlag(coord);
        }


        private void ShowClickedFlag(MapCoord handCoordinate)
        {
            _mapHandler.ShowClickedFlag(handCoordinate, new Uri("/Resources/Images/flag_target.png", UriKind.Relative));
        }
        
    }
}
