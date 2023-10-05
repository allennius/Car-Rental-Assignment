using Car_Rental.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Common.Classes;
public class Customer : IPerson
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Name => $"{FirstName} {LastName}";
    public int SSN { get; init; }

    public Customer(string firstName, string lastName, int ssn) =>
        (FirstName, LastName, SSN) = (firstName, lastName, ssn);
}
