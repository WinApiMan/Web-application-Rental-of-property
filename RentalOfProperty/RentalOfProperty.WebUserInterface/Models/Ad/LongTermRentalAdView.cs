namespace RentalOfProperty.WebUserInterface.Models.Ad
{
    /// <summary>
    /// Long term ad view model.
    /// </summary>
    public class LongTermRentalAdView : RentalAdView
    {
        /// <summary>
        /// Gets or sets BYN price.
        /// </summary>
        public double BYNPrice { get; set; }

        /// <summary>
        /// Gets or sets USD price.
        /// </summary>
        public double USDPrice { get; set; }
    }
}
