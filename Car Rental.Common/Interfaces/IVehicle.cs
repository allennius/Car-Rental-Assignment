using Car_Rental.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Common.Interfaces;
public interface IVehicle : IEntity
{
    public VehicleTypes VehicleType { get; }
    public VehicleStatuses Status { get; }
    public string Make { get; }
    public string RegNo { get; }
    public int Odometer { get; }
    public double CostKM { get; }
    public double CostDay { get; }

    void ReturnVehicle(int odometer);
    void Book();
}
