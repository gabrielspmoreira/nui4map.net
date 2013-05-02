using System;
using System.Linq;
using Kinect.Toolbox;
using Microsoft.Kinect;
using System.Windows;
using NUI4Map.Handler;

namespace Kinect4Map.Handler
{
    public class KinectHandler : INUIHandler
    {
        #region Attributes
        protected KinectSensor kinectSensor;
        protected Skeleton[] skeletons;
        #endregion

        #region Properties
        public KinectSensor KinectSensor
        {
            get
            {
                return kinectSensor;
            }
        }
        #endregion

        #region Events
        public event Action<object> OnFrame;
        public event Action SensorInitialized;
        #endregion

        public void Start()
        {
            try
            {
                if (kinectSensor != null)
                    return;

                //listen to any status change for Kinects
                KinectSensor.KinectSensors.StatusChanged += Kinects_StatusChanged;

                //loop through all the Kinects attached to this PC, and start the first that is connected without an error.
                foreach (KinectSensor kinect in KinectSensor.KinectSensors)
                {
                    if (kinect.Status == KinectStatus.Connected)
                    {
                        kinectSensor = kinect;
                        break;
                    }
                }

                if (KinectSensor.KinectSensors.Count == 0)
                    MessageBox.Show("No Kinect found");
                else
                {
                    InitializeSensor();
                }
                    

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Stop()
        {
            if (kinectSensor != null)
            {
                kinectSensor.SkeletonFrameReady -= kinectRuntime_SkeletonFrameReady;
                kinectSensor.Stop();
                kinectSensor = null;
            }
        }

        public Skeleton GetFirstTrackedSkeleton(SkeletonFrame frame)
        {
            var skeletonData = new Skeleton[frame.SkeletonArrayLength];
            frame.CopySkeletonDataTo(skeletonData);

            var trackedSkel = TrackNearerSkeleton(skeletonData);

            var skeleton = (from s in skeletonData
                                 where s.TrackingState == SkeletonTrackingState.Tracked
                                    && s.TrackingId == trackedSkel
                                 select s).FirstOrDefault();
            return skeleton;
        }

        protected int TrackNearerSkeleton(Skeleton[] skeletonData)
        {
            float closestDistance = 10000f; // Start with a far enough distance
            int closestID = 0;

            foreach (Skeleton skeleton in skeletonData.Where(s => s.TrackingState != SkeletonTrackingState.NotTracked))
            {
                if (skeleton.Position.Z < closestDistance)
                {
                    closestID = skeleton.TrackingId;
                    closestDistance = skeleton.Position.Z;
                }
            }

            if (closestID > 0)
            {
                kinectSensor.SkeletonStream.ChooseSkeletons(closestID); // Track this skeleton
            }

            return closestID;
        }


        protected void InitializeSensor()
        {
            if (kinectSensor == null)
                return;

            // App controls with skeleton to track
            kinectSensor.SkeletonStream.AppChoosesSkeletons = true;

            kinectSensor.SkeletonStream.Enable(new TransformSmoothParameters
            {
                Smoothing = 0.5f,
                Correction = 0.5f,
                Prediction = 0.5f,
                JitterRadius = 0.05f,
                MaxDeviationRadius = 0.04f
            });
            kinectSensor.SkeletonFrameReady -= kinectRuntime_SkeletonFrameReady;
            kinectSensor.SkeletonFrameReady += kinectRuntime_SkeletonFrameReady;

            kinectSensor.Start();

            if (SensorInitialized != null)
            {
                SensorInitialized();
            }            
        }

        private void Kinects_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case KinectStatus.Connected:
                    if (kinectSensor == null)
                    {
                        kinectSensor = e.Sensor;
                        InitializeSensor();
                    }
                    break;
                case KinectStatus.Disconnected:
                    if (kinectSensor == e.Sensor)
                    {
                        Stop();
                        MessageBox.Show("Kinect was disconnected");
                    }
                    break;
                case KinectStatus.NotReady:
                    break;
                case KinectStatus.NotPowered:
                    if (kinectSensor == e.Sensor)
                    {
                        Stop();
                        MessageBox.Show("Kinect is no more powered");
                    }
                    break;
                default:
                    break;
            }
        }

        

        private void kinectRuntime_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame frame = e.OpenSkeletonFrame())
            {
                if (frame == null)
                    return;

                Tools.GetSkeletons(frame, ref skeletons);

                if (skeletons.All(s => s.TrackingState == SkeletonTrackingState.NotTracked))
                    return;

                var skeleton = GetFirstTrackedSkeleton(frame);

                if (OnFrame != null)
                {
                    OnFrame(skeleton);
                }
            }
        }
    }
}
