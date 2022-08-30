using Vsto.Sample.Client.Commands;

namespace Vsto.Sample.Tests
{
    [TestFixture]
    public class DelegateCommandTests
    {
        private readonly static Action<object> EmptyAction = _ => { };

        private DelegateCommand _target;

        [Test]
        public void TestCanExecuteCallsOnCanExecuteAndReturnsOnCanExecute()
        {
            var wasOnCanExecuteCalled = false;
            _target = new DelegateCommand(EmptyAction, _ => wasOnCanExecuteCalled = true);
            var result = _target.CanExecute(null);
            Assert.IsTrue(wasOnCanExecuteCalled);
            Assert.IsTrue(result);
        }

        [Test]
        public void TestExecuteCallsOnExecute()
        {
            var wasOnExecuteCalled = false;
            _target = new DelegateCommand(_ => wasOnExecuteCalled = true);
            _target.Execute(null);
            Assert.IsTrue(wasOnExecuteCalled);
        }
    }
}