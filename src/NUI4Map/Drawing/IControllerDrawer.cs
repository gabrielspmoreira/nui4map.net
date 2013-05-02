using System.Windows.Controls;
using System.Windows.Media.Imaging;
using NUI4Map.Structs;

namespace NUI4Map.Drawing
{
    public interface IControllerDrawer
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
        void DrawHands(Vector3D rightHandPoint, Vector3D leftHandPoint, double screenWidth, double screenHeight);
        void DrawRightHand(Vector3D rightHandPoint, double screenWidth, double screenHeight);
        void DrawLeftHand(Vector3D rightHandPoint, double screenWidth, double screenHeight);
        void HideRightHand();
        void HideLeftHand();
        void SetHandsState(ControllerState handState);
        
    }
}