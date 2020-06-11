using System.Windows;
using System.Windows.Controls;

namespace Funktionenplotter.UserControls
{
    /// <summary>
    /// Interaction logic for Header.xaml
    /// </summary>
    public partial class Header : UserControl
    {
        public Header()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The dependency property of <see cref="HeadlineText"/>
        /// </summary>
        public static readonly DependencyProperty FourthCoefficientGraphProperty = DependencyProperty.Register(
            nameof(FourthCoefficientGraph), typeof(string), typeof(Header), new PropertyMetadata(default(string)));

        /// <summary>
        /// Gets or sets the text of the headline
        /// </summary>
        public string FourthCoefficientGraph
        {
            get => (string)GetValue(FourthCoefficientGraphProperty);
            set => SetValue(FourthCoefficientGraphProperty, value);
        }

        /// <summary>
        /// The dependency property of <see cref="HeadlineText"/>
        /// </summary>
        public static readonly DependencyProperty ThirdCoefficientGraphProperty = DependencyProperty.Register(
            nameof(ThirdCoefficientGraph), typeof(string), typeof(Header), new PropertyMetadata(default(string)));

        /// <summary>
        /// Gets or sets the text of the headline
        /// </summary>
        public string ThirdCoefficientGraph
        {
            get => (string)GetValue(ThirdCoefficientGraphProperty);
            set => SetValue(ThirdCoefficientGraphProperty, value);
        }

        /// <summary>
        /// The dependency property of <see cref="HeadlineText"/>
        /// </summary>
        public static readonly DependencyProperty SecondCoefficientGraphProperty = DependencyProperty.Register(
            nameof(SecondCoefficientGraph), typeof(string), typeof(Header), new PropertyMetadata(default(string)));

        /// <summary>
        /// Gets or sets the text of the headline
        /// </summary>
        public string SecondCoefficientGraph
        {
            get => (string)GetValue(SecondCoefficientGraphProperty);
            set => SetValue(SecondCoefficientGraphProperty, value);
        }

        /// <summary>
        /// The dependency property of <see cref="HeadlineText"/>
        /// </summary>
        public static readonly DependencyProperty FirstCoefficientGraphProperty = DependencyProperty.Register(
            nameof(FirstCoefficientGraph), typeof(string), typeof(Header), new PropertyMetadata(default(string)));

        /// <summary>
        /// Gets or sets the text of the headline
        /// </summary>
        public string FirstCoefficientGraph
        {
            get => (string)GetValue(FirstCoefficientGraphProperty);
            set => SetValue(FirstCoefficientGraphProperty, value);
        }

        /// <summary>
        /// The dependency property of <see cref="HeadlineText"/>
        /// </summary>
        public static readonly DependencyProperty BProperty = DependencyProperty.Register(
            nameof(B), typeof(string), typeof(Header), new PropertyMetadata(default(string)));

        /// <summary>
        /// Gets or sets the text of the headline
        /// </summary>
        public string B
        {
            get => (string)GetValue(BProperty);
            set => SetValue(BProperty, value);
        }
    }
}
