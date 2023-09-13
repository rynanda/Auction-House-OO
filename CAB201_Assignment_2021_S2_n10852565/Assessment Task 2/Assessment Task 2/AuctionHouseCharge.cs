namespace AuctionHouse
{
    class AuctionHouseCharge : Delivery
    {
        public int Charge { get; }

        public AuctionHouseCharge(bool deliverybool, string deliverytype, int charge) : base (deliverybool, deliverytype)
        {
            Charge = charge;
        }
    }
}
