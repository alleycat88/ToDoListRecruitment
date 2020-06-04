using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using ToDoListRecruitment.Models;

namespace ToDoListRecruitment.Utility
{
    public class ListExistAttribute : ValidationAttribute
    {
        private readonly List.Query _listQ;
        public ListExistAttribute()
        {
            this._listQ = new List.Query(new ApiDb());
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || Convert.ToInt64(value) == 0) return ValidationResult.Success;

            // get the id to match email and id owner
            var listExist = _listQ.getByIdSync(Convert.ToInt64(value));

            if (listExist != null) return ValidationResult.Success;
            else return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }

    public class ColorHexAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString())) return ValidationResult.Success;

            var realval = value.ToString();
            Regex regex = new Regex(@"^#(?:[0-9a-fA-F]{3}){1,2}$");
            Match match = regex.Match(realval);

            if(string.IsNullOrEmpty(realval)) return ValidationResult.Success;
            else if (match.Success) return ValidationResult.Success;
            else return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }

    public class StringLengthNullableAttribute : ValidationAttribute
    {
        private readonly int min;
        private readonly int max;

        public StringLengthNullableAttribute(int MinimumLength = 3, int MaximumLength = 400)
        {
            this.min = MinimumLength;
            this.max = MaximumLength;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString())) return ValidationResult.Success;

            var realval = value.ToString();

            if (string.IsNullOrEmpty(realval)) return ValidationResult.Success;
            else if (realval.Length >= min && realval.Length <= max) return ValidationResult.Success;
            else return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}
