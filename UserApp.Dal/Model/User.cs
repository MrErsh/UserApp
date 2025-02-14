using System;

namespace UserApp.Dal.Model
{
    public class User
    {
        public virtual Guid Id { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
