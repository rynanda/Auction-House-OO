namespace AuctionHouse
{
   class Delivery
    {
        public bool DeliveryBool { get; }
        public string DeliveryType { get; }

        public Delivery(bool deliverybool, string deliverytype)
        {
            DeliveryBool = deliverybool;
            DeliveryType = deliverytype;
        }
    }
}
