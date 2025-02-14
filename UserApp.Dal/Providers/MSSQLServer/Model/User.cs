using System;
using System.ComponentModel.DataAnnotations;

namespace UserApp.Dal.Providers.MSSQLServer.Model
{
    public class User : Dal.Model.User
    {
        [Key]
        public override Guid Id { get;set; }
    }
}
