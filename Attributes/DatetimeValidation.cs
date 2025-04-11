using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace LiveChartPlay.Attributes
{
    public class DatetimeValidation : ValidationAttribute
    {
        private readonly string _format;
        private readonly CultureInfo _culture;

        public DatetimeValidation(string format = "yyyy-mm-dd", string culture = "en-US") {
            _format = format;
            _culture = new CultureInfo(culture);
        }

        public override bool IsValid(object value)
        {

            if (value == null || string.IsNullOrWhiteSpace(value.ToString())) 
                {
                    return false;
                }

                return DateTime.TryParseExact(value.ToString(), _format, _culture, DateTimeStyles.None, out _);
            }
        
    }
}
