namespace Core.MVVM
{
    public class DisplayViewModel : ViewModel
    {
        private Calculator _calculator;

        public Calculator Calculator => _calculator;

        public void Construct(Calculator calculator)
        {
            _calculator = calculator;
        }
    }
}