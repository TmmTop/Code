using NetDimension.NanUI;

namespace OrderExample
{
    public partial class UI : HtmlUIForm
    {
        public UI() : base("embedded://UI/index.html")
        {
            InitializeComponent();
        }

        private void UI_Load(object sender, System.EventArgs e)
        {

        }
    }
}
