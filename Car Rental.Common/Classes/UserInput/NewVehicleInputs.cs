using Car_Rental.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Common.Classes.UserInput;
public class NewVehicleInputs : Vehicle
{
    //public string make = string.Empty;
    //public int odometer = default;
    //public double costKm;
    //public double costDay;
    //public string regNo = string.Empty;
    //public VehicleStatuses status;
    //public VehicleTypes type;

    public string errorMessage { get; set; } = string.Empty;

    public NewVehicleInputs(int id, VehicleTypes vehicleType, double costKM, double costDay, string make, string regNo, int odometer, VehicleStatuses status = VehicleStatuses.Available) : base(id, vehicleType, costKM, costDay, make, regNo, odometer, status)
    {
        //Id = id;
        //VehicleType = vehicleType;
        //CostKM = costKM;
        //CostDay = costDay;
        //Make = make;
        //RegNo = regNo;  
        //Odometer = odometer;
        //Status = status;
    }

    public void SetVehicleType(VehicleTypes type) => VehicleType = type;

    public bool isInputs()
    {
        errorMessage = string.Empty;

        if (Make.Length == 0) errorMessage += "Make, ";
        if (Odometer < 0 || Odometer.Equals(default)) errorMessage += " Odometer";
        if (CostKM <= 0) errorMessage += " CostKm,";
        if (CostDay <= 0) errorMessage += " CostDay,";
        if (RegNo.Length < 6) errorMessage += " RegNo,";
        if (VehicleType == VehicleTypes.None) errorMessage += " VehicleType,";
        if (Status.Equals(VehicleStatuses.None)) { Status = VehicleStatuses.Available; } 

        if (!errorMessage.Equals(string.Empty))
        {
            errorMessage = $"Check Input: {errorMessage.Remove(errorMessage.Length - 1)}";
            return false;
        }

        return true;
    }
}
