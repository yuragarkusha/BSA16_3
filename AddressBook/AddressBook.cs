using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook
{
    public class AddressBookEventArgs : EventArgs
    {
        public readonly string Message;
        public readonly bool WasAdded;

        public AddressBookEventArgs(string message, bool wasAdded)
        {
            Message = message;
            WasAdded = wasAdded;
        }
    }
    public class AddressBook
    {
        private readonly List<User> _userList;
        private readonly List<User> _users = new List<User>();

        public AddressBook()
        {
            _userList = new List<User>();
        }

        public bool AddUser(User user)
        {
            var detectedUser = _userList.Find(item => (user.Id == item.Id));
            if (detectedUser != null)
            {
                if (UserAdded != null)
                {
                    var msg = $"This user( id={user.Id} ) has already been added earlier.";
                    UserAdded(this, new AddressBookEventArgs(msg, false));
                }
                return false;
            }
            _userList.Add(user);
            user.TimeAdded = DateTime.Now;
            if (UserAdded != null)
            {
                var msg = $"The user( id= {user.Id} ) has been successfully added.";
                UserAdded(this, new AddressBookEventArgs(msg, true));
            }
            return true;
        }

        public bool RemoveUser(string userId)
        {
            var removedId = _userList.RemoveAll(item => (userId == item.Id));

            if (removedId == 0)
            {
                if (UserRemoved != null)
                {
                    var msg = $"This user(id={userId}) does not exist";
                    UserRemoved(this, new AddressBookEventArgs(msg, false));
                }
                return false;
            }

            if (UserRemoved != null)
            {
                var msg = $"The user(id={userId}) has been successfully deleted.";
                UserRemoved(this, new AddressBookEventArgs(msg, true));
            }
            return true;
        }

        public bool RemoveUser(User user)
        {
            RemoveUser(user.Id);
            return true;
        }

        public IEnumerable<User> GmailUsers()
        {
            return _users.Where(u => u.Email.EndsWith("@gmail.com"));
        }

        public IEnumerable<User> RecentlyAddedFemale()
        {
            return from user in _users where 
                   (user.Gender == User.GenderEnum.Female &&
                   user.TimeAdded > DateTimeOffset.Now.AddDays(-10))
                   select user;
        }

        public IEnumerable<User> BornInJanuary()
        {
            return _users.Where(u => u.BirthDate.Month == 1 &&
                                    !string.IsNullOrEmpty(u.Address) &&
                                    !string.IsNullOrEmpty(u.PhoneNumber))
                        .OrderByDescending(u => u.LastName);
        }

        public IDictionary<string, List<User>> UsersGenderDictionary()
        {
            return _users.GroupBy(u => u.Gender).ToDictionary(u => u.Key.ToString().ToLower(), u => u.ToList());
        }

        public int BirthdayTodayUsersCount(string city)
        { 
                return (from user in _users where user.City == city &&
                        user.BirthDate.Month == DateTime.Today.Month &&
                        user.BirthDate.Day == DateTime.Today.Day select
                        user).Count();
        }

        public IEnumerable<User> PagingUsers(Func<User, bool> predicate, int first, int last)
        {
            return _users.Where(predicate).Skip(first).Take(last);
        }
        public IEnumerable<User> PeopleWhoHaveBirthdayToday()
        {
            return
                _users.Where(
                    user => user.BirthDate.Month == DateTime.Now.Month && user.BirthDate.Day == DateTime.Now.Day);
        }

        public event EventHandler<AddressBookEventArgs> UserAdded;
        public event EventHandler<AddressBookEventArgs> UserRemoved;
    }
}
