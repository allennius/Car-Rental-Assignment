using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Common.Classes;
public class Car : IVehicle, IEntity
{
    public int Id { get; init; }
    public VehicleTypes VehicleType { get; init; }
    public VehicleStatuses Status { get; private set; }
    public string Make { get; init; }
    public string RegNo { get; init; }
    public int Odometer { get; private set; }
    public double CostKM { get; init; }
    public double CostDay { get; init; }


    public Car(int id, VehicleTypes vehicleType, double costKM, double costDay, string make, string regNo, int odometer, VehicleStatuses status = VehicleStatuses.Available) =>
        (Id, VehicleType, CostKM, CostDay, Make, RegNo, Odometer, Status) = (id, vehicleType, costKM, costDay, make, regNo, odometer, status);

    public void ReturnVehicle(int odometer) => (Odometer, Status) = (odometer, VehicleStatuses.Available);

    public void Book() => Status = VehicleStatuses.Booked;

}
