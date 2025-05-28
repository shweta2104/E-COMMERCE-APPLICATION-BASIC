namespace econest.Models
{
    public class prolist
    {
        public int pid { get; set; }

        public int cid { get; set; }

        public int uid { get; set; }
        public string pname{ get; set; }

        public string pdes { get; set; }

        public int price { get; set; }
        public string material { get; set; }

        public string image { get; set; }

        public int qty { get; set; }

        public DateTime created_at { get; set; }
    }
}
