using System;
using System.Collections.Generic;
using Avalonia.Metadata;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReversePolishNotation.Models;

namespace ReversePolishNotation.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ConvertToCommand))]
    private string? expression;

    // private List<string> _tokens;
    private List<string>? _tokens;

    [ObservableProperty]
    private string? _expressionRpn;

    [ObservableProperty]
    private string? _value;

    private bool ValidExpression() => !string.IsNullOrWhiteSpace(Expression);

    [RelayCommand (CanExecute = nameof(ValidExpression))]
    private void ConvertTo()
    {
        if(Expression != null)
        {
            try
            {
                _tokens = Rpn.ConvertTo(Expression);
                ExpressionRpn = string.Join(' ', _tokens);
                Calculate();
            }
            catch (Exception ex)
            {
                ExpressionRpn = ex.Message;
            }
        }
    }

    private void Calculate()
    {
        try
        {
            double val = Rpn.Solve(_tokens);
            Value = val.ToString();
        }
        catch (Exception ex)
        {
            Value = $"ERROR: {ex.Message}";
        }
    }
}
