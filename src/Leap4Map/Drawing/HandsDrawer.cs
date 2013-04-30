using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Leap4Map.Extensions;
using Leap;
using System.Windows;


namespace Leap4Map.Drawing
{
    public class HandsDrawer : IHandsDrawer
    {

        #region Attributes

        private Image _rightHandImage;
        private Image _leftHandImage;

        private HandsState _handsState;
        #endregion

        #region Properties
        public Image RightHandImage
        {
            get { return _rightHandImage; }
            set { 
                    _rightHandImage = value;
                    CreateTransformGroup(_rightHandImage);
                }
        }

        public Image LeftHandImage
        {
            get { return _leftHandImage; }
            set
            {
                _leftHandImage = value;
                CreateTransformGroup(_leftHandImage);
            }
        }

        public BitmapImage RightHandBrowsingSource { get; set; }

        public BitmapImage LeftHandBrowsingSource { get; set; }

        public BitmapImage RightHandPanningSource { get; set; }

        public BitmapImage LeftHandPanningSource { get; set; }

        public BitmapImage RightHandZoomingSource { get; set; }

        public BitmapImage LeftHandZoomingSource { get; set; }

        #endregion

        #region Public Methods
        
        public void Initialize()
        {
            SetHandsState(HandsState.Browsing);
            UpdateRightHandImage();
            UpdateLeftHandImage();
        }

        public void DrawHands(Leap.Vector rightHandPoint, Leap.Vector leftHandPoint, double screenWidth, double screenHeight)
        {
            DrawRightHand(rightHandPoint, screenWidth, screenHeight);
            DrawLeftHand(leftHandPoint, screenWidth, screenHeight);
        }

        public void DrawRightHand(Leap.Vector rightHandPoint, double screenWidth, double screenHeight)
        {
            DrawHand(rightHandPoint, RightHandImage, screenWidth, screenHeight);
        }

        public void DrawLeftHand(Leap.Vector leftHandPoint, double screenWidth, double screenHeight)
        {
            DrawHand(leftHandPoint, LeftHandImage, screenWidth, screenHeight);
        }

        public void HideRightHand()
        {
            HideHand(RightHandImage);
        }

        public void HideLeftHand()
        {
            HideHand(LeftHandImage);
        }

        public void SetHandsState(HandsState handState)
        {
            var stateChanged = (_handsState != handState);
            _handsState = handState;

            if (stateChanged)
            {
                UpdateRightHandImage();
                UpdateLeftHandImage();
            }
        }

        #endregion


        #region Private Methods

        private void UpdateRightHandImage()
        {
            if (RightHandImage == null) return;
            switch (_handsState)
            {
                case HandsState.Browsing:
                    RightHandImage.Source = RightHandBrowsingSource;
                    break;
                case HandsState.Panning:
                    RightHandImage.Source = RightHandPanningSource;
                    break;
                case HandsState.Zooming:
                    RightHandImage.Source = RightHandZoomingSource;
                    break;
            }
        }

        private void UpdateLeftHandImage()
        {
            if (LeftHandImage == null) return;
            switch (_handsState)
            {
                case HandsState.Browsing:
                    LeftHandImage.Source = LeftHandBrowsingSource;
                    break;
                case HandsState.Panning:
                    LeftHandImage.Source = LeftHandPanningSource;
                    break;
                case HandsState.Zooming:
                    LeftHandImage.Source = LeftHandZoomingSource;
                    break;
            }
        }

        private static void DrawHand(Leap.Vector handPoint, Image handImage, double screenWidth, double screenHeight)
        {
            ShowHand(handImage);
            var handScreenPoint = handPoint.ToScreenPoint(screenWidth, screenHeight);
            UpdateHandPosition(handImage, handScreenPoint);
        }

        private static void ShowHand(Image handImage)
        {
            handImage.Visibility = Visibility.Visible;
        }

        public static void HideHand(Image handImage)
        {
            handImage.Visibility = Visibility.Hidden;
        }

        private static void UpdateHandPosition(Image handImage, System.Windows.Point newPosition)
        {
            var tt = (TranslateTransform)((TransformGroup)handImage.RenderTransform).Children.First(tr => tr is TranslateTransform);
            tt.X = newPosition.X - (handImage.Width / 2);
            tt.Y = newPosition.Y - (handImage.Height / 2);
        }

        private static void CreateTransformGroup(Image handImage)
        {
            var group = new TransformGroup();
            var tt = new TranslateTransform();
            group.Children.Add(tt);
            handImage.RenderTransform = group;
        }

        #endregion
    }
}
