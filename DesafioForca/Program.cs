using System;
using System.Security.Cryptography;

namespace DesafioForca
{
    class Program
    {
        static void Main(string[] args)
        {
            var forca1 = new Forca();
            var forca2 = new ForcaLINQ();

            Console.WriteLine("Bem-Vinde");
            Console.WriteLine("O jogo da forca possui 2 modos, vidas infinitas ou 5 vidas");
            Console.Write("Digite 1 para jogar no modo devidas infinitas ou 2 para jogar no modo de 5 vidas: ");
            var choice = Console.ReadLine() ?? "";
            int value;
            var isInt = int.TryParse(choice, out value);

            while (choice.Length > 1 || !isInt || (value != 1 && value != 2))
            {
                Console.WriteLine("Opção inválida.");
                Console.Write("Digite 1 para jogar no modo devidas infinitas ou 2 para jogar no modo de 5 vidas: ");
                choice = Console.ReadLine() ?? "";
                isInt = int.TryParse(choice, out value);
            }

            
            if (value == 1) forca1.Game();
            forca2.Game();
        }
    }

    internal class Forca
    {
        static bool InputIsValid(string input)
        {
            if (
                input == null
                || input.Length == 0
                || input.Length > 1
                || int.TryParse(input, out _)
                || !Char.IsLetter(input[0])
            )
            {
                return false;
            }

            return true;
        }

        static List<int> CheckLetterInWord(string SelectedWord, string guess)
        {
            List<int> indexes = new();
            for (int i = 0; i < SelectedWord.Length; i++)
            {
                if (guess?.ToLower() == SelectedWord[i].ToString().ToLower())
                {
                    indexes.Add(i);
                }
            }

            return indexes;
        }

        static string ReplaceCharacters(List<int> indexes, string word, string character)
        {
            if (indexes.Count == 0)
                return word;

            for (int i = 0; i < indexes.Count; i++)
            {
                word = word.Remove(indexes[i], 1).Insert(indexes[i], character);
            }

            return word;
        }

        static bool PlayAgain(string misteryWord)
        {
            Console.Clear();
            Console.WriteLine($"Ganhou!! A palavra era: {misteryWord}");
            Console.Write("Deseja jogar novamente, digite 'S' ou 'N': ");
            string playAgain = Console.ReadLine() ?? "N";

            while (playAgain.ToLower() != "s" && playAgain.ToLower() != "n")
            {
                Console.Write(
                    "Comando Inválido, verifique se digitou 'S' ou 'N' e tente novamente: "
                );
                playAgain = Console.ReadLine() ?? "N";
            }

            if (playAgain.ToLower() == "n")
                return false;

            return true;
        }

        public void Game()
        {
            Random rnd = new();

            bool playing = true;
            string[] words =
            {
                "gostoso",
                "universo",
                "terra",
                "jardim",
                "brigadeiro",
                "lingua",
                "sapato",
                "espelho",
                "mar",
                "livro",
                "estudar",
                "sofa",
                "geladeira",
                "aeroporto",
                "almofada",
                "cogumelo"
            };

            string SelectedWord = words[rnd.Next(words.Length)];
            string misteryWord = new('_', SelectedWord.Length);
            string guess;
            List<string> guesses = new();

            while (playing)
            {
                Console.Clear();
                Console.WriteLine(
                    "Bem-vinde ao jogo da forca do Gustavo, aqui você tem quantas chances precisar. Boa Sorte!!!"
                );
                Console.WriteLine(
                    $"A sua palavra misteriosa tem {SelectedWord.Length} espaços: {misteryWord}"
                );
                Console.WriteLine($"Letras que já foram: {string.Join(", ", guesses)}");
                Console.Write("Qual o seu palpite de letra: ");
                guess = Console.ReadLine() ?? "";

                while (!InputIsValid(guess))
                {
                    Console.WriteLine($"O caractere que digitou é inválido, tente novamente.");
                    Console.Write("Qual o seu palpite de letra: ");
                    guess = Console.ReadLine() ?? "";
                }

                guesses.Add(guess);

                List<int> indexes = CheckLetterInWord(SelectedWord, guess);
                misteryWord = ReplaceCharacters(indexes, misteryWord, guess);

                if (misteryWord == SelectedWord)
                {
                    if (PlayAgain(misteryWord))
                    {
                        SelectedWord = words[rnd.Next(words.Length)];
                        misteryWord = new('_', SelectedWord.Length);
                        guesses = new();
                    }
                    else
                    {
                        playing = false;
                    }
                }
                Console.Clear();
            }
        }
    }

