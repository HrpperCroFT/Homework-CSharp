using System;

namespace Calculator {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Console calculator. Enter 'q' or 'exit' to quit program.");
            Console.WriteLine("Input format: <number1> <number2> <operation>");
            Console.WriteLine("Operations: + (add), - (subtract), * (multiply), / (divide)");

            while (true) {
                Console.Write("\n> ");
                string? input = Console.ReadLine()?.Trim();

                if (input == null) {
                    Console.WriteLine("Error: input is null");
                    break;
                }

                if (string.Equals(input, "q", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase)) {
                    break;
                }

                string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 3) {
                    Console.WriteLine("Error: Invalid format. Expected: <number1> <number2> <operation>");
                    continue;
                }

                if (!double.TryParse(parts[0], out double num1)) {
                    Console.WriteLine($"Error: '{parts[0]}' is not a valid number.");
                    continue;
                }

                if (!double.TryParse(parts[1], out double num2)) {
                    Console.WriteLine($"Error: '{parts[1]}' is not a valid number.");
                    continue;
                }

                string operation = parts[2];
                double result;

                switch (operation) {
                    case "+":
                        result = num1 + num2;
                        break;
                    case "-":
                        result = num1 - num2;
                        break;
                    case "*":
                        result = num1 * num2;
                        break;
                    case "/":
                        if (num2 == 0)
                        {
                            Console.WriteLine("Error: Division by zero is not allowed.");
                            continue;
                        }
                        result = num1 / num2;
                        break;
                    default:
                        Console.WriteLine($"Error: Unknown operation '{operation}'. Use +, -, *, or /.");
                        continue;
                }

                Console.WriteLine($"Result: {result}");
            }
        }
    }
}