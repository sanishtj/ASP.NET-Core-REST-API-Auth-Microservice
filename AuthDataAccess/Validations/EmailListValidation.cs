using System;
using System.ComponentModel.DataAnnotations;


namespace AuthDataAccess.Validations
{
    public sealed class EmailListAttribute : DataTypeAttribute
    {

        public EmailListAttribute() : base(DataType.Text)
        {
            ErrorMessage = "One of the emails is not in right format";
        }


        public override bool IsValid(object value)
        {
            string[] splitters = new string[] { ",", ";" };
            string inputVal = Convert.ToString(value);
            string[] emails = inputVal.Split(splitters, StringSplitOptions.None);
            foreach (var email in emails)
            {
                EmailAddressAttribute sf = new EmailAddressAttribute();
                if (!sf.IsValid(email))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
