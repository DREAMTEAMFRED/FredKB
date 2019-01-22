﻿using System;
using System.Collections.Generic;
using System.Json;
using System.Net.Http;
using System.Text;

namespace FredKB
{
    class Program
    {
        static void Main(string[] args)
        {
            // Represents the various elements used to create HTTP request URIs
            // for QnA Maker operations.
            // From Publish Page: HOST
            // Example: https://YOUR-RESOURCE-NAME.azurewebsites.net/qnamaker
            string host = "https://qnafred.azurewebsites.net/qnamaker";

            // Authorization endpoint key
            // From Publish Page
            string endpoint_key = "9ed8e55e-e00b-4f2e-a4cf-87098d19c2a6";

            // Management APIs postpend the version to the route
            // From Publish Page, value after POST
            // Example: /knowledgebases/ZZZ15f8c-d01b-4698-a2de-85b0dbf3358c/generateAnswer
            string route = "/knowledgebases/85e578a7-bf44-4f12-b46e-26efa40b1653/generateAnswer";

            string quest = Console.ReadLine();

            // JSON format for passing question to service
            string question = @"{'question': '" + quest + "','top': 1}";

            string answer = "";

            // Create http client
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // POST method
                request.Method = HttpMethod.Post;

                // Add host + service to get full URI
                request.RequestUri = new Uri(host + route);

                // set question
                request.Content = new StringContent(question, Encoding.UTF8, "application/json");

                // set authorization
                request.Headers.Add("Authorization", "EndpointKey " + endpoint_key);

                // Send request to Azure service, get response
                var response = client.SendAsync(request).Result;
                var jsonResponse = response.Content.ReadAsStringAsync().Result;

                // Output JSON response
                //Console.WriteLine(jsonResponse);

                JsonObject jsonDoc = (JsonObject)JsonValue.Parse(jsonResponse);
                JsonArray jsonArray = (JsonArray)jsonDoc["answers"];

                foreach (JsonObject obj in jsonArray)
                {
                    JsonValue text;
                    obj.TryGetValue("answer", out text);
                    answer = text.ToString();
                }
                Console.WriteLine(answer.Replace("\"", ""));

                Console.ReadLine();
            }
        }
    }
}