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
        private List<User> _userList;

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

        public event EventHandler<AddressBookEventArgs> UserAdded;
        public event EventHandler<AddressBookEventArgs> UserRemoved;
    }
}
