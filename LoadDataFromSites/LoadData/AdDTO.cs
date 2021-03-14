using System.Collections.Generic;

namespace LoadData
{
    public class AdDTO
    {
        public RentalAdDTO RentalAd { get; set; }

        public ContactPersonDTO ContactPerson { get; set; }

        public IEnumerable<HousingPhotoDTO> HousingPhotos { get; set; }
    }
}
