using System;
using System.Collections.Generic;
using System.Linq;
using CAB201;

namespace AuctionHouse
{
    class AuctionHouseClient
    {
        public static void Main()
        {
            /*Obtains what a user wants to do in the main menu.*/
            int UserOption = UserInterface.GetOption("Please select one of the following:",
                    ") Register as new Client",
                    ") Login as existing Client",
                    ") Exit");
            if (UserOption == 0)
            {
                var ClientRegister = new ClientAccount();
                ClientRegister.GetClientDetails();
            }
            else if (UserOption == 1)
            {
                var ClientLogin = new ClientAccount();
                ClientLogin.Login();
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }
    class ClientAccount
    {
        private string ClientName { get; set; } /*Defines necessary variables in class.*/
        private string ClientEmail { get; set; }
        private string ClientPassword { get; set; }
        private string ClientAddress { get; set; }
        public static string LoggedInUser { get; set; }
        private static List<ClientAccount> ClientsList = new List<ClientAccount>();
        public override string ToString()
        {
            return ClientName + ", " + ClientEmail;
        }
        public void GetClientDetails()
        {
            /*Obtain necessary information.*/
            string InputFullname = UserInterface.GetInput("\nFull name");
            string InputEmail = UserInterface.GetInput("\nEmail");
            string InputPassword = UserInterface.GetPassword("\nPassword");
            string InputAddress = UserInterface.GetInput("\nAddress");

            /*Adds necessary information to relevant list.*/
            ClientsList.Add(new ClientAccount()
            {
                ClientName = InputFullname, ClientEmail = InputEmail, 
                ClientPassword = InputPassword, ClientAddress = InputAddress
            });

            Console.WriteLine("\nUser {0} registered successfully.\n", InputFullname);
            AuctionHouseClient.Main();
        }
        public void Login()
        {
            string InputEmail = UserInterface.GetInput("\nEmail").Trim();
            string InputPassword = UserInterface.GetPassword("\nPassword");

            var ClientFindEmail = ClientsList.Find(x => x.ClientEmail.Contains(InputEmail));
            var ClientFindPassword = ClientsList.Find(x => x.ClientPassword.Contains(InputPassword));

            /*Checks if email and password are found in list.*/
            if ((ClientFindEmail != null) && (ClientFindPassword != null))
            {
                LoggedInUser = ClientFindEmail.ToString();
                Console.WriteLine("\nWelcome {0}\n", LoggedInUser);
                BiddingMenu.Menu();
            }
            else
            {
                Console.WriteLine("Email or password is not registered or is entered incorrectly. " +
                    "Please register or login again.\n");
                AuctionHouseClient.Main();
            }
        }
    }
    class BiddingMenu
    {
        public static void Menu()
        {
            int UserOption = UserInterface.GetOption("Please select one of the following:",
                    ") Register item for sale",
                    ") List my items",
                    ") Search for items",
                    ") Place a bid on an item",
                    ") List bids received for my items",
                    ") Sell one of my items to highest bidder",
                    ") Logout");

            if (UserOption == 0)
            {
                var ItemRegister = new ItemSystem();
                ItemRegister.RegisterItem();
            }
            if (UserOption == 1)
            {
                var ItemUserList = new ItemSystem();
                ItemUserList.ListUserItems();
            }
            if (UserOption == 2)
            {
                var ItemSearch = new ItemSystem();
                ItemSearch.SearchItems();
            }
            if (UserOption == 3)
            {
                var BiddingPlace = new BiddingSystem();
                BiddingPlace.PlaceBid();
            }
            if (UserOption == 4)
            {
                var BiddingReceived = new BiddingSystem();
                BiddingReceived.ListBidsReceived();
            }
            if (UserOption == 5)
            {
                var BiddingSell = new Selling();
                BiddingSell.SellItem();
            }
            if (UserOption == 6)
            {
                Console.WriteLine("User logged out.\n");
                ClientAccount.LoggedInUser = null;
                AuctionHouseClient.Main();
            }

        }
    }
    class ItemSystem
    {
        public string ClientEmail { get; set; }
        public string ItemType { get; set; }
        public string ItemDescription { get; set; }
        public int InitialBid { get; set; }
        public override string ToString()
        {
            return ClientEmail + ", " + ItemType + ", " + ItemDescription + ", " + InitialBid;
        }
        public static List<ItemSystem> ItemList = new List<ItemSystem>();
        public void RegisterItem()
        {
            string InputItemType = UserInterface.GetInput("\nType");
            string InputItemDescription = UserInterface.GetInput("\nDescription");
            int InputInitialBid = UserInterface.GetInt("\nInitial bid");
            string InputEmail = ClientAccount.LoggedInUser.Split(", ")[1];
            
            ItemList.Add(new ItemSystem()
            {
                ClientEmail = InputEmail,
                ItemType = InputItemType.ToLower(),
                ItemDescription = InputItemDescription.ToLower(),
                InitialBid = InputInitialBid
            });

            Console.WriteLine("\n{0}, {1} registered successfully.\n", InputItemType, InputItemDescription);
            BiddingMenu.Menu();
        }
        public void ListUserItems()
        {
            string InputEmail = ClientAccount.LoggedInUser.Split(", ")[1];
            var ClientFindEmail = ItemList.FindAll(x => x.ClientEmail.Contains(InputEmail));
            string items = string.Join(Environment.NewLine, ClientFindEmail);

            Console.WriteLine("\n" + items + "\n");
            BiddingMenu.Menu();
        }
        public void SearchItems()
        {
            string InputType = UserInterface.GetInput("\nType").Trim();
            var ItemsType = from i in ItemList
                            where i.ItemType == InputType.ToLower()
                            select i;

            Console.WriteLine("\nItems found:");
            foreach (var items in ItemsType)
                Console.WriteLine(items.ItemType + ", " + items.ItemDescription + ", " + items.InitialBid);
            Console.WriteLine("\n");
            BiddingMenu.Menu();
        }
    }
    class BiddingSystem
    {

        public static List<BiddingSystem> BidList = new List<BiddingSystem>();
        private string BidItemOwner { get; set; }
        private string BidItemType { get; set; }
        private string BidItemDescription { get; set; }
        public string BidderEmail { get; set; }
        public int BidAmount { get; set; }
        public bool HomeDelivery { get; set; }

        public override string ToString()
        {
            return BidderEmail + " bid $" + BidAmount + " for item " + BidItemType + ", " + BidItemDescription +
                " listed by " + BidItemOwner;
        }
        public void PlaceBid()
        {
            List<ItemSystem> ItemsForBid = new List<ItemSystem>();
            string InputType = UserInterface.GetInput("\nType").Trim();
            var ItemsType = from i in ItemSystem.ItemList
                            where i.ItemType == InputType.ToLower()
                            select i;

            foreach (var items in ItemsType)
                ItemsForBid.Add(items);

            int UserOption = UserInterface.GetOption("Please select one of the following:", ItemsForBid.ToArray());
            int BidPlaced = UserInterface.GetInt("\nEnter Bid ($)");
            bool HomeDelivery = UserInterface.GetBool("\nHome delivery? (true or false)");
            string BidderEmail = ClientAccount.LoggedInUser.Split(", ")[1];

            try
            {
                var ChosenItem = ItemsForBid.ElementAt(UserOption);

                string BidItemOwner = ChosenItem.ToString().Split(", ")[0];
                string BidItemType = ChosenItem.ToString().Split(", ")[1];
                string BidItemDescription = ChosenItem.ToString().Split(", ")[2];

                Console.WriteLine(BidItemType);

                BidList.Add(new BiddingSystem()
                {
                    BidItemOwner = BidItemOwner,
                    BidItemType = BidItemType,
                    BidItemDescription = BidItemDescription,
                    BidderEmail = BidderEmail,
                    BidAmount = BidPlaced,
                    HomeDelivery = HomeDelivery,
                });

                Console.WriteLine("\nSuccessfully bid ${0} on {1} listed by {2}\n", BidPlaced, BidItemType, BidItemOwner);
                ItemsForBid.Clear();
            }
            catch (System.ArgumentOutOfRangeException)
            {
                Console.WriteLine("\nSomething went wrong (item type may not be found), please try placing a bid again.\n");
            }
            BiddingMenu.Menu();
        }
        public void ListBidsReceived()
        {
            string InputEmail = ClientAccount.LoggedInUser.Split(", ")[1];
            var ClientFindEmail = BidList.FindAll(x => x.BidItemOwner.Contains(InputEmail));
            string items = string.Join(Environment.NewLine, ClientFindEmail);

            Console.WriteLine("\nBids received:");
            Console.WriteLine(items + "\n");
            BiddingMenu.Menu();
        }
    }
    class Selling
    {
        public static string BidderName { get; private set; }
        public static bool Delivery { get; private set; }
        public static string DeliveryType { get; private set; }
        public static int AuctionHouseCharge { get; private set; }
        public static double TaxPayable { get; private set; }

        public void SellItem()
        {
            List<ItemSystem> ItemsForSelling = new List<ItemSystem>();
            string InputEmail = ClientAccount.LoggedInUser.Split(", ")[1];
            var ClientFindEmail = ItemSystem.ItemList.FindAll(x => x.ClientEmail.Contains(InputEmail));
            string items = string.Join(Environment.NewLine, ClientFindEmail);

            Console.WriteLine("\nItems you have listed: ");
            Console.WriteLine(items);

            string InputType = UserInterface.GetInput("\nItem type you want to sell").Trim();
            var ItemsType = from i in ItemSystem.ItemList
                            where i.ItemType == InputType.ToLower()
                            select i;

            foreach (var item in ItemsType)
                ItemsForSelling.Add(item);

            int UserOption = UserInterface.GetOption("\nPlease select the item you want to sell:", ItemsForSelling.ToArray());

            try
            {
                /*Obtains necessary information.*/
                var ChosenItem = ItemsForSelling.ElementAt(UserOption);
                string SellItemType = ChosenItem.ToString().Split(", ")[1];
                string SellItemDescription = ChosenItem.ToString().Split(", ")[2];

                var HighestBidAmount = BiddingSystem.BidList.Max(x => x.BidAmount);
                var HighestBidder = from i in BiddingSystem.BidList
                                    where i.BidAmount == HighestBidAmount
                                    select i;

                foreach (var bidder in HighestBidder)
                    BidderName = bidder.BidderEmail;

                foreach (var delivery in HighestBidder)
                    Delivery = delivery.HomeDelivery;

                if (Delivery == true)
                {
                    DeliveryType = "Home Delivery";
                    AuctionHouseCharge = 20;
                    TaxPayable = (HighestBidAmount * 0.15) + 5;
                }
                else if (Delivery == false)
                {
                    DeliveryType = "Click and Collect";
                    AuctionHouseCharge = 10;
                    TaxPayable = HighestBidAmount * 0.15;
                }

                TaxPayable sellbid = new TaxPayable(Delivery, DeliveryType, AuctionHouseCharge, HighestBidAmount); /*Inheritance.*/
                Console.WriteLine("\n{0}, {1}, has been sold to {2} for ${3}.\n",
                    SellItemType, SellItemDescription, BidderName, HighestBidAmount);
                Console.WriteLine("${0} has been sent to the auction house for delivery type charge of {1}.\n",
                    sellbid.Charge, sellbid.DeliveryType);
                Console.WriteLine("Tax payable is ${0}.\n", TaxPayable);

                var RemoveSoldItem = ItemSystem.ItemList.Single(i => i.ItemDescription == SellItemDescription &&
                i.ItemType == SellItemType && i.ClientEmail == InputEmail);
                ItemSystem.ItemList.Remove(RemoveSoldItem);

                ItemsForSelling.Clear(); /*Removes sold item.*/
            }
            catch (System.ArgumentOutOfRangeException) /*If error found, return an error string.*/
            {
                Console.WriteLine("\nSomething went wrong (item type maybe not found). Please try again.\n");
            }
            BiddingMenu.Menu();
        }
    }
}
