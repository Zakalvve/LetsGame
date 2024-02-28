using System.ComponentModel.DataAnnotations;

namespace LetsGame.Common.Data.DataAnnotations
{
    internal class FutureDateAttribute : ValidationAttribute
    {
        public FutureDateAttribute() { }

        public override bool IsValid(object? value)
        {
            if (value == null) return true;
            DateTime input = (DateTime)value;
            if (input >= (DateTime.Now.Date))
            {
                return true;
            }
            return false;
        }
    }
}
