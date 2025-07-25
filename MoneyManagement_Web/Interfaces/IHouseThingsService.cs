using MoneyManagement_Web.Data.HouseThings;

namespace MoneyManagement_Web.Interfaces;

public interface IHouseThingsService
{
    Task<List<HouseThings>> GetActiveHouseThingsList();
    Task<List<HouseThings>> GetActiveHouseThingsListByRoom(int id);
    Task<List<HouseThings>> GetHistoryHouseThingsList(int historyId);
    Task<HouseThings> GetHouseThings(int houseThingsId);
    Task<HouseThings> AddHouseThings(HouseThings houseThings);
    Task<HouseThings> RenewHouseThings(HouseThings houseThings);
    Task<HouseThings> UpdateHouseThings(HouseThings houseThings);
    Task<HouseThings> DeleteHouseThings(HouseThings houseThings);

    Task<List<HouseThingsRooms>> GetActiveHouseThingsRoomsList();
    Task<HouseThingsRooms> GetHouseThingsRooms(int houseThingsRoomsId);
    Task<HouseThingsRooms> AddHouseThingsRooms(HouseThingsRooms houseThingsRooms);
    Task<HouseThingsRooms> UpdateHouseThingsRooms(HouseThingsRooms houseThingsRooms);
    Task<HouseThingsRooms> DeleteHouseThingsRooms(HouseThingsRooms houseThingsRooms);
}