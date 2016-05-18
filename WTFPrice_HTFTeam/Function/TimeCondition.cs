using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WTFPrice_HTFTeam.Function
{
    public static class TimeCondition
    {
        public static bool TimeNew(DateTime createdDate,int duration)
        {
            if (createdDate.Hour >= (DateTime.Now.TimeOfDay.Hours-duration) && DateTime.Equals(createdDate.Date,DateTime.Now.Date))
            {
                return true;
            }
            return false;
        }

        public static string TimeString(DateTime createdDate)
        {
            var now = DateTime.Now;
            if (DateTime.Equals(createdDate.Date, now.Date))
            {
                return "Hôm nay - " + createdDate.ToString("HH:mm");
            }
            if (createdDate.Day.Equals(now.Day - 1))
            {
                return "Hôm qua - " + createdDate.ToString("HH:mm");
            }
            return createdDate.ToString("dd/MM/yyyy - HH:mm");
            
        }

        public static string ToStringFormat(DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy");
        }
    }
}