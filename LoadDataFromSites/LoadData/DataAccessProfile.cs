using AutoMapper;

namespace LoadData
{
    /// <summary>
    /// Class for mapping settings.
    /// </summary>
    public class DataAccessProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessLogicProfile"/> class.
        /// </summary>
        public DataAccessProfile()
        {
            CreateMap<DailyRentalAdDTO, RentalAdDTO>().ReverseMap();
            CreateMap<LongTermRentalAdDTO, RentalAdDTO>().ReverseMap();
        }
    }
}
