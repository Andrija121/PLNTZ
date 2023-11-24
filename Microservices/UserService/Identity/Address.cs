namespace UserService.Identity
{
    public class Address
    {
        public Address() { }
        public int Id { get; set; }
        public string Name { get; set; }
        public int AddressNumber { get; set; }
        public string AdditionalInformation { get; set; }
        public string CityName { get; set; }
        public int CityId { get; set; }

    }
}