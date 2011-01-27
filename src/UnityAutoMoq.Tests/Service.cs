namespace UnityAutoMoq.Tests
{
    public class Service : IService
    {
        public Service()
        {
        }

        public Service(IAnotherService anotherService)
        {
            AnotherService = anotherService;
        }

        public IAnotherService AnotherService
        {
            get; set;
        }

        public string PropertyWithoutSetter { get; private set; }
        public string Property { get; set; }
    }
}