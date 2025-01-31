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
                double resultValue = EvaluateExpression(canculatorDTO.Action);

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

        private double EvaluateExpression(string expression)
        {
            List<double> values = new();
            List<char> operators = new();
            int i = 0;

            while (i < expression.Length)
            {
                char toChar = expression[i];

                if (char.IsDigit(toChar) || toChar == '.')
                {
                    int start = i;
                    while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.')) i++;
                    values.Add(double.Parse(expression[start..i]));
                    continue;
                }
                else if (IsOperator(toChar))
                {
                    while (operators.Count > 0 && Priority(operators[^1]) >= Priority(toChar))
                        ApplyOperator(operators, values);
                    operators.Add(toChar);
                }
                i++;
            }

            while (operators.Count > 0)
                ApplyOperator(operators, values);

            return values[^1];
        }

        private static bool IsOperator(char c) => "+-*/".Contains(c);

        private static int Priority(char op) => op is '+' or '-' ? 1 : 2;

        private static void ApplyOperator(List<char> operators, List<double> values)
        {
            double b = values[^1]; values.RemoveAt(values.Count - 1);
            double a = values[^1]; values.RemoveAt(values.Count - 1);
            char op = operators[^1]; operators.RemoveAt(operators.Count - 1);

            values.Add(op switch
            {
                '+' => a + b,
                '-' => a - b,
                '*' => a * b,
                '/' when b != 0 => a / b,
                '/' => throw new Exception("Nolga bo‘lish mumkin emas"),
                _ => throw new Exception("Noto‘g‘ri operator")
            });
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
