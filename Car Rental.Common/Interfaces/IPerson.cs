using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Common.Interfaces;
public interface IPerson : IEntity
{
    public string FirstName { get; init; }
    public string LastName { get; init; }

    public string Name => $"{FirstName} {LastName}";
    public int SSN { get; init; }
}
