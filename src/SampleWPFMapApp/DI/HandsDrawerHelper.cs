using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Autofac;
using Leap4Map.Drawing;

namespace SampleWPFMappApp.DI
{
    class HandsDrawerHelper
    {
        public static IHandsDrawer GetHandsDrawer()
        {
            var container = DiHelper.GetContainer();
            var handsDrawer = container.Resolve<IHandsDrawer>();

            handsDrawer.RightHandImage = new Image
            {
                Width = 64,
                Height = 64,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Visibility = Visibility.Hidden
            };
            
            handsDrawer.LeftHandImage = new Image
            {
                Width = 64,
                Height = 64,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Visibility = Visibility.Hidden
            };
            
            handsDrawer.RightHandBrowsingSource = new BitmapImage(new Uri("/Resources/Images/handright.png", UriKind.Relative));
            handsDrawer.LeftHandBrowsingSource = new BitmapImage(new Uri("/Resources/Images/handleft.png", UriKind.Relative));
            handsDrawer.RightHandPanningSource = new BitmapImage(new Uri("/Resources/Images/handright_pan.png", UriKind.Relative));
            handsDrawer.LeftHandPanningSource = new BitmapImage(new Uri("/Resources/Images/handleft_pan.png", UriKind.Relative));
            handsDrawer.RightHandZoomingSource = new BitmapImage(new Uri("/Resources/Images/handright_zoom.png", UriKind.Relative));
            handsDrawer.LeftHandZoomingSource = new BitmapImage(new Uri("/Resources/Images/handleft_zoom.png", UriKind.Relative));
            handsDrawer.Initialize();

            return handsDrawer;
        }
    }
}
