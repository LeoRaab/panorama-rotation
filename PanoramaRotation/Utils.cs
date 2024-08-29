using System.Globalization;
using System.Numerics;

namespace PanoramaRotation;

public static class Utils
{
    public static double? CalculateYawToNorth(string orientationX, string orientationY, string orientationZ, string orientationW)
    {
        if (!float.TryParse(orientationX, CultureInfo.InvariantCulture.NumberFormat, out var x) ||
            !float.TryParse(orientationY, CultureInfo.InvariantCulture.NumberFormat, out var y) ||
            !float.TryParse(orientationZ, CultureInfo.InvariantCulture.NumberFormat, out var z) ||
            !float.TryParse(orientationW, CultureInfo.InvariantCulture.NumberFormat, out var w)) return null;
            
        var quaternion = new Quaternion(x, y, z, w);
        var axis = ToEuler(quaternion);
        var degrees = RadiantToDegree(axis[2]) - 90;
        var resultAngle = NormalizeDegrees(degrees);
        var degreesToNorth = degrees > 0 ? 0 - resultAngle : 0 + resultAngle;
            
        return IsDegreesDeltaSignificant(degrees, degreesToNorth) ? degreesToNorth : degrees;
    }

    private static Double[] ToEuler(Quaternion q) {
        var wx = q.W * q.X;
        var wy = q.W * q.Y;
        var wz = q.W * q.Z;
        var xx = q.X * q.X;
        var xy = q.X * q.Y;
        var xz = q.X * q.Z;
        var yy = q.Y * q.Y;
        var yz = q.Y * q.Z;
        var zz = q.Z * q.Z;

        double Asin(float t) {
            return t >= 1 ? Math.PI / 2 : (t <= -1 ? -Math.PI / 2 : Math.Asin(t));
        }

        var x = -Math.Atan2(2 * (yz - wx), 1 - 2 * (xx + yy));
        var y = Asin(2 * (xz + wy));
        var z = -Math.Atan2(2 * (xy - wz), 1 - 2 * (yy + zz));

        return [x, y, z];
    }

    private static double RadiantToDegree(double angle) {
        return angle * (180 / Math.PI);
    }

    private static double NormalizeDegrees(double degrees) {
        var result = degrees;
        while (result >= 360) {
            result -= 360;
        }
        while (result < 0) {
            result += 360;
        }
        return result;
    }

    private static bool IsDegreesDeltaSignificant(double inputDegrees, double calculatedDegrees) {
        double[] absoluteDegrees = [Math.Abs(inputDegrees), Math.Abs(calculatedDegrees)];
        Array.Sort(absoluteDegrees);

        return absoluteDegrees[1] - absoluteDegrees[0] > 1;
    }
}