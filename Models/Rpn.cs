using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReversePolishNotation.Models;

public static class Rpn
{
    public static double Solve(List<string> tokens)
    {
        Stack<double> stack = new Stack<double>();

        foreach(var s in tokens)
        {
            double number;
            if(double.TryParse(s, out number))
            {
                stack.Push(number);
            }
            else
            {
                double right = stack.Pop();
                double left = stack.Pop();
                double result = s switch
                {
                    "+" => left + right,
                    "-" => left - right,
                    "*" => left * right,
                    "/" => left / right,
                    _ => throw new Exception("ERROR: Unknown operator."),
                };
                
                stack.Push(result);
            }
        }

        return stack.Peek();
    }

    public static List<string> ConvertTo(string expression)
    {
        List<string> rpn = new List<string>();

        Stack<char> stack = new Stack<char>();

        for(int i = 0; i < expression.Length; i++)
        {
            char c = expression[i];
            if(char.IsLetter(c))
            {
                throw new FormatException("ERROR: Letter(s) in the expression.");
            }
            else if(char.IsDigit(c))
            {
                StringBuilder number = new StringBuilder();
                while(i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.' || expression[i] == ','))
                {
                    if(expression[i] == '.')
                    {
                        number.Append(',');
                    }
                    else
                    {
                        number.Append(expression[i]);
                    }
                    
                    i++;
                }
                rpn.Add(number.ToString());
                i--;
            }
            else
            {
                switch (c)
                {
                    case '(':
                    {
                        stack.Push(c);
                        break;
                    }
                    case ')':
                    {
                        while(stack.Count > 0 && stack.Peek() != '(')
                        {
                            rpn.Add(stack.Pop().ToString());
                        }
                        stack.Pop();
                        break;
                    }
                    case '+':
                    case '-':
                    {                  
                        while(stack.Count > 0 && stack.Peek() != '(')
                        {
                            rpn.Add(stack.Pop().ToString());
                        }
                        stack.Push(c);
                        break;
                    }
                    case '*':
                    case '/':
                    {
                        while(stack.Count > 0 && stack.Peek() != '(' && stack.Peek() != '+' && stack.Peek() != '-')
                        {
                            rpn.Add(stack.Pop().ToString());
                        }
                        stack.Push(c);
                        break;
                    }
                    case ' ':
                    {
                        break;
                    }
                    default:
                    {
                        throw new FormatException($"ERROR: Unknown operator: {c}");
                    }
                }
            }
        }

        while(stack.Count != 0)
        {
            rpn.Add(stack.Pop().ToString());
        }

        return rpn;
    }
}
