using System;

using UnityEngine;

namespace Extensions
{
    public class TimerNew
    {
        public float ResponseTime;
        public float RemainingTime;

        public bool AutoReset;
        public bool Enabled;

        public Action OnTimeUp;

        public TimerNew(float seconds = 0)
        {
            ResponseTime = seconds;
            RemainingTime = ResponseTime;
            AutoReset = false;
            Enabled = seconds > 0 ? true : false;
        }

        public bool Update()
        {
            if (Enabled is false)
                return false;

            RemainingTime -= Time.deltaTime;
            if (RemainingTime <= 0)
            {
                RemainingTime = 0;
                OnTimeUp?.Invoke();

                if (AutoReset)
                    Reset();
                else
                    Enabled = false;
            }

            return true;
        }

        public void Reset()
        {
            RemainingTime = ResponseTime;
            Enabled = true;
        }
    }
}