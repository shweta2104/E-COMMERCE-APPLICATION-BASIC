using System.ComponentModel.DataAnnotations;

namespace econest.Models
{
    public class tblcart
    {
        [Key]
        public int cid { get; set; }
        public int uid { get; set; }
        public int pid { get; set; }
        public int qty { get; set; }
        public DateTime created_at { get; set; }
    }

}
