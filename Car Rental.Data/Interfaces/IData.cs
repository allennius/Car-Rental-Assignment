using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using System.Linq.Expressions;

namespace Car_Rental.Data.Interfaces;
public interface IData
{
    List<T> Get<T>(Expression<Func<T, bool>>? expression = null) where T : class;
    T? GetSingle<T>(Expression<Func<T, bool>>? expression) where T : class, IEntity;
    public void Add<T>(T item) where T : class, IEntity;

    int NextVehicleId { get; }
    int NextPersonId { get; }
    int NextBookingId { get; }

    IBooking RentVehicle(int vehicleId, int customerId);
    IBooking ReturnVehicle(int vehicleId);

    public string[] VehicleStatusNames => Enum.GetNames(typeof(VehicleStatuses));
    public string[] VehicleTypeNames => Enum.GetNames(typeof(VehicleTypes));
    public VehicleTypes GetVehicleType(string name) => (VehicleTypes)Enum.Parse(typeof(VehicleTypes), name);
}
