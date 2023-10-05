using Car_Rental.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Common.Interfaces;
public interface IVehicle
{
    public VehicleTypes VehicleType { get; init; }
    public VehicleStatuses Status { get; }
    public string Make { get; init; }
    public string RegNo { get; init; }
    public int Odometer { get; }
    public double CostKM { get; init; }
    public double CostDay { get; init; }

    void ReturnVehicle(int odometer);
    void Book();
}
