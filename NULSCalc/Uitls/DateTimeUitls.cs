using System;

namespace NULSCalc.Uitls
{
    public static class DateTimeUitls
    {
        private static DateTime DateTime1970 = new DateTime(1970, 1, 1);

        private static long DateTime1970TimeStamp = 621355968000000000;

        /// <summary>
        /// 时间戳
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <param name="isMillisecond">是否为毫秒级</param>
        /// <returns>时间戳</returns>
        public static long GetTimeStamp(this DateTime dateTime, bool isMillisecond = false)
        {
            if (isMillisecond)
            {
                return (long)(dateTime.ToUniversalTime() - DateTime1970).TotalMilliseconds;
            }
            else
            {
                return (dateTime.ToUniversalTime().Ticks - DateTime1970TimeStamp) / 10000000;
            }
        }

        /// <summary>
        /// 时间戳
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <param name="isMillisecond">是否为毫秒级</param>
        /// <returns>时间戳</returns>
        public static long? GetTimeStamp(this DateTime? dateTime, bool isMillisecond = false)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.GetTimeStamp();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 时间戳获取时间
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="isMillisecond"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(long timestamp, bool isMillisecond = false)
        {
            if (isMillisecond)
            {
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(DateTime1970); // 当地时区
                return startTime.AddMilliseconds(timestamp);
            }
            else
            {
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(DateTime1970); // 当地时区
                return startTime.AddSeconds(timestamp);
            }
        }
    }
}
