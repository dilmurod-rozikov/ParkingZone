﻿using ParkingZoneApp.Models;
using ParkingZoneApp.Models.Entities;
using ParkingZoneApp.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ParkingZoneApp.ViewModels.ParkingSlots
{
    public class ListItemVM
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public Category Tariff { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        [Required]
        public ParkingZone ParkingZone { get; set; }

        public ListItemVM(ParkingSlot slot)
        {
            Id = slot.Id;
            Number = slot.Number;
            Tariff = slot.Tariff;
            IsAvailable = slot.IsAvailable;
        }

        public static IEnumerable<ListItemVM> MapToVM(IEnumerable<ParkingSlot> slots)
        {
            return slots.Select(slot => new ListItemVM(slot));
        }
    }
}