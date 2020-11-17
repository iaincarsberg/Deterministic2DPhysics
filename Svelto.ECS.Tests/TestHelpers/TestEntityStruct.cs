namespace Svelto.ECS.Tests
{
    struct TestEntityStruct : IEntityComponent, INeedEGID
    {
        public float floatValue;
        public int   intValue;

        public TestEntityStruct(float floatValue, int intValue) : this()
        {
            this.floatValue = floatValue;
            this.intValue   = intValue;
        }

        public EGID ID { get; set; }
    }
}