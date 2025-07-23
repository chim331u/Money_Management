using Microsoft.EntityFrameworkCore;
using MoneyManagement_Api.AppContext;
using MoneyManagement_Api.Contract;
using MoneyManagement_Api.Interfaces;
using MoneyManagement_Api.Models.HouseThings;

namespace MoneyManagement_Api.Services;

public class HouseThingsService : IHouseThingsService
{
    private readonly ApplicationContext _context;
    private readonly ILogger<HouseThingsService> _logger;

    public HouseThingsService(ApplicationContext context, ILogger<HouseThingsService> logger)
    {
        _context = context;
        _logger = logger;
    }

    #region House Things

    public async Task<ApiResponse<ICollection<HouseThings>>> GetActiveHouseThingsList()
    {
        try
        {
            var result = await _context.HouseThings
                .Include(i => i.Room).Where(x => x.IsActive).OrderByDescending(x => x.PurchaseDate).ToListAsync();
            return new ApiResponse<ICollection<HouseThings>>(result, $"HouseThings list retrieved successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving active HouseThings list: {ex.Message}");
            return new ApiResponse<ICollection<HouseThings>>(null,
                $"Error retrieving active HouseThings list: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ICollection<HouseThings>>> GetActiveHouseThingsListByRoom(int id)
    {
        try
        {
            var result = await _context.HouseThings
                .Include(x => x.Room)
                .Where(x => x.IsActive && x.Room.Id == id).OrderByDescending(x => x.PurchaseDate).ToListAsync();
            return new ApiResponse<ICollection<HouseThings>>(result,
                $"HouseThings list by room ID {id} retrieved successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving active HouseThings list by room: {ex.Message}");
            return new ApiResponse<ICollection<HouseThings>>(null,
                $"Error retrieving active HouseThings list by room: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ICollection<HouseThings>>> GetHistoryHouseThingsList(int historyId)
    {
        try
        {
            var result = await _context.HouseThings.Include(x => x.Room)
                .Where(x => x.IsActive == false && x.HistoryId == historyId)
                .OrderByDescending(x => x.PurchaseDate).ToListAsync();
            return new ApiResponse<ICollection<HouseThings>>(result,
                $"History HouseThings list retrieved successfully for history ID {historyId}.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving history HouseThings list: {ex.Message}");
            return new ApiResponse<ICollection<HouseThings>>(null,
                $"Error retrieving history HouseThings list: {ex.Message}");
        }
    }

    public async Task<ApiResponse<HouseThings>> GetHouseThings(int houseThingsId)
    {
        try
        {
            var result = await _context.HouseThings.FindAsync(houseThingsId);
            return new ApiResponse<HouseThings>(result, $"HouseThings with ID {houseThingsId} retrieved successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving HouseThings with ID {houseThingsId}: {ex.Message}");
            return new ApiResponse<HouseThings>(null,
                $"Error retrieving HouseThings with ID {houseThingsId}: {ex.Message}");
        }
    }

    public async Task<ApiResponse<HouseThings>> UpdateHouseThings(HouseThings item)
    {
        try
        {
            var existingItem = await _context.HouseThings.Include(x => x.Room).Where(x => x.Id == item.Id)
                .FirstOrDefaultAsync();

            if (existingItem == null)
            {
                _logger.LogWarning($"Unable to find HouseThings with ID {item.Id} for update.");
                return new ApiResponse<HouseThings>(null, $"HouseThings with ID {item.Id} not found for update.");
            }

            existingItem.Cost = item.Cost;
            existingItem.Description = item.Description;
            existingItem.IsActive = item.IsActive;
            existingItem.LastUpdatedDate = DateTime.Now;
            existingItem.Name = item.Name;
            existingItem.PurchaseDate = item.PurchaseDate;
            existingItem.HistoryId = item.HistoryId;
            existingItem.ItemType = item.ItemType;
            existingItem.Model = item.Model;
            existingItem.Note = item.Note;

            _context.HouseThings.Update(existingItem);

            await _context.SaveChangesAsync();

            return new ApiResponse<HouseThings>(existingItem, $"HouseThings with ID {item.Id} updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating HouseThings with ID {item.Id}: {ex.Message}");
            return new ApiResponse<HouseThings>(null, $"Error updating HouseThings with ID {item.Id}: {ex.Message}");
        }
    }

    public async Task<ApiResponse<HouseThings>> AddHouseThings(HouseThings item)
    {
        try
        {
            var room = await _context.houseThingsRooms.FindAsync(item.Room.Id);

            item.CreatedDate = DateTime.Now;
            item.IsActive = true;
            item.Room = room;

            await _context.HouseThings.AddAsync(item);
            await _context.SaveChangesAsync();

            return new ApiResponse<HouseThings>(await _context.HouseThings.FindAsync(item.Id),
                $"HouseThings added successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding HouseThings: {ex.Message}");
            return new ApiResponse<HouseThings>(null, $"Error adding HouseThings: {ex.Message}");
        }
    }

    public async Task<ApiResponse<HouseThings>> RenewHouseThings(HouseThings item)
    {
        //item = new HouseThings;

        try
        {
            var oldItem = await _context.HouseThings.Include(x => x.Room).Where(x => x.Id == item.Id)
                .FirstOrDefaultAsync();

            if (oldItem == null)
            {
                _logger.LogWarning($"Unable to find HouseThings with ID {item.Id} to renew.");
                return new ApiResponse<HouseThings>(null, $"HouseThings with ID {item.Id} not found for renewal.");
            }

            await DeleteHouseThings(oldItem.Id);

            item.Id = 0;
            var renewedItem = await AddHouseThings(item);

            return new ApiResponse<HouseThings>(renewedItem.Data,
                $"HouseThings with ID {item.Id} renewed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error renewing HouseThings with ID {item.Id}: {ex.Message}");
            return new ApiResponse<HouseThings>(null, $"Error renewing HouseThings with ID {item.Id}: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeleteHouseThings(int id)
    {
        try
        {
            var existingItem = await _context.HouseThings.Include(x => x.Room).Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            if (existingItem == null)
            {
                _logger.LogWarning($"Unable to find HouseThings with ID {id} for deletion.");
                return new ApiResponse<bool>(false, $"HouseThings with ID {id} not found for deletion.");
            }

            existingItem.LastUpdatedDate = DateTime.Now;
            existingItem.IsActive = false;

            _context.HouseThings.Update(existingItem);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>(true, $"HouseThings with ID {id} deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting HouseThings with ID {id}: {ex.Message}");
            return new ApiResponse<bool>(false, $"Error deleting HouseThings with ID {id}: {ex.Message}");
        }
    }

    #endregion

    #region House Things Rooms

    public async Task<ApiResponse<ICollection<HouseThingsRooms>>> GetActiveHouseThingsRoomsList()
    {
        try
        {
            var result = await _context.houseThingsRooms.Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedDate).ToListAsync();
            return new ApiResponse<ICollection<HouseThingsRooms>>(result,
                "Active HouseThingsRooms list retrieved successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving active HouseThingsRooms list: {ex.Message}");
            return new ApiResponse<ICollection<HouseThingsRooms>>(null,
                $"Error retrieving active HouseThingsRooms list: {ex.Message}");
        }
    }

    public async Task<ApiResponse<HouseThingsRooms>> GetHouseThingsRooms(int houseThingsRoomId)
    {
        try
        {
            var result = await _context.houseThingsRooms.FindAsync(houseThingsRoomId);
            return new ApiResponse<HouseThingsRooms>(result,
                $"HouseThingsRooms with ID {houseThingsRoomId} retrieved successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving HouseThingsRooms with ID {houseThingsRoomId}: {ex.Message}");
            return new ApiResponse<HouseThingsRooms>(null,
                $"Error retrieving HouseThingsRooms with ID {houseThingsRoomId}: {ex.Message}");
        }
    }

    public async Task<ApiResponse<HouseThingsRooms>> UpdateHouseThingsRooms(HouseThingsRooms item)
    {
        try
        {
            var existingItem = await _context.houseThingsRooms.FindAsync(item.Id);

            if (existingItem == null)
            {
                _logger.LogWarning($"Unable to find HouseThingsRooms with ID {item.Id} for update.");
                return new ApiResponse<HouseThingsRooms>(null,
                    $"HouseThingsRooms with ID {item.Id} not found for update.");
            }

            existingItem.Color = item.Color;
            existingItem.Description = item.Description;
            existingItem.IsActive = item.IsActive;
            existingItem.Name = item.Name;
            existingItem.Icon = item.Icon;
            existingItem.Note = item.Note;
            existingItem.LastUpdatedDate = DateTime.Now;

            _context.houseThingsRooms.Update(existingItem);
            await _context.SaveChangesAsync();

            return new ApiResponse<HouseThingsRooms>(existingItem,
                $"HouseThingsRooms with ID {item.Id} updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating HouseThingsRooms with ID {item.Id}: {ex.Message}");
            return new ApiResponse<HouseThingsRooms>(null,
                $"Error updating HouseThingsRooms with ID {item.Id}: {ex.Message}");
        }
    }

    public async Task<ApiResponse<HouseThingsRooms>> AddHouseThingsRooms(HouseThingsRooms item)
    {
        try
        {
            item.CreatedDate = DateTime.Now;
            item.IsActive = true;

            await _context.houseThingsRooms.AddAsync(item);
            await _context.SaveChangesAsync();

            return new ApiResponse<HouseThingsRooms>(await _context.houseThingsRooms.FindAsync(item.Id),
                "HouseThingsRooms added successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding HouseThingsRooms: {ex.Message}");
            return new ApiResponse<HouseThingsRooms>(null, $"Error adding HouseThingsRooms: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeleteHouseThingsRooms(int id)
    {
        try
        {
            var existingItem = await _context.houseThingsRooms.FindAsync(id);
            if (existingItem == null)
            {
                _logger.LogWarning($"Unable to find HouseThingsRooms with ID {id} for deletion.");
                return new ApiResponse<bool>(false, $"HouseThingsRooms with ID {id} not found for deletion.");
            }

            existingItem.LastUpdatedDate = DateTime.Now;
            existingItem.IsActive = false;

            _context.houseThingsRooms.Update(existingItem);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>(true, $"HouseThingsRooms with ID {id} deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting HouseThingsRooms with ID {id}: {ex.Message}");
            return new ApiResponse<bool>(false, $"Error deleting HouseThingsRooms with ID {id}: {ex.Message}");
        }
    }

    #endregion
}