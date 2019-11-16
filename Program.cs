using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bot
{
    class Program
    {
        private static Currency currencyApi;
        private static Dictionary<string, string> DataBase;
        
        static void Main(string[] args)
        {
            currencyApi = new Currency();
            currencyApi.download();
            var Data = System.IO.File.ReadAllText("C:/Users/Waynosaur/source/repos/Bot/Bot/database.json");
            DataBase = JsonConvert.DeserializeObject<Dictionary<string, string>>(Data);

            var Api = new TelegramAPI();
            //Api.sendMessage("Yo sobaki, Im Naruto Uzumaki", 219793136);
            while(true)
            {
                var updates = Api.getUpdates();
                foreach(var update in updates)
                {
                    if (update.message == null || update.update_id == null || update.message.text == null)
                    {
                        continue;
                    }
                     var answer = answerQuestion(update.message.text);
                    var message = $"{answer}"; //{update.message.chat.first_name}, 
                    Api.sendMessage(message, update.message.chat.id);
                }
            }
        }
        
        private static string answerQuestion(string Question)
        {
            var UserQuestion = Question.ToLower();
            var Answers = new List<string>();

            foreach (var Entry in DataBase)
            {
                if (UserQuestion.Contains(Entry.Key))
                {
                    Answers.Add(Entry.Value);
                }
            }

            if (UserQuestion.Contains("time"))
            {
                var Time = DateTime.Now.ToString("HH:mm");
                Answers.Add($"Current time {Time}");
            }

            if (UserQuestion.Contains("day"))
            {
                var Date = DateTime.Now.ToString("dd.MM.yyyy");
                Answers.Add($"Today is {Date}");
            }

            if (UserQuestion.Contains("quote"))
            {
                var forismatic = new Forismatic();
                Answers.Add(forismatic.getRandom());
            }

            if (UserQuestion.Contains("exchange rate"))
            {
                var code = UserQuestion.Substring(UserQuestion.Length - 3).ToUpper(); //последние 3 символа
                Answers.Add(currencyApi.getRate(code));
            }

            if (Answers.Count == 0)
            {
                Answers.Add("I don't understand you :(");
            }

            var Result = String.Join(", ", Answers);
            return Result;
        }
    }
}