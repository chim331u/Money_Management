using MoneyManagement_Api.Contract;
using MoneyManagement_Api.Models.HouseThings;

namespace MoneyManagement_Api.Interfaces;

public interface IHouseThingsService
{
    Task<ApiResponse<ICollection<HouseThings>>> GetActiveHouseThingsList();
    Task<ApiResponse<ICollection<HouseThings>>> GetActiveHouseThingsListByRoom(int id);
    Task<ApiResponse<ICollection<HouseThings>>> GetHistoryHouseThingsList(int historyId);
    Task<ApiResponse<HouseThings>> GetHouseThings(int houseThingsId);
    Task<ApiResponse<HouseThings>> AddHouseThings(HouseThings houseThings);
    Task<ApiResponse<HouseThings>> RenewHouseThings(HouseThings houseThings);
    Task<ApiResponse<HouseThings>> UpdateHouseThings(HouseThings houseThings);
    Task<ApiResponse<bool>> DeleteHouseThings(int id);

    Task<ApiResponse<ICollection<HouseThingsRooms>>> GetActiveHouseThingsRoomsList();
    Task<ApiResponse<HouseThingsRooms>> GetHouseThingsRooms(int houseThingsRoomsId);
    Task<ApiResponse<HouseThingsRooms>> AddHouseThingsRooms(HouseThingsRooms houseThingsRooms);
    Task<ApiResponse<HouseThingsRooms>> UpdateHouseThingsRooms(HouseThingsRooms houseThingsRooms);
    Task<ApiResponse<bool>> DeleteHouseThingsRooms(int id);
}