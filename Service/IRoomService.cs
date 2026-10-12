using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    using BusinessObject;

    public interface IRoomService
    {
        // ================= READ =================
        Task<List<RoomDTO>> GetAll();
        Task<RoomDTO?> GetById(int id);

        // ================= WRITE =================
        Task<ApiResponse<string>> Create(RoomCreateRequest req);
        Task<ApiResponse<string>> Update(int id, RoomCreateRequest req);
        Task<ApiResponse<string>> Delete(int id);
    }
}
