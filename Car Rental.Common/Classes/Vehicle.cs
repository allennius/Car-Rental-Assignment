using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Common.Classes;
public class Vehicle : IVehicle
{
    public int Id { get; init; }
    public VehicleTypes VehicleType { get; set; }
    public VehicleStatuses Status { get; set; }
    public string Make { get; set; }
    public string RegNo { get; set; }
    public int Odometer { get; set; }
    public double CostKM { get; set; }
    public double CostDay { get; set; }

    public Vehicle(int id, VehicleTypes vehicleType, double costKM, double costDay, string make, string regNo, int odometer, VehicleStatuses status = VehicleStatuses.Available) =>
        (Id, VehicleType, CostKM, CostDay, Make, RegNo, Odometer, Status) = (id, vehicleType, costKM, costDay, make, regNo, odometer, status);


    public void ReturnVehicle(int odometer) => (Odometer, Status) = (odometer, VehicleStatuses.Available);

    public void Book() => Status = VehicleStatuses.Booked;
}
