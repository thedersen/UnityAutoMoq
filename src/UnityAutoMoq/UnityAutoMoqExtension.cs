using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace UnityAutoMoq
{
    public class UnityAutoMoqExtension : UnityContainerExtension
    {
        private readonly UnityAutoMoqContainer autoMoqContainer;

        public UnityAutoMoqExtension(UnityAutoMoqContainer autoMoqContainer)
        {
            this.autoMoqContainer = autoMoqContainer;
        }

        protected override void Initialize()
        {
            Context.Strategies.Add(new UnityAutoMoqBuilderStrategy(autoMoqContainer), UnityBuildStage.PreCreation);
        }
    }
}