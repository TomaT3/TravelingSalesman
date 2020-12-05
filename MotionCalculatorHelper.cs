using System;
using UnitsNet;

namespace TravelingSalesman
{
    public static class MotionCalculatorHelper
    {
        public static Duration GetTimeForDistance(Length distance, Acceleration acceleration, Speed maxSpeed)
        {
            Duration totalTime;
            distance = Length.FromMillimeters(Math.Abs(distance.Millimeters));
            var isMaxSpeedReachable = IsMaxSpeedReached(distance, acceleration, maxSpeed);
            if(isMaxSpeedReachable)
            {
                var timeToReachMaxSpeed = GetTimeToReachMaxSpeed(acceleration, maxSpeed);
                var distanceNeededToAccelAndDeccel = 2 * GetDistanceForConstAcceleration(acceleration, timeToReachMaxSpeed);
                var distanceTravelledWithMaxSpeed = distance - distanceNeededToAccelAndDeccel;
                var timeWithMaxSpeed = GetTimeForConstVelocity(maxSpeed, distanceTravelledWithMaxSpeed);
                totalTime = timeToReachMaxSpeed + timeWithMaxSpeed + timeToReachMaxSpeed;
            }
            else
            {
                var halfDistance = distance / 2;
                var timeForAcceleration = GetTimeForConstAcceleration(halfDistance, acceleration);
                totalTime = 2 * timeForAcceleration;
            }

            return totalTime;
        }

        private static bool IsMaxSpeedReached(Length distance, Acceleration acceleration, Speed maxSpeed)
        {
            var maxReachableSpeed = Math.Sqrt(distance.Millimeters / acceleration.MicrometersPerSecondSquared);
            return Speed.FromMillimetersPerSecond(maxReachableSpeed) > maxSpeed;
        }

        private static Duration GetTimeForConstAcceleration(Length distance, Acceleration acceleration)
        {
            var time = Math.Sqrt(2 * distance.Millimeters / acceleration.MillimetersPerSecondSquared);
            return Duration.FromSeconds(time);
        }

        private static Duration GetTimeToReachMaxSpeed(Acceleration acceleration, Speed maxSpeed)
        {
            var time = maxSpeed.MillimetersPerSecond / acceleration.MillimetersPerSecondSquared;
            return Duration.FromSeconds(time);
        }

        private static Length GetDistanceForConstAcceleration(Acceleration acceleration, Duration time)
        {
            var distance = 0.5 * acceleration.MillimetersPerSecondSquared * Math.Pow(time.Seconds, 2);
            return Length.FromMillimeters(distance);
        }

        private static Duration GetTimeForConstVelocity(Speed velocity, Length distance)
        {
            var time = distance / velocity;
            return time;
        }
    }
}
