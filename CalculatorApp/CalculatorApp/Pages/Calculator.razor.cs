using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;



namespace CalculatorApp.Pages
{
    public enum CustomOperations
    {
        Sin, Cos, Sqrt, Tan, Log
    }


    public partial class Calculator
    {
        [Inject]
        public ILocalStorageService LocalStorageService { get; set; }


        public string Result { get; set; }

        private double _secondnumber;
        private double _firstnumber;
        private string _operand;



        public void Number(string n)
        {
            Result += n;
        }

        public void Operand(string o)
        {
            if (Result != "")
            {
                _secondnumber = double.Parse(Result);
                Result = " ";
                _operand += o;
            }
        }

        public void ClearAll()
        {
            Result = "";
        }

        public void ClearOne()
        {
            if (Result.Length > 0)
            {
                Result = Result.Substring(0, Result.Length - 1);
            }
            if (Result.Length < 1)
            {
                Result = " ";
            }
        }


        protected override async Task OnInitializedAsync()
        {
            Result = await LocalStorageService.GetItemAsync<string>("Result");
            await Task.Delay(TimeSpan.FromSeconds(3));


        }

        public async Task PerformCustomOperation(CustomOperations operation)
        {
            if (Result == "" || Result == " ")
                return;

            double operandmath = double.Parse(Result);

            switch (operation)
            {
                case CustomOperations.Sqrt:
                    Result = Math.Sqrt(operandmath).ToString();
                    break;
                case CustomOperations.Sin:
                    Result = Math.Sin(operandmath).ToString();
                    break;
                case CustomOperations.Cos:
                    Result = Math.Cos(operandmath).ToString();
                    break;
                case CustomOperations.Tan:
                    Result = Math.Tan(operandmath).ToString();
                    break;
                case CustomOperations.Log:
                    Result = Math.Log(operandmath).ToString();
                    break;
            }

            await LocalStorageService.SetItemAsync("Result", Result);
        }

        public async Task Calculate()
        {
            if (Result != " ")
            {
                _firstnumber = double.Parse(Result);

                Result = " ";

                switch (_operand)
                {
                    case "+":
                        Result = (_secondnumber + _firstnumber).ToString();
                        break;

                    case "-":
                        Result = (_secondnumber - _firstnumber).ToString();
                        break;

                    case "*":
                        Result = (_secondnumber * _firstnumber).ToString();
                        break;

                    case "/":
                        Result = (_secondnumber / _firstnumber).ToString();
                        break;
                }
            }

            await LocalStorageService.SetItemAsync("Result", Result);
            await Task.Delay(100);
            toastService.ShowSuccess("saved Result");
        }

    }
}