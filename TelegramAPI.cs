using System;
using Newtonsoft.Json;
using RestSharp;

namespace Bot
{
    public class TelegramAPI
    {
        
        public class ApiResult
        {
            public Update[] result { get; set; }
        }

        public class Update
        {
            public int? update_id { get; set; }
            public Message message { get; set; }
        }

        public class Message
        {
            public Chat chat { get; set; }
            public string text { get; set; }
        }

        public class Chat
        {
            public int id { get; set; }
            public string first_name { get; set; }
        }

        private int lastUpdateId = 0;
        public TelegramAPI() {}
        

        RestClient RC = new RestClient();

        const string API_URL = "https://api.telegram.org/bot" + SecretKey.API_KEY + "/";

        public void sendMessage(string text, int chat_id)
        {
            sendApiRequest("sendMessage", $"chat_id={chat_id}&text={text}");
        }

        public Update[] getUpdates()
        {
            var json = sendApiRequest("getUpdates", $"offset={lastUpdateId}");
            var apiResult = JsonConvert.DeserializeObject<ApiResult>(json);
            if (apiResult.result == null)
            {
                return new Update[] { };
            }
            foreach(var update in apiResult.result)
            {
                if (update.message == null || update.update_id == null || update.message.text == null)
                {
                    continue;
                } else
                {
                Console.WriteLine($"Получен апдейт {update.update_id}, "
                    + $"сообщение от {update.message.chat.first_name}, "
                    + $"текст: {update.message.text}");
                lastUpdateId = (int)(update.update_id + 1);
                }
            }
            return apiResult.result;
        }

        private string sendApiRequest(string ApiMethod, string Params)
        {
            //Формирование url
            var Url = API_URL + ApiMethod + "?" + Params;

            //Объект запроса
            var Request = new RestRequest(Url);

            //Выполнение запроса
            var Response = RC.Get(Request);

            //Результат запроса
            return Response.Content;
        }
    }
}