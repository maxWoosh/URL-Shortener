namespace API.Utils
{
    public class CodeGenerator
    {
        private const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        private readonly Random _random = new Random();

        /// <summary>
        /// Генерирует случайный код заданной длины
        /// </summary>
        /// <param name="length"></param>
        /// <returns>Случайная строка из Base62 Alphabet</returns>
        private string Generate(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentException("Длина кода не может быть равна ", nameof(length));
            }

            char[] code = new char[length];

            for (int i = 0; i < length; i++)
            {
                code[i] = Alphabet[_random.Next(Alphabet.Length)];
            }

            return new string(code);
        }


    }
}
