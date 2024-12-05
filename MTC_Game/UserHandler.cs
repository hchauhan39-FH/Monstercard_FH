using MTC_Game;
using MTC_Game.Exceptions;
using System;
using System.Text.Json.Nodes;

namespace MTC_Game
{
    /// <summary>This class implements a handler for user-specific requests.</summary>
    public class UserHandler : Handler, IHandler
    {
        /// <summary>Handles an incoming HTTP request.</summary>
        /// <param name="e">Event arguments.</param>
        public override bool Handle(HttpSvrEventArgs e)
        {
            JsonObject? response = null;
            int statusCode = HttpStatusCode.BAD_REQUEST;

            // Create user
            if (e.Path.TrimEnd('/', ' ', '\t') == "/users" && e.Method == "POST")
            {
                try
                {
                    JsonNode? json = JsonNode.Parse(e.Payload);
                    if (json != null)
                    {
                        User.Create((string)json["username"]!,
                                    (string)json["password"]!,
                                    (string?)json["fullName"] ?? "",
                                    (string?)json["email"] ?? "");
                        statusCode = HttpStatusCode.OK;
                        response = new JsonObject
                        {
                            ["success"] = true,
                            ["message"] = "User created."
                        };
                    }
                }
                catch (UserException ex)
                {
                    response = new JsonObject
                    {
                        ["success"] = false,
                        ["message"] = ex.Message
                    };
                }
                catch (Exception)
                {
                    response = new JsonObject
                    {
                        ["success"] = false,
                        ["message"] = "Invalid request."
                    };
                }

                e.Reply(statusCode, response?.ToJsonString());
                return true;
            }
            // Retrieve information about the logged-in user
            else if (e.Path == "/users/me" && e.Method == "GET")
            {
                (bool Success, User? User) authResult = Token.Authenticate(e);

                if (authResult.Success)
                {
                    statusCode = HttpStatusCode.OK;
                    response = new JsonObject
                    {
                        ["success"] = true,
                        ["username"] = authResult.User!.Username,
                        ["fullName"] = authResult.User!.FullName,
                        ["email"] = authResult.User!.Email
                    };
                }
                else
                {
                    statusCode = HttpStatusCode.UNAUTHORIZED;
                    response = new JsonObject
                    {
                        ["success"] = false,
                        ["message"] = "Unauthorized."
                    };
                }

                e.Reply(statusCode, response?.ToJsonString());
                return true;
            }
            // Other user-related requests
            else if (e.Path.StartsWith("/users"))
            {
                if (e.Method == "GET" && e.Path == "/users")
                {
                    return HandleGetAllUsers(e);
                }
                else if (e.Method == "GET" && e.Path.StartsWith("/users/"))
                {
                    return HandleGetUser(e);
                }
                else if (e.Method == "POST" && e.Path.EndsWith("/stack/add-package"))
                {
                    return HandleAddPackage(e);
                }
                else if (e.Method == "POST" && e.Path.EndsWith("/deck/choose"))
                {
                    return HandleChooseDeck(e);
                }
            }

            return false;
        }

        /// <summary>Handles retrieving all users.</summary>
        private bool HandleGetAllUsers(HttpSvrEventArgs e)
        {
            JsonObject? response = null;
            int statusCode = HttpStatusCode.UNAUTHORIZED;

            (bool Success, User? User) authResult = Token.Authenticate(e);

            if (authResult.Success)
            {
                JsonArray usersArray = new JsonArray();
                foreach (var user in User.GetAllUsers())
                {
                    usersArray.Add(new JsonObject
                    {
                        ["username"] = user.Username,
                        ["fullName"] = user.FullName,
                        ["email"] = user.Email
                    });
                }

                statusCode = HttpStatusCode.OK;
                response = new JsonObject
                {
                    ["success"] = true,
                    ["users"] = usersArray
                };
            }
            else
            {
                response = new JsonObject
                {
                    ["success"] = false,
                    ["message"] = "Unauthorized."
                };
            }

            e.Reply(statusCode, response?.ToJsonString());
            return true;
        }

        /// <summary>Handles retrieving a specific user.</summary>
        private bool HandleGetUser(HttpSvrEventArgs e)
        {
            JsonObject? response = null;
            int statusCode = HttpStatusCode.NOT_FOUND;

            string requestedUsername = e.Path.Substring("/users/".Length);

            if (User.Exists(requestedUsername))
            {
                User? user = User.Get(requestedUsername);
                if (user != null)
                {
                    statusCode = HttpStatusCode.OK;
                    response = new JsonObject
                    {
                        ["success"] = true,
                        ["username"] = user.Username,
                        ["fullName"] = user.FullName,
                        ["email"] = user.Email
                    };
                }
            }
            else
            {
                response = new JsonObject
                {
                    ["success"] = false,
                    ["message"] = "User not found."
                };
            }

            e.Reply(statusCode, response?.ToJsonString());
            return true;
        }

        /// <summary>Handles adding a package to the user's stack.</summary>
        private bool HandleAddPackage(HttpSvrEventArgs e)
        {
            JsonObject? response = null;
            int statusCode = HttpStatusCode.UNAUTHORIZED;

            (bool Success, User? User) authResult = Token.Authenticate(e);

            if (authResult.Success)
            {
                statusCode = HttpStatusCode.OK;
                response = new JsonObject
                {
                    ["success"] = true,
                    ["message"] = "Package added to user's stack."
                };
            }
            else
            {
                response = new JsonObject
                {
                    ["success"] = false,
                    ["message"] = "Unauthorized."
                };
            }

            e.Reply(statusCode, response?.ToJsonString());
            return true;
        }

        /// <summary>Handles selecting a new deck from the user's stack.</summary>
        private bool HandleChooseDeck(HttpSvrEventArgs e)
        {
            JsonObject? response = null;
            int statusCode = HttpStatusCode.UNAUTHORIZED;

            (bool Success, User? User) authResult = Token.Authenticate(e);

            if (authResult.Success)
            {
                statusCode = HttpStatusCode.OK;
                response = new JsonObject
                {
                    ["success"] = true,
                    ["message"] = "Deck selected from user's stack."
                };
            }
            else
            {
                response = new JsonObject
                {
                    ["success"] = false,
                    ["message"] = "Unauthorized."
                };
            }

            e.Reply(statusCode, response?.ToJsonString());
            return true;
        }
    }
}
