using System;

namespace LoadData
{
    public class RentalAdDTO
    {
        public int Id { get; set; }

        public string SourceLink { get; set; }

        public int RentalAdNumber { get; set; }

        public DateTime UpdateDate { get; set; }

        public string Region { get; set; }

        public string District { get; set; }

        public string Locality { get; set; }

        public string Address { get; set; }

        public int TotalCountOfRooms { get; set; }

        public int RentCountOfRooms { get; set; }

        public double TotalArea { get; set; }

        public double LivingArea { get; set; }

        public double KitchenArea { get; set; }

        public int TotalFloors { get; set; }

        public int Floor { get; set; }

        public double XMapCoordinate { get; set; }

        public double YMapCoordinate { get; set; }

        public string Bathroom { get; set; }

        public string Notes { get; set; }

        public string Description { get; set; }

        public string Facilities { get; set; }

        public int RentalType { get; set; }

        public int TotalViews { get; set; }

        public int MonthViews { get; set; }

        public int WeekViews { get; set; }

        public double LandArea { get; set; }
    }
}
