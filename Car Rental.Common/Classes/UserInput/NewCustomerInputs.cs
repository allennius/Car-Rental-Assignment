using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Common.Classes.UserInput;
public class NewCustomerInputs
{
    public string errorMessage = string.Empty;
    public int? socialSecurityNumber = default;
    public string firstName = string.Empty;
    public string lastName = string.Empty;
    
    public bool isInputs()
    {
        errorMessage = string.Empty;
        if (socialSecurityNumber.Equals(null)) errorMessage += " SSN,";
        if (firstName.Equals(string.Empty)) errorMessage += " Firstname,";
        if (lastName.Equals(string.Empty)) errorMessage += " Lastname,";

        if (errorMessage.Equals(string.Empty)) return true;

        errorMessage = $"Check Input: {errorMessage.Remove(errorMessage.Length - 1)}";
        return false;
    }

}

