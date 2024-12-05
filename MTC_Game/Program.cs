using System;
using MTC_Game;



namespace MTC_Game
{
    /// <summary>This class contains the main entry point of the application.</summary>
    internal class Program
    {

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public constants                                                                                                 //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        /// <summary>Determines if debug token ("UserName-debug") will be accepted.</summary>
        public const bool ALLOW_DEBUG_TOKEN = true;


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // entry point                                                                                                      //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Application entry point.</summary>
        /// <param name="args">Command line arguments.</param>
        static void Main(string[] args)
        {
            HttpSvr svr = new();
            svr.Incoming += Svr_Incoming;

            svr.Run();
            Console.WriteLine("Bereit: http://127.0.0.1:12000\"");
        }



        private static void Svr_Incoming(object sender, HttpSvrEventArgs e)
        {
            Handler.HandleEvent(e);

            /*
            Console.WriteLine(e.Method);
            Console.WriteLine(e.Path);
            Console.WriteLine();
            foreach(HttpHeader i in e.Headers)
            {
                Console.WriteLine(i.Name + ": " + i.Value);
            }
            Console.WriteLine();
            Console.WriteLine(e.Payload);

            e.Reply(HttpStatusCode.OK, "Yo Baby!");
            */
        }
    }
}
