using Vsto.Sample.Client.Commands;
using Vsto.Sample.Client.Models;
using Vsto.Sample.Client.ViewModels;

namespace Vsto.Sample.Tests
{
    [TestFixture]
    public class ContentViewModelTests
    {
        internal class MockServiceClient : IServiceClient
        {
            internal void RaiseActionLogged(string subject)
            {
                ActionLogged?.Invoke(subject);
            }

            internal void RaiseDataPublished(object subject)
            {
                DataPublished?.Invoke(subject);
            }

            internal Func<string> GetEndpoint { get; set; }
            internal Action<string> SetEndpoint { get; set; }
            internal Action OnLoadData { get; set; }

            internal MockServiceClient()
            {
                GetEndpoint = () => null;
                SetEndpoint = _ => { };
                OnLoadData = () => { };
            }
            public string Endpoint { get => GetEndpoint(); set => SetEndpoint(value); }

            public event Action<string> ActionLogged;
            public event Action<object> DataPublished;

            public void LoadData()
            {
                OnLoadData();
            }
        }

        private readonly static Action<object> EmptyAction = _ => { };

        private ContentViewModel _target;
        private MockServiceClient _mockServiceClient;

        [SetUp]
        public void Setup()
        {
            _mockServiceClient = new MockServiceClient();
            _target = new ContentViewModel(_mockServiceClient);
        }

        [Test]
        public void TestServiceEndpointGetsAndSetsServiceClientEndpointAndPropertyChangedIsRaised()
        {
            var wasServiceEndpointSetCalled = false;
            var wasServiceEndpointGetCalled = false;
            var wasPropertyChangedCalled = false;
            _mockServiceClient.GetEndpoint = () => { wasServiceEndpointGetCalled = true; return null; };
            _mockServiceClient.SetEndpoint = _ => wasServiceEndpointSetCalled = true;
            _target.PropertyChanged += (sender, subject) => wasPropertyChangedCalled = subject.PropertyName == "ServiceEndpoint";
            _target.ServiceEndpoint = "http://SomeValue";
            Assert.IsTrue(wasServiceEndpointGetCalled);
            Assert.IsTrue(wasServiceEndpointSetCalled);
            Assert.IsTrue(wasPropertyChangedCalled);
        }

        [Test]
        public void TestWhenOutputIsSetPropertyChangedIsRaised()
        {
            var wasPropertyChangedCalled = false;
            _target.PropertyChanged += (sender, subject) => wasPropertyChangedCalled = subject.PropertyName == "Output";
            _target.Output = "something";
            Assert.IsTrue(wasPropertyChangedCalled);
        }

        [Test]
        public void TestWhenLoadContentIsExecutedThenLoadDataIsCalled()
        {
            var wasLoadDataCalled = false;
            _mockServiceClient.OnLoadData = () => wasLoadDataCalled = true;
            _target.LoadContent.Execute(null);
            Assert.IsTrue(wasLoadDataCalled);
        }

        [Test]
        public void TestWhenActionLoggedIsRaisedThenOutputSetAndPropertyChangedIsRaised()
        {
            var wasPropertyChangedCalled = false;
            _target.PropertyChanged += (sender, subject) => wasPropertyChangedCalled = subject.PropertyName == "Output";
            var expectedLogString = "Something to log.";
            _mockServiceClient.RaiseActionLogged(expectedLogString);
            Assert.IsTrue(wasPropertyChangedCalled);
            Assert.That(expectedLogString, Is.SameAs(_target.Output));
        }
    }
}