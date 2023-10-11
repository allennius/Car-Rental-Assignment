using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Common.Classes.UserInput;
public class NewCustomerInputs : Customer
{
    public string errorMessage = string.Empty;

    public NewCustomerInputs(string firstName, string lastName, int? ssn = null, int id = default) : base(firstName, lastName, ssn, id)
    {
        //FirstName = firstName;
        //LastName = lastName;
        //SSN = ssn;
        //errorMessage = string.Empty;
    }

    public bool isInputs()
    {
        errorMessage = string.Empty;
        if (SSN.Equals(0)) errorMessage += " SSN,";
        if (FirstName.Equals(string.Empty)) errorMessage += " Firstname,";
        if (LastName.Equals(string.Empty)) errorMessage += " Lastname,";

        if (errorMessage.Equals(string.Empty)) return true;

        errorMessage = $"Check Input: {errorMessage.Remove(errorMessage.Length - 1)}";
        return false;
    }

}

