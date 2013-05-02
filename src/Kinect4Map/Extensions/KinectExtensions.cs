using Microsoft.Kinect;
using NUI4Map.Structs;

namespace Kinect4Map.Extensions
{
    static class KinectExtensions
    {
        public static Vector3D ScaleTo(this Vector3D skeletonPoint, double width, double height)
        {
            return ScaleTo(skeletonPoint, width, height, 1.0f, 1.0f);
        }


        public static Vector3D ScaleTo(this Vector3D skeletonPoint, double width, double height, double skeletonMaxX, double skeletonMaxY)
        {
            var position = new Vector3D
                                {
                                    X = Scale(width, skeletonMaxX, skeletonPoint.X),
                                    Y = Scale(height, skeletonMaxY, -skeletonPoint.Y),
                                    Z = skeletonPoint.Z
                                };
            return position;
        }

        /// <summary>
        /// Returns the scaled value of the specified position.
        /// </summary>
        /// <param name="maxPixel">Width or height.</param>
        /// <param name="maxSkeleton">Border (X or Y).</param>
        /// <param name="position">Original position (X or Y).</param>
        /// <returns>The scaled value of the specified position.</returns>
        private static float Scale(double maxPixel, double maxSkeleton, double position)
        {
            var value = ((((maxPixel / maxSkeleton) / 2) * position) + (maxPixel / 2));

            if (value > maxPixel)
            {
                return (float) maxPixel;
            }

            if (value < 0)
            {
                return 0;
            }

            return (float) value;
        }

    }
}
