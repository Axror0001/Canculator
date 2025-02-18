using FirstCanculator.DTO;
using FirstCanculator.Models;
using FirstCanculator.Repository;

namespace FirstCanculator.Service
{
    public class CanculatorService
    {
        private readonly ICanculatorRepository _repository;
        private readonly ILogger<CanculatorService> _logger;

        public CanculatorService(ICanculatorRepository repository, ILogger<CanculatorService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<CanculatorDTO> Create(CanculatorDTO canculatorDTO)
        {
            if (canculatorDTO == null || string.IsNullOrWhiteSpace(canculatorDTO.Action))
            {
                return null;
            }

            try
            {
                double resultValue = Calculate(canculatorDTO.Action);

                var canculator = new CanculatorModels
                {
                    Action = canculatorDTO.Action,
                    Result = resultValue,
                    CreatedAt = DateTime.UtcNow,
                };

                var savedCanculator = await _repository.Create(canculator);

                var result = new CanculatorDTO
                {
                    Action = savedCanculator.Action,
                    Result = savedCanculator.Result,
                    CreatedAt = DateTime.UtcNow,
                };
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Xatolik yuz berdi: Create metodida");
                throw;
            }
        }

        public double Calculate(string expression)
        {
            var tokens = Tokenize(expression);
            var postfix = InfixToPostfix(tokens);
            return EvaluatePostfix(postfix);
        }
        private List<string> Tokenize(string expression)
        {
            var tokens = new List<string>();
            string number = "";

            foreach (char c in expression)
            {
                if (char.IsDigit(c) || c == '.')
                {
                    number += c; 
                }
                else if (c == ' ')
                {
                    continue; 
                }
                else
                {
                    if (!string.IsNullOrEmpty(number))
                    {
                        tokens.Add(number);
                        number = "";
                    }
                    tokens.Add(c.ToString()); 
                }
            }
            if (!string.IsNullOrEmpty(number))
            {
                tokens.Add(number);
            }

            return tokens;
        }
        private List<string> InfixToPostfix(List<string> tokens)
        {
            var output = new List<string>();
            var operators = new Stack<string>();

            var precedence = new Dictionary<string, int>
        {
            { "+", 1 },
            { "-", 1 },
            { "*", 2 },
            { "/", 2 }
        };

            foreach (var token in tokens)
            {
                if (double.TryParse(token, out _)) 
                {
                    output.Add(token);
                }
                else if (precedence.ContainsKey(token)) 
                {
                    while (operators.Count > 0 && precedence.ContainsKey(operators.Peek()) &&
                           precedence[operators.Peek()] >= precedence[token])
                    {
                        output.Add(operators.Pop()); 
                    }
                    operators.Push(token); 
                }
            }

            while (operators.Count > 0)
            {
                output.Add(operators.Pop());
            }

            return output;
        }

        private double EvaluatePostfix(List<string> postfix)
        {
            var stack = new Stack<double>();

            foreach (var token in postfix)
            {
                if (double.TryParse(token, out double number)) 
                {
                    stack.Push(number);
                }
                else 
                {
                    double operand2 = stack.Pop();
                    double operand1 = stack.Pop();
                    double result = ApplyOperation(token, operand1, operand2);
                    stack.Push(result);
                }
            }

            return stack.Pop(); 
        }

        private double ApplyOperation(string operation, double operand1, double operand2)
        {
            switch (operation)
            {
                case "+": return operand1 + operand2;
                case "-": return operand1 - operand2;
                case "*": return operand1 * operand2;
                case "/": return operand1 / operand2;
                default: throw new InvalidOperationException("Noto'g'ri operator");
            }
        }



        public async Task<List<CanculatorModels>> GetAll()
        {
            try
            {
                return await _repository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Xatolik yuz berdi: GetAll metodida");
                throw;
            }
        }
    }
}
