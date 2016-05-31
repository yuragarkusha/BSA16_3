using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook
{
    public static class UsersExtension
    {
        public static IEnumerable<User> Over18YearsUsersFromKiev(this IEnumerable<User> users)
        {
            foreach (var user in users)
            {
                if (user.BirthDate < DateTimeOffset.Now.Date.AddYears(-18) &&
                    string.Compare(user.City, "Kiev", StringComparison.OrdinalIgnoreCase) == 0)
                    yield return user;
                
            }
        }
    }
}
