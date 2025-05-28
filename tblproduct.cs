using System.ComponentModel.DataAnnotations;

namespace econest.Models
{
    public class tblproduct
    {
        [Key]
        public int pid { get; set; }

        public int cid { get; set; }

        public string pname { get; set; }

        public string pdes { get; set; }

        public int price { get; set; }

        public string material { get; set; }

        public string image { get; set; }
    }
}
