using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace econest.Models
{
    public class tbluser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int uid { get; set; }

        public string uname { get; set; }

        public string email { get; set; }

        public string password { get; set; }

        public string role {  get; set; }   
    }
}
