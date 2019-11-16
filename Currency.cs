using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;

namespace Bot
{
    public class Currency
    {
        public class ApiResult
        {
            public Dictionary<string, Rate> Valute { get; set; }
        }


        public class Rate
        {
            public int Nominal { get; set; }
            public string Name { get; set; }
            public float Value { get; set; }
        }

        private RestClient RC = new RestClient();
        private const string API_URL = "https://www.cbr-xml-daily.ru/daily_json.js";

        private ApiResult rates;

        public Currency()
        {
        }

        // USD, EUR, CHF, GBP
        public void download()
        {
            var Request = new RestRequest(API_URL);
            var Response = RC.Get(Request);
            var json = Response.Content;

            rates = JsonConvert.DeserializeObject<ApiResult>(json);
        }

        public string getRate(string currency)
        {
            if (rates.Valute.ContainsKey(currency))
            {
                var rate = rates.Valute[currency];
                return $"За {rate.Nominal} {rate.Name} дают {rate.Value} рублей";
            } else
            {
                return $"There is no {currency} currency";
            }
        }
    }
}