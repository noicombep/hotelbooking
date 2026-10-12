using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject
{

    public class CreateBookingRequest
    {


        [Required]
        public int RoomId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckIn { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckOut { get; set; }

        // Validate business basic
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CheckIn >= CheckOut)
            {
                yield return new ValidationResult(
                    "CheckOut phải lớn hơn CheckIn",
                    new[] { nameof(CheckOut) });
            }
        }
    }
}
