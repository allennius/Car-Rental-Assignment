using Car_Rental.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Common.Classes.UserInput;
public class NewVehicleInputs
{
    public string make = string.Empty;
    public int odometer = default;
    public double costKm;
    public double costDay;
    public string regNo = string.Empty;
    public VehicleStatuses status;
    public VehicleTypes type;

    public string errorMessage = string.Empty;

    public void SetVehicleType(VehicleTypes type) => this.type = type;

    public bool isInputs()
    {
        errorMessage = string.Empty;

        if (make.Length == 0) errorMessage += "Make, ";
        if (odometer < 0 || odometer.Equals(default)) errorMessage += " Odometer";
        if (costKm <= 0) errorMessage += " CostKm,";
        if (costDay <= 0) errorMessage += " CostDay,";
        if (regNo.Length < 6) errorMessage += " RegNo,";
        if (type == VehicleTypes.None) errorMessage += " VehicleType,";
        if (status.Equals(VehicleStatuses.None)) { status = VehicleStatuses.Available; } 

        if (!errorMessage.Equals(string.Empty))
        {
            errorMessage = $"Check Input: {errorMessage.Remove(errorMessage.Length - 1)}";
            return false;
        }

        return true;
    }
}
