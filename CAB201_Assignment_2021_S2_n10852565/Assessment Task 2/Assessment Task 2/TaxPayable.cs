namespace AuctionHouse
{
    class TaxPayable : AuctionHouseCharge
    {
        public double BidAmount { get; }
        public double Tax { get; }

        public TaxPayable(bool deliverybool, string deliverytype, int charge, double bidamount) 
            : base (deliverybool, deliverytype, charge)
        {
            BidAmount = bidamount;
            if (deliverybool == true)
            {
                Tax = (bidamount * 0.15) * 5;
            }
            else if (deliverybool == false)
            {
                Tax = bidamount * 0.15;
            }
        }
    }
}
