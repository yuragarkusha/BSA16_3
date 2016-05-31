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
                GetLogger().Warning("This user is don't exist");
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

                AddUsers(addressBook,logger);

                logger.Info(string.Join(", ", addressBook.GmailUsers()) + ": users who have gmail.com domain");
                logger.Info(string.Join(", ", UsersArray.Over18YearsUsersFromKiev()) + ": over 18 years users from Kiev");
                logger.Info(string.Join(", ", addressBook.RecentlyAddedFemale()) + ": female users who was added last 10 days");
                logger.Info(string.Join(", ", addressBook.BornInJanuary()) + ": users who born in January and who has address and phone number");
                foreach (var users in addressBook.UsersGenderDictionary())
                {
                    logger.Info($"{users.Key}(s) : " + string.Join(", ", users.Value));
                }
                logger.Info(string.Join(", ", addressBook.BirthdayTodayUsersCount("Kiev")) + ": users who live in Kiev and have birthday today");
                logger.Info(string.Join(", ", addressBook.PagingUsers(u => !string.IsNullOrEmpty(u.FirstName), 1, 3)) + ": paging");

                Console.WriteLine();
                Console.WriteLine("Press key...");
                Console.ReadKey();

                //TestAddressBook(addressBook);
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

        public static void AddUsers(AddressBook.AddressBook addressBook,Logger.Logger logger)
        {
            foreach (var user in UsersArray)
            {
                try
                {
                    addressBook.AddUser(user);
                }
                catch (ArgumentException ex)
                {
                    logger.Error(ex.ToString());
                }
            }
        }


        private static readonly User[] UsersArray =
        {
            new User
            {
                Address = "Yangekya str. 92",
                BirthDate = new DateTime(1994,3,22),
                City = "kiev",
                Email = "omimi@gmail.com",
                FirstName = "Serhiy",
                LastName = "Lizniy",
                Gender = User.GenderEnum.Male,
                PhoneNumber = "+380945443344",
                TimeAdded = DateTimeOffset.Now
            },
            new User
            {
                Address = "Lenina str.20",
                BirthDate = new DateTime(1964,1,14),
                City = "Moscow",
                Email = "oli@outlook.com",
                FirstName = "Olya",
                LastName = "Rumich",
                Gender = User.GenderEnum.Female,
                PhoneNumber = "+320334448797",
                TimeAdded = DateTimeOffset.Now.AddDays(-1)
            },
            new User
            {
                Address = "Muzeyna str. 67",
                BirthDate = new DateTime(1998,5,31),
                City = "Vinnitsya",
                Email = "mollin@gmail.com",
                FirstName = "Mihaiil",
                LastName = "Odintsov",
                Gender = User.GenderEnum.Male,
                PhoneNumber = "+380988773248",
                TimeAdded = DateTimeOffset.Now
            }
        };
    }
}
