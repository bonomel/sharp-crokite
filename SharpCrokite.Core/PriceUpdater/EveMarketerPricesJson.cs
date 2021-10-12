namespace SharpCrokite.Core.PriceUpdater
{
    public class EveMarketerPricesJSON
    { 
        public Buy buy { get; set; }
        public Sell sell { get; set; }

        public class Buy
        {
            public ForQuery forQuery { get; set; }
            //public ulong volume { get; set; }
            //public float wavg { get; set; }
            //public float avg { get; set; }
            //public float variance { get; set; }
            //public float stdDev { get; set; }
            //public float median { get; set; }
            public decimal fivePercent { get; set; }
            public decimal max { get; set; }
            public decimal min { get; set; }
            //public bool highToLow { get; set; }
            //public ulong generated { get; set; }
        }

        public class Sell
        {
            public ForQuery forQuery { get; set; }
            //public ulong volume { get; set; }
            //public float wavg { get; set; }
            //public float avg { get; set; }
            //public float variance { get; set; }
            //public float stdDev { get; set; }
            //public float median { get; set; }
            public decimal fivePercent { get; set; }
            public decimal max { get; set; }
            public decimal min { get; set; }
            //public bool highToLow { get; set; }
            //public ulong generated { get; set; }
        }

        public class ForQuery
        {
            public bool bid { get; set; }
            public int[] types { get; set; }
            public int[] regions { get; set; }
            public int[] systems { get; set; }
            //public int hours { get; set; }
            //public int minq { get; set; }
        }
    }
}
