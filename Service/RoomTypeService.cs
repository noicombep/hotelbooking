using BusinessObject;
using Repository;
using Service;

public class RoomTypeService : IRoomTypeService
{
    private readonly IRoomTypeRepository _repo;

    public RoomTypeService(IRoomTypeRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<RoomTypeDTO>> GetAll()
    {
        var types = await _repo.GetAll();

        return types.Select(t => new RoomTypeDTO
        {
            Id = t.Id,
            Name = t.Name,
            RoomCount = t.Rooms.Count
        }).ToList();
    }

    public async Task<ApiResponse<string>> Create(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return new ApiResponse<string>
            {
                Success = false,
                Message = "Tên không được để trống"
            };
        }

        var roomType = new RoomType
        {
            Name = name
        };

        await _repo.Add(roomType);

        return new ApiResponse<string>
        {
            Success = true,
            Message = "Tạo thành công"
        };
    }

    public async Task<ApiResponse<string>> Update(int id, string name)
    {
        var type = await _repo.GetById(id);
        if (type == null)
        {
            return new ApiResponse<string>
            {
                Success = false,
                Message = "Không tìm thấy"
            };
        }

        type.Name = name;
        await _repo.Update(type);

        return new ApiResponse<string>
        {
            Success = true,
            Message = "Cập nhật thành công"
        };
    }

    public async Task<ApiResponse<string>> Delete(int id)
    {
        var type = await _repo.GetById(id);
        if (type == null)
        {
            return new ApiResponse<string>
            {
                Success = false,
                Message = "Không tìm thấy"
            };
        }

        if (type.Rooms.Any())
        {
            return new ApiResponse<string>
            {
                Success = false,
                Message = "Không thể xóa vì còn phòng"
            };
        }

        await _repo.Delete(id);

        return new ApiResponse<string>
        {
            Success = true,
            Message = "Xóa thành công"
        };
    }
}