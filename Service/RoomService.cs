using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    using BusinessObject;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using Repository;

    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _repo;
        private readonly IBookingRepository _rebook;

        public RoomService(IRoomRepository repo, IBookingRepository rebook)
        {
            _repo = repo;
            _rebook = rebook;
        }

        // ================= GET ALL =================
        public async Task<List<RoomDTO>> GetAll()
        {
            var rooms = await _repo.GetAll();

            return rooms.Select(r => new RoomDTO
            {
                Id = r.Id,
                RoomNumber = r.RoomNumber,
                Capacity = r.Capacity,
                Price = r.Price,
                Status = r.Status,
                Description = r.Description,
                TypeId = r.RoomTypeId ?? 0,
                Type = r.RoomType?.Name,
                Images = r.RoomImages.Select(i => i.ImageUrl).ToList()
            }).ToList();
        }

        // ================= GET BY ID =================
        public async Task<RoomDTO?> GetById(int id)
        {
            var r = await _repo.GetById(id);
            if (r == null) return null;

            return new RoomDTO
            {
                Id = r.Id,
                RoomNumber = r.RoomNumber,
                Capacity = r.Capacity,
                Price = r.Price,
                Status = r.Status,
                Description = r.Description,
                TypeId = r.RoomTypeId ?? 0,
                Type = r.RoomType?.Name,
                Images = r.RoomImages.Select(i => i.ImageUrl).ToList()
            };
        }

        // ================= CREATE =================
        public async Task<ApiResponse<string>> Create(RoomCreateRequest req)
        {
            // 🔥 VALIDATE
            if (string.IsNullOrWhiteSpace(req.RoomNumber))
                return Fail("RoomNumber không được để trống");

            if (req.Price <= 0)
                return Fail("Giá phải > 0");

            var room = new Room
            {
                RoomNumber = req.RoomNumber,
                Capacity = req.Capacity,
                Price = req.Price,
                Status = req.Status,
                Description = req.Description,
                RoomTypeId = req.RoomTypeId,
                CreatedAt = DateTime.Now
            };

            // 🔥 Upload ảnh
            await SaveImages(room, req.Images);

            await _repo.Add(room);

            return Success("Tạo phòng thành công");
        }

        // ================= UPDATE =================
        public async Task<ApiResponse<string>> Update(int id, RoomCreateRequest req)
        {
            var room = await _repo.GetById(id);
            if (room == null)
                return Fail("Không tìm thấy phòng");

            room.RoomNumber = req.RoomNumber;
            room.Capacity = req.Capacity;
            room.Price = req.Price;
            room.Status = req.Status;
            room.Description = req.Description;
            room.RoomTypeId = req.RoomTypeId;

            // 🔥 thêm ảnh mới (không xóa ảnh cũ)
            await SaveImages(room, req.Images);

            await _repo.Update(room);

            return Success("Cập nhật thành công");
        }

        // ================= DELETE =================
public async Task<ApiResponse<string>> Delete(int id)
{
    var room = await _repo.GetById(id);
    if (room == null)
        return Fail("Không tìm thấy phòng");



            if (room.Bookings != null && room.Bookings.Count > 0)
            {
                return Fail("Room đang có booking, không thể xóa!");
            }



            if (room.RoomImages != null && room.RoomImages.Count > 0)
            {
                foreach (var img in room.RoomImages)
                {
                    var fullPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        img.ImageUrl.TrimStart('/')
                    );

                    if (File.Exists(fullPath))
                        File.Delete(fullPath);
                }
            }
            await _repo.Delete(id);

            return Success("Xóa phòng thành công");
}

        // ================= PRIVATE METHODS =================

        private async Task SaveImages(Room room, List<IFormFile>? files)
        {
            if (files == null || !files.Any()) return;

            var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            foreach (var file in files)
            {
                // 🔥 validate basic
                if (file.Length == 0) continue;

                var ext = Path.GetExtension(file.FileName).ToLower();

                if (ext != ".jpg" && ext != ".png" && ext != ".jpeg")
                    continue;

                var fileName = Guid.NewGuid() + ext;
                var path = Path.Combine(folder, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);

                room.RoomImages.Add(new RoomImage
                {
                    ImageUrl = "/images/" + fileName
                });
            }
        }

        private ApiResponse<string> Success(string msg)
        {
            return new ApiResponse<string>
            {
                Success = true,
                Message = msg
            };
        }

        private ApiResponse<string> Fail(string msg)
        {
            return new ApiResponse<string>
            {
                Success = false,
                Message = msg
            };
        }
    }
}
