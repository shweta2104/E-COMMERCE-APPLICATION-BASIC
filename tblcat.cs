using System.ComponentModel.DataAnnotations;

namespace econest.Models
{
    public class tblcat
    {
        [Key]
        public int cid { get; set; }

        public string cname { get; set; }

        public string cdes { get; set; }
    }
}
