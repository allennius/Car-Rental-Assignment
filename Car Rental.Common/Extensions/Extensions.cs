using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Common.Extensions;
public static class Extensions
{
    public static string ToDollar(this double value) => value.ToString("C", new CultureInfo("en-US"));

    public static string ToKm(this int value) => string.Format(new CultureInfo("en-US"), $"{value:N0} km");

    public static int Duration(this DateOnly startDate, DateOnly endDate) =>
        endDate.DayNumber - startDate.DayNumber;
}