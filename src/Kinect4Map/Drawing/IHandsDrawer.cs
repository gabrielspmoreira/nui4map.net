using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;

namespace Kinect4Map.Drawing
{
    public interface IHandsDrawer
    {
        Image RightHandImage { get; set; }
        Image LeftHandImage { get; set; }
        BitmapImage RightHandBrowsingSource { get; set; }
        BitmapImage LeftHandBrowsingSource { get; set; }
        BitmapImage RightHandPanningSource { get; set; }
        BitmapImage LeftHandPanningSource { get; set; }
        BitmapImage RightHandZoomingSource { get; set; }
        BitmapImage LeftHandZoomingSource { get; set; }
        void Initialize();
        void DrawHands(Joint rightHandJoint, Joint leftHandJoint, double screenWidth, double screenHeight);
        void DrawRightHand(Joint rightHandJoint, double screenWidth, double screenHeight);
        void DrawLeftHand(Joint rightHandJoint, double screenWidth, double screenHeight);
        void HideRightHand();
        void HideLeftHand();
        void SetHandsState(HandsState handState);
        
    }
}