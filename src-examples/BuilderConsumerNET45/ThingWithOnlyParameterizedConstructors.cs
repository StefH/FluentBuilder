namespace BuilderConsumerNET45
{

    public class ThingWithOnlyParameterizedConstructors
    {
        public int X { get; set; }

        public int Y { get; set; }

        public string Z { get; set; }

        public ThingWithOnlyParameterizedConstructors(int x, int y, string z = "test")
        {
            X = x;
            Y = y;
            Z = z;
        }

        public ThingWithOnlyParameterizedConstructors(int x)
        {
            X = x;
            Y = -1;
            Z = "zzz";
        }
    }
}