using MTC_Game;
using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace MTC_Game
{
    public class SessionHandler : Handler, IHandler
    {
        private static readonly Dictionary<string, string> ActiveSessions = new();

        public override bool Handle(HttpSvrEventArgs e)
        {
            // Check for the /sessions endpoint with POST method
            if (e.Path.TrimEnd('/', ' ', '\t') == "/sessions" && e.Method == "POST")
            {
                return ProcessSessionPost(e);
            }

            return false;
        }

        private bool ProcessSessionPost(HttpSvrEventArgs e)
        {
            JsonObject? responseMessage;
            int statusCode = HttpStatusCode.BAD_REQUEST;

            try
            {
                JsonNode? requestData = JsonNode.Parse(e.Payload);
                if (requestData != null)
                {
                    string username = requestData["username"]!.ToString();
                    string password = requestData["password"]!.ToString();

                    var loginResult = User.Logon(username, password);

                    if (loginResult.Success)
                    {
                        return ProcessSuccessfulLogin(e, loginResult.Token, username);
                    }
                    else
                    {
                        statusCode = HttpStatusCode.UNAUTHORIZED;
                        responseMessage = CreateJsonResponse(false, "Logon failed.");
                    }
                }
                else
                {
                    responseMessage = CreateJsonResponse(false, "Invalid request format.");
                }
            }
            catch (Exception)
            {
                responseMessage = CreateJsonResponse(false, "Invalid request.");
            }

            e.Reply(statusCode, responseMessage?.ToJsonString());
            return true;
        }

        private bool ProcessSuccessfulLogin(HttpSvrEventArgs e, string token, string username)
        {
            ActiveSessions[token] = username;

            var responseMessage = new JsonObject
            {
                ["success"] = true,
                ["message"] = "User logged in.",
                ["token"] = token
            };

            var headers = new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {token}" }
            };

            e.Reply(HttpStatusCode.OK, responseMessage.ToJsonString(), headers);
            return true;
        }

        private static JsonObject CreateJsonResponse(bool success, string message)
        {
            return new JsonObject
            {
                ["success"] = success,
                ["message"] = message
            };
        }

        public static string? GetUsernameFromToken(string token)
        {
            return ActiveSessions.TryGetValue(token, out var username) ? username : null;
        }
    }
}
