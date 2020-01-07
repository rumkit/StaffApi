using StaffApi.Models;

namespace StaffApi.DTO
{
    public class PositionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Grade { get; set; }

        public PositionDTO()
        {
        }

        public PositionDTO(Position position)
        {
            Id = position.Id;
            Name = position.Name;
            Grade = position.Grade;
        }
    }
}