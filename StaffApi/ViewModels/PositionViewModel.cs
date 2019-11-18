using StaffApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApi.ViewModels
{
    public class PositionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Grade { get; set; }

        public PositionViewModel()
        {

        }

        public PositionViewModel(Position position)
        {
            Id = position.Id;
            Name = position.Name;
            Grade = position.Grade;
        }
    }
}
