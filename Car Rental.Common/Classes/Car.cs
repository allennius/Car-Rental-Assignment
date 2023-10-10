using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Common.Classes;
public class Car : Vehicle, IEntity
{
    public Car(int id, VehicleTypes vehicleType, double costKM, double costDay, string make, string regNo, int odometer, VehicleStatuses status = VehicleStatuses.Available) 
        : base(id, vehicleType, costKM, costDay, make, regNo, odometer, status)
    {
    }
}
