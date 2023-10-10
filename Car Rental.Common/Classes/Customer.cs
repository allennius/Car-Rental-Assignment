﻿using Car_Rental.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Common.Classes;
public class Customer : IPerson
{
    public int Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Name => $"{FirstName} {LastName}";
    public int SSN { get; init; }

    public Customer(int id, string firstName, string lastName, int ssn) =>
        (Id, FirstName, LastName, SSN) = (id, firstName, lastName, ssn);
}
