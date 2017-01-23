using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseAccess;
using DialplanManager.Interfaces;

namespace DialplanManager.DialplanClasses
{
  public class DialplanFilters : IDialplanFilters
  {
    public IDialplan FilterBasedOnDate(IEnumerable<IDialplanDate> input)
    {
      var now = DateTime.Now;
      var rules = input.Where(rule =>
                              rule.StartDate.Date <= now.Date &&
                              rule.EndDate.Date >= now.Date
                              ).Select(rule => rule.Dialplan).FirstOrDefault();
      return rules;
    }
    public IDialplan FilterBasedOnRange(IEnumerable<IDialplanRange> input)
    {
      var now = DateTime.Now;
      var listOfRange = input.OrderBy(r => r.Priority);
      var planRange = listOfRange.FirstOrDefault(r => SetDaysOfWeek(r.DaysOfWeek).Contains(now.DayOfWeek) && IsTimeInRange(r.TimeRange, now));
      return planRange == null ? null : planRange.Dialplan;
    }

    private static IEnumerable<DayOfWeek> SetDaysOfWeek(string daysOfWeek)
    {
      var daysOfWeekList = daysOfWeek.Split(',');
      return daysOfWeekList.Select(ConvertDayOfWeek).ToList();
    }

    private static DayOfWeek ConvertDayOfWeek(string dayOfWeek)
    {
      switch (dayOfWeek.Trim())
      {
        case "mon":
          return DayOfWeek.Monday;
        case "tue":
          return DayOfWeek.Tuesday;
        case "wed":
          return DayOfWeek.Wednesday;
        case "thur":
          return DayOfWeek.Thursday;
        case "fri":
          return DayOfWeek.Friday;
        case "sat":
          return DayOfWeek.Saturday;
        case "sun":
          return DayOfWeek.Sunday;
        default:
          throw new Exception("Day" + dayOfWeek + "not recognised");
      }
    }

    private static bool IsTimeInRange(string timeRange, DateTime now)
    {
      var timeList = timeRange.Split('-').Select(ConvertToTimeSpan).ToList();
      return (timeList.Count == 2) && ((now > CreateDateTime(now, timeList[0]) && now < SetEndDate(now, timeList)));
    }

    private static TimeSpan ConvertToTimeSpan(string time)
    {
      return TimeSpan.Parse(string.Format("{0}:00", time));
    }

    private static DateTime SetEndDate(DateTime now, IList<TimeSpan> timeList)
    {
      return (timeList[0] > timeList[1]) ? CreateDateTime(now.AddDays(1), timeList[1]) : CreateDateTime(now, timeList[1]);
    }

    private static DateTime CreateDateTime(DateTime now, TimeSpan time)
    {
      return new DateTime(now.Year, now.Month, now.Day, time.Hours, time.Minutes, time.Seconds);
    }
  }
}