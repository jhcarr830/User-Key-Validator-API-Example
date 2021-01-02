using System.Collections.Generic;

namespace ExampleAPI
{
    public class User
    {
        public string UserName { get; set; }
        public string UserKey { get; set; }
        public string Permission { get; set; }
    }

    public class Globals
    {
        public static List<User> userList = new List<User>();
        public static User CurrentUser;
    }
}
