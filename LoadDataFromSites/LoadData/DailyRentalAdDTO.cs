namespace LoadData
{
    public class DailyRentalAdDTO : RentalAdDTO
    {
        public double BYNPricePerPerson { get; set; }

        public double USDPricePerPerson { get; set; }

        public double USDPricePerDay { get; set; }

        public double BYNPricePerDay { get; set; }
    }
}
