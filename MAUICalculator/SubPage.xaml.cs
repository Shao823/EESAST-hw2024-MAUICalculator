namespace MAUICalculator;

public partial class SubPage : ContentPage
{
    private double currentNumber = 0;
    private double lastNumber = 0;
    private string currentOperator = "";
    private bool isResult = false;

    public SubPage()
    {
        InitializeComponent();
    }

    private void OnNumberClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var number = button.Text;

        if (isResult || displayLabel.Text == "0")
        {
            displayLabel.Text = "";
            if (number == ".")
                displayLabel.Text = "0";
            isResult = false;
        }

        displayLabel.Text += number;
        currentNumber = double.Parse(displayLabel.Text);
    }

    private void OnOperatorClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var op = button.Text;

        if (isResult && currentOperator == "")
        {
            isResult = false;
            displayLabel.Text = "0";
        }

        if (currentOperator != "")
        {
            if (!isResult)
            {
                Calculate();
                displayLabel.Text = lastNumber.ToString();
                isResult = true;
            }
        }
        else
        {
            lastNumber = currentNumber;
            displayLabel.Text = "0";
            isResult = false;
        }

        currentOperator = op;
    }

    private void OnEqualClicked(object sender, EventArgs e)
    {
        if (currentOperator != "")
        {
            Calculate();
            displayLabel.Text = lastNumber.ToString();
            isResult = true;
            currentOperator = "";
        }
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        currentNumber = 0;
        lastNumber = 0;
        currentOperator = "";
        isResult = false;
        displayLabel.Text = "0";
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        if (displayLabel.Text.Length > 0)
        {
            if (displayLabel.Text == lastNumber.ToString() && isResult)
            {
                displayLabel.Text = "0";
                isResult = false;
            }
            else
            {
                displayLabel.Text = displayLabel.Text.Substring(0, displayLabel.Text.Length - 1);
                if (displayLabel.Text.Length == 0 || displayLabel.Text == "-")
                    displayLabel.Text = "0";
            }

            currentNumber = double.TryParse(displayLabel.Text, out double parsedValue) ? parsedValue : 0;
        }
    }

    private async void OnMainPageClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private void OnAdvancedOperatorClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var op = button.Text;

        switch (op)
        {
            case "x^y":
                if (currentOperator == "x^y" && !isResult)
                {
                    // 如果之前已经选择了乘方操作符并且还没有得到结果
                    lastNumber = Math.Pow(lastNumber, currentNumber);
                    displayLabel.Text = lastNumber.ToString();
                    isResult = true;
                }
                else
                {
                    // 保存当前输入的基数
                    lastNumber = currentNumber;
                    currentOperator = "x^y"; // 设置操作符
                    isResult = false;
                    displayLabel.Text = "0"; // 准备输入指数
                }
                break;
            case "lg":
                if (currentNumber <= 0)
                {
                    displayLabel.Text = "Error";
                }
                else
                {
                    lastNumber = Math.Log10(currentNumber);
                    displayLabel.Text = lastNumber.ToString();
                }
                isResult = true;
                break;
            case "√":
                if (currentNumber < 0)
                {
                    displayLabel.Text = "Error";
                }
                else
                {
                    lastNumber = Math.Sqrt(currentNumber);
                    displayLabel.Text = lastNumber.ToString();
                }
                isResult = true;
                break;
            case "x!":
                if (currentNumber < 0 || currentNumber != (int)currentNumber)
                {
                    displayLabel.Text = "Error";
                }
                else
                {
                    lastNumber = Factorial((int)currentNumber);
                    displayLabel.Text = lastNumber.ToString();
                }
                isResult = true;
                break;
            default:
                break;
        }
    }

    private void Calculate()
    {
        switch (currentOperator)
        {
            case "+":
                lastNumber += currentNumber;
                break;
            case "-":
                lastNumber -= currentNumber;
                break;
            case "*":
                lastNumber *= currentNumber;
                break;
            case "/":
                if (currentNumber == 0)
                {
                    displayLabel.Text = "Error";
                    return;
                }
                lastNumber /= currentNumber;
                break;
            case "x^y":
                lastNumber = Math.Pow(lastNumber, currentNumber);
                break;
            default:
                break;
        }
        lastNumber = Math.Round(lastNumber, 4);
        currentNumber = lastNumber;
    }

    private double Factorial(int n)
    {
        if (n < 0)
            return double.NaN; // Return NaN for negative inputs
        if (n == 0 || n == 1)
            return 1;
        double result = 1;
        for (int i = 2; i <= n; i++)
        {
            result *= i;
        }
        return result;
    }
}


