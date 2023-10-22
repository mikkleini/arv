namespace Calc
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            CreateButtons();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        /// <summary>
        /// Number buttons column and row offsets
        /// </summary>
        private readonly (int col, int row)[] NumberButtonsLayout = new[]
        {
            (0, 3), // 0
            (0, 2), // 1
            (1, 2), // 2
            (2, 2), // 3
            (0, 1), // 4
            (1, 1), // 5
            (2, 1), // 6
            (0, 0), // 7
            (1, 0), // 8
            (2, 0), // 9
        };

        private void CreateButtons()
        {
            for (int i = 0; i < 10; i++)
            {
                Button btn = new()
                {
                    Text = i.ToString()
                };

                Grid.SetColumn(btn, NumberButtonsLayout[i].col);
                Grid.SetRow(btn, 1 + NumberButtonsLayout[i].row);

                ButtonGrid.Add(btn);
            }
        }
    }
}