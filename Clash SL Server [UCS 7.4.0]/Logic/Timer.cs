using System;

namespace CSS.Logic
{
    internal class Timer
    {
        internal int Seconds;
        internal DateTime StartTime;

        internal Timer()
        {
            this.StartTime = new DateTime(1970, 1, 1);
            this.Seconds = 0;
        }
        internal void FastForward(int seconds) => this.Seconds -= seconds;

        internal int GetRemainingSeconds(DateTime time, bool boost, DateTime boostEndTime = default(DateTime), float multiplier = 0f)
        {
            int result;
            if (!boost)
            {
                result = this.Seconds - (int)time.Subtract(this.StartTime).TotalSeconds;
            }
            else
            {
                if (boostEndTime >= time)
                    result = this.Seconds - (int)(time.Subtract(this.StartTime).TotalSeconds * multiplier);
                else
                {
                    var boostedTime = (float)time.Subtract(this.StartTime).TotalSeconds - (float)(time - boostEndTime).TotalSeconds;
                    var notBoostedTime = (float)time.Subtract(this.StartTime).TotalSeconds - boostedTime;

                    result = this.Seconds - (int)(boostedTime * multiplier + notBoostedTime);
                }
            }
            if (result <= 0)
                result = 0;
            return result;
        }
        internal int GetRemainingSeconds(DateTime time)
        {
            int result = this.Seconds - (int)time.Subtract(this.StartTime).TotalSeconds;
            if (result <= 0)
            {
                result = 0;
            }
            return result;
        }
        internal DateTime GetStartTime() => this.StartTime;

        internal void StartTimer(int seconds, DateTime time)
        {
            this.StartTime = time;
            this.Seconds = seconds;
        }
    }
}