    internal class ForcaLINQ
    {
        Random Rnd;
        List<string> words;
        string selectedWord;
        string misteryWord;

        string guess;
        List<string> guesses;
        List<int> indexes;

        bool playing;
        int lives;

        public ForcaLINQ() 
        { 
            Rnd = new Random();
            words = new List<string>
            {
                "gostoso",
                "universo",
                "terra",
                "jardim",
                "brigadeiro",
                "lingua",
                "sapato",
                "espelho",
                "mar",
                "livro",
                "estudar",
                "sofa",
                "geladeira",
                "aeroporto",
                "almofada",
                "cogumelo"
            };
            selectedWord = words[Rnd.Next(words.Count)];
            misteryWord = new string('_', selectedWord.Length);

            guess = "";
            guesses = new List<string>();
            indexes = new List<int>();

            playing = true;
            lives = 5;
        }
        ReturnInputValid InputIsValid()
        {
            if (int.TryParse(guess, out _))
                return new ReturnInputValid(
                    false,
                    "Não são permitidos números. Por favor insira apenas uma letra!"
                );

            if (guess == null || !Char.IsLetter(guess[0]))
                return new ReturnInputValid(
                    false,
                    "Caractere Inválido. Por favor tente novamente!"
                );

            if (guess.Length == 0)
                return new ReturnInputValid(false, "Campo Vazio. Por favor insira uma letra!");

            if (guess.Length > 1)
                return new ReturnInputValid(
                    false,
                    "Caracteres Demais. Por favor insira apenas uma letra!"
                );

            return new ReturnInputValid(true, "Input Válido");
        }

        List<int> CheckLetterInWord()
        {
            var indexes = selectedWord
                .Select((ch, index) => (ch, index))
                .Where(tuple => tuple.ch.ToString() == guess)
                .Select(tuple => tuple.index)
                .ToList();

            return indexes;
        }

        void ReplaceCharacters()
        {
            indexes.ForEach(value => misteryWord = misteryWord.Remove(value, 1).Insert(value, guess));
        }

        bool PlayAgain(bool win)
        {
            Console.Clear();

            if (win)
            {
                Console.WriteLine($"Ganhou!! A palavra era: {selectedWord}");
                Console.Write("Deseja jogar novamente, digite 'S' ou 'N': ");
            }
            else
            {
                Console.WriteLine("Suas vidas acabaram. Você perdeu!");
                Console.WriteLine($"A palavra correta era {selectedWord}");
                Console.Write("Deseja jogar novamente, digite 'S' ou 'N': ");
            }

            var playAgain = Console.ReadLine() ?? "";
            while (playAgain.ToLower() is not "s" and not "n")
            {
                Console.Write(
                    "Comando Inválido, verifique se digitou 'S' ou 'N' e tente novamente: "
                );
                playAgain = Console.ReadLine() ?? "";
            }

            if (playAgain.ToLower() == "n")
                return false;

            Reset();
            return true;
        }

        void Reset()
        {
            lives = 5;
            selectedWord = words[Rnd.Next(words.Count)];
            misteryWord = new string('_', selectedWord.Length);
            guesses = new List<string>();
        }

        public void Game()
        {
            while (playing && lives > 0)
            {
                Console.Clear();
                Console.WriteLine(
                    $"Bem-vinde ao jogo da forca do Gustavo, você tem {lives} vidas. Boa Sorte!!!"
                );
                Console.WriteLine(
                    $"A sua palavra misteriosa tem {selectedWord.Length} espaços: {misteryWord}"
                );
                Console.WriteLine($"Letras que já foram: {string.Join(", ", guesses)}");
                Console.Write("Qual o seu palpite de letra: ");
                guess = Console.ReadLine() ?? "";

                var inputIsValid = InputIsValid();

                while (!inputIsValid.valid)
                {
                    Console.WriteLine(inputIsValid.message);
                    Console.Write("Qual o seu palpite de letra: ");
                    guess = Console.ReadLine() ?? "";

                    inputIsValid = InputIsValid();
                }

                guesses.Add(guess);

                indexes = CheckLetterInWord();
                ReplaceCharacters();

                if (indexes.Count == 0) lives--;

                if (misteryWord == selectedWord) playing = PlayAgain(true);
                if (lives == 0) playing = PlayAgain(false);
            }
        }
    }

    class ReturnInputValid
    {
        public bool valid;
        public string message;

        public ReturnInputValid(bool valid, string message)
        {
            this.valid = valid;
            this.message = message;
        }
    }
}
