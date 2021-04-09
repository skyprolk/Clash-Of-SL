/*
 * Program : Clash Of SL Server
 * Description : A C# Writted 'Clash of SL' Server Emulator !
 *
 * Authors:  Sky Tharusha <Founder at Sky Production>,
 *           And the Official DARK Developement Team
 *
 * Copyright (c) 2021  Sky Production
 * All Rights Reserved.
 */

using System;

namespace CSS.Logic
{
    internal class Timer
    {
        #region Public Constructors

        public Timer()
        {
            m_vStartTime = new DateTime(1970, 1, 1);
            m_vSeconds = 0;
        }

        #endregion Public Constructors

        #region Private Fields

        int m_vSeconds;
        DateTime m_vStartTime;

        #endregion Private Fields

        #region Public Methods

        public void FastForward(int seconds)
        {
            m_vSeconds -= seconds;
        }

        public int GetRemainingSeconds(DateTime time, bool boost = false, DateTime boostEndTime = default(DateTime),
            float multiplier = 0f)
        {
            var result = int.MaxValue;
            if (!boost)
                result = m_vSeconds - (int) time.Subtract(m_vStartTime).TotalSeconds;
            else
            {
                if (boostEndTime >= time)
                    result = m_vSeconds - (int) (time.Subtract(m_vStartTime).TotalSeconds * multiplier);
                else
                {
                    var boostedTime = (float) time.Subtract(m_vStartTime).TotalSeconds -
                                      (float) (time - boostEndTime).TotalSeconds;
                    var notBoostedTime = (float) time.Subtract(m_vStartTime).TotalSeconds - boostedTime;

                    result = m_vSeconds - (int) (boostedTime * multiplier + notBoostedTime);
                }
            }
            if (result <= 0)
                result = 0;
            return result;
        }

        public int GetRemainingSeconds(DateTime time)
        {
            var result = m_vSeconds - (int) time.Subtract(m_vStartTime).TotalSeconds;
            if (result <= 0)
                result = 0;
            return result;
        }

        public DateTime GetStartTime() => m_vStartTime;

        public void StartTimer(int seconds, DateTime time)
        {
            m_vStartTime = time;
            m_vSeconds = seconds;
        }

        #endregion Public Methods
    }
}
