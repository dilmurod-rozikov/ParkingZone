﻿using ParkingZoneApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingZones
{
    public class EditVM
    {
        [Required]
        public Guid? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public DateOnly? CreatedDate { get; init; }

        public EditVM(ParkingZone parkingZone)
        {
            Id = parkingZone.Id;
            Name = parkingZone.Name;
            Address = parkingZone.Address;
            CreatedDate = parkingZone.CreatedDate;
        }

        public ParkingZone MapToModel(ParkingZone parkingZone)
        {
            parkingZone.Id = Id;
            parkingZone.Name = Name;
            parkingZone.Address = Address;
            return parkingZone;
        }
    }
}
