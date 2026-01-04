using System;

using UnityEngine;

namespace Extensions
{
    public class Timer
    {
        private float _targetTime;
        public float TargetTime => _targetTime;
        private float _responseTime;
        public float ResponseTime
        {
            get => _responseTime;
            set => SetResponseTime(value);
        }

        public float RemainingTime
        {
            get => GetRemainingTime();
            set => SetTargetTime(value + Time.time);
        }

        public Action SingleEvent;
        public bool IsSingleEventExpected;

        public Timer(float seconds)
        {
            _targetTime = 0;
            SetResponseTime(seconds);
        }

        private void SetResponseTime(float seconds)
        {
            _responseTime = seconds;
        }

        public void SetTargetTime(float time)
        {
            _targetTime = time;
        }

        public void Reset()
        {
            _targetTime = Time.time + _responseTime;
            IsSingleEventExpected = true;
        }

        public bool CheckIsTimeUp()
        {
            if (Time.time >= _targetTime)
            {
                if (IsSingleEventExpected)
                    ExecuteSingleEvent();

                return true;
            }
            else
                return false;
        }

        private void ExecuteSingleEvent()
        {
            SingleEvent?.Invoke();
            IsSingleEventExpected = false;
        }

        private float GetRemainingTime()
        {
            float remainingTime = _targetTime - Time.time;
            if (remainingTime <= 0)
                return 0;
            return remainingTime;
        }
    }
}