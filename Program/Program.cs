using System;
using AddressBook;
using Logger;
using static Logger.Logger;

namespace Program
{
    class Program
    {
        private static User GetUser()
        {
            Console.Write("Enter user id: ");
            var userId = Console.ReadLine();

            if (string.IsNullOrEmpty(userId))
            {
                GetLogger().Warning("This user doesn't exist");
            }

            var user = new User(userId);

            Console.Write("First name: ");
            user.FirstName = Console.ReadLine();

            Console.Write("Last name: ");
            user.LastName = Console.ReadLine();

            return user;
        }

        private static void TestAddressBook(AddressBook.AddressBook addressBook)
        {
            string id;
            User user;
            var logger = GetLogger();
            try
            {
                while (true)
                {
                    Console.WriteLine("Do you want to add or remove user? (a/r).");
                    var choise = Console.ReadLine();
                    switch (choise)
                    {
                        case "a":
                            logger.Info("User will be add...");
                            user = GetUser();
                            if (user == null)
                            {
                                logger.Warning("User is null");
                            }
                            addressBook.AddUser(user);
                            break;
                        case "r":
                            logger.Info("Remove user operation selected");
                            Console.Write("User id: ");
                            id = Console.ReadLine();
                            if (string.IsNullOrEmpty(id))
                            {
                                logger.Warning("User do not exist");
                            }
                            addressBook.RemoveUser(id);
                            break;
                        default:
                            logger.Error("Invalid operation");
                            break;
                    }

                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }
        }

        static void Main(string[] args)
        {
            var logger = GetLogger();

            using (var strategy = new LoggerFile("loggerFile.txt", false))
            {
                logger.ChangeStrategy(strategy);
                logger.Info("Logger was started");
                var addressBook = new AddressBook.AddressBook();
                addressBook.UserAdded += HandleBookChanged;
                addressBook.UserRemoved += HandleBookChanged;
                TestAddressBook(addressBook);
                logger.Info("Logger finish work");
            }
        }

        public static void HandleBookChanged(object sender, AddressBookEventArgs args)
        {
            var logger = GetLogger();
            if (args.WasAdded)
            {
                logger.Info(args.Message);
            }
            else
            {
                logger.Debug(args.Message);
            }
        }
    }
}
