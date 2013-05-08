using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using Leap4Map.Drawing;
using Leap4Map.Gestures;
using Leap4Map.Handler;
using MapUtils.Structs;
using SampleWPFMappApp.DI;
using Leap;
using System.Diagnostics;
using NUI4Map.Drawing;
using Leap4Map.Extensions;
using NUI4Map.Gestures;
using Kinect4Map.KinectUtils;
using NUI4Map.Handler;

namespace SampleWPFMapApp.View
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

            _nuiHandler = _container.Resolve<INUIHandler>();
            _nuiHandler.OnFrame += LeapHandler_OnFrame;
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
            // Incluindo imagens na tela para que sejam exibidas quando o usuário interagir com Kinect
            ViewGrid.Children.Add(_handsDrawer.RightHandImage);
            ViewGrid.Children.Add(_handsDrawer.LeftHandImage);
        }

        private void LeapHandler_OnFrame(object controller)
        {
            // Get the most recent frame and report some basic information
		    var frame = ((Controller)controller).Frame ();

            if (!frame.Hands.Empty)
            {
                // Get the first hand
                Leap.Hand hand = frame.Hands[0];

                // Check if the hand has any fingers
                FingerList fingers = hand.Fingers;
                if (!fingers.Empty)
                {
                    if (fingers.Count > 0)
                    {

                        this.Dispatcher.BeginInvoke(
                            new Action(() =>
                            {
                                /*if (_mapClickGestureHandler.Detect(frame))
                                {

                                }
                                else*/ if (_panGestureHandler.Detect(frame))
                                {
                                    _handsDrawer.SetHandsState(ControllerState.Panning);
                                    _handsDrawer.DrawRightHand(fingers[0].TipPosition.ToVector3D(), ActualWidth, ActualHeight);
                                }
                                else if (_zoomGestureHandler.Detect(frame))
                                {
                                    _handsDrawer.SetHandsState(ControllerState.Zooming);
                                    _handsDrawer.DrawRightHand(fingers[0].TipPosition.ToVector3D(), ActualWidth, ActualHeight);
                                }
                                else
                                {
                                    _handsDrawer.SetHandsState(ControllerState.Browsing);
                                    _handsDrawer.DrawRightHand(fingers[0].TipPosition.ToVector3D(), ActualWidth, ActualHeight);
                                }

                                       StatusLabel.Text = fingers[0].TipPosition.DistanceTo(fingers[1].TipPosition).ToString();

                            }));
                    }
                }
            }
        }

        private void MapInitialized()
        {
            
        }

        /*private void kinectHandler_CapturedSkeleton(object frame)
        {
            var skeleton = (Skeleton) frame;
            ProcessFrame(skeleton);
        }

        private void ProcessFrame(SkeletonFrame frame)
        {
            StatusLabel.Text = "";
            var skeleton = _kinectHandler.GetFirstTrackedSkeleton(frame);

            if (skeleton != null)
            {
                //Dictionary<int, string> stabilities = new Dictionary<int, string>();
                //barycenterHelper.Add(skeleton.Position.ToVector3(), skeleton.TrackingId);
                //if (!barycenterHelper.IsStable(skeleton.TrackingId))
                //    return;

                var rightHandJoint = skeleton.Joints.First(j => j.JointType == JointType.HandRight);
                var rightHandTracked = rightHandJoint.TrackingState == JointTrackingState.Tracked;
                var rightHandPosition = rightHandJoint.Position;
                
                var leftHandJoint = skeleton.Joints.First(j => j.JointType == JointType.HandLeft);
                var leftHandTracked = leftHandJoint.TrackingState == JointTrackingState.Tracked;
                var leftHandPosition = leftHandJoint.Position;

                var shoulderCenterPosition =
                    skeleton.Joints.First(j => j.JointType == JointType.ShoulderCenter).Position;
                var headPosition = skeleton.Joints.First(j => j.JointType == JointType.Head).Position;

                // Calculate Skeleton Height (without head)
                var skeletonHeight = skeleton.Height();
                var minZDistanceFromBody = skeletonHeight/4;
                _panGestureHandler.MinZDistanceFromBody = minZDistanceFromBody;
                _zoomGestureHandler.MinZDistanceFromBody = minZDistanceFromBody;


                // If panning
               if (_panGestureHandler.Detect(shoulderCenterPosition, rightHandPosition, rightHandTracked,
                                              leftHandPosition, leftHandTracked))
                {
                    StatusLabel.Text = "Panning";
                    _handsDrawer.SetHandsState(HandsState.Panning);

                    if (_panGestureHandler.PanningHand == Hand.Right)
                    {
                        _handsDrawer.DrawRightHand(rightHandJoint, ActualWidth, ActualHeight);
                        _handsDrawer.HideLeftHand();

                        if (_mapClickGestureHandler.Detect(rightHandPosition, leftHandPosition, headPosition))
                            StatusLabel.Text = "Clicking";
                    }
                    else
                    {
                        _handsDrawer.DrawLeftHand(leftHandJoint, ActualWidth, ActualHeight);
                        _handsDrawer.HideRightHand();

                        if (_mapClickGestureHandler.Detect(leftHandPosition, rightHandPosition, headPosition))
                            StatusLabel.Text = "Clicking";
                    }
                }
                    // If zooming
                else if (_zoomGestureHandler.Detect(shoulderCenterPosition, rightHandPosition, rightHandTracked,
                                                    leftHandPosition, leftHandTracked))
                {
                    StatusLabel.Text = "Zooming";
                    _handsDrawer.SetHandsState(HandsState.Zooming);
                    _handsDrawer.DrawHands(rightHandJoint, leftHandJoint, ActualWidth, ActualHeight);
                }
                    // If browsing
                else
                {
                    StatusLabel.Text = "Browsing";
                    _handsDrawer.SetHandsState(HandsState.Browsing);
                    _handsDrawer.DrawHands(rightHandJoint, leftHandJoint, ActualWidth, ActualHeight);           
                }
            }
            else
            {
                _handsDrawer.HideRightHand();
                _handsDrawer.HideLeftHand();
            }
        }
        */

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //_kinectHandler.CapturedSkeletonFrame -= kinectHandler_CapturedSkeletonFrame;

            _nuiHandler.Stop();
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
