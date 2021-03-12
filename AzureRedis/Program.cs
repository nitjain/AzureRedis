using System;
using StackExchange.Redis;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace AzureRedis
{
    class Program
    {

        private static IConfigurationRoot _config;

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>

            {
                //string cacheConnection = _config["CacheConnection"];
                string cacheConnection = "ReportHubRedisInstance.redis.cache.windows.net:6380,password=6DGfDR5uaTiJCYowyEvWaW3ETM0sqopvTS8qSvMh44Q=,ssl=True,abortConnect=False";
                return ConnectionMultiplexer.Connect(cacheConnection);
            });


        //prperty to return the Multiplexer connection 
        public static ConnectionMultiplexer Connection
        {

            get { return lazyConnection.Value; }
        }


        // loaded the appsettings file for the configuration 
        private static void CreateConfig()
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _config = builder.Build();

        }


        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            try
            {
                CreateConfig();

                IDatabase cache = lazyConnection.Value.GetDatabase();

                // Try to execute PING Command on Redis cache 
                string cacheCommand = "PING";
                Console.WriteLine("Cache Command: " + cacheCommand);
                Console.WriteLine("Response: " + cache.Execute(cacheCommand).ToString());

                cacheCommand = "GET Message";
                Console.WriteLine("Cache Command:" + cacheCommand + " or StringGet()");
                Console.WriteLine("Response :" + cache.StringGet("Message").ToString());

                cacheCommand = "SET Message \"Hello I can get Redis cache working from this console application :-)\"";
                Console.WriteLine("Cache Command" + cacheCommand + " or StringSet");
                Console.WriteLine("Response: " + cache.StringSet("Message", "Hello I can get Redis cache working from this console application"));

                cacheCommand = "GET Message";
                Console.WriteLine("Cache Command: " + cacheCommand + " or StringGet()");
                Console.WriteLine("Response :" + cache.StringGet("Message").ToString());

                cacheCommand = "CLIENT LIST";
                Console.WriteLine("Cache Command:" + cacheCommand);
                Console.WriteLine("Response: " + cache.Execute("CLIENT", "LIST").ToString().Replace("id=", "id="));


                ToDoItem toDoitem = new ToDoItem("100", "This is a new incoming item", DateTime.UtcNow);
                Console.WriteLine("Response: " + cache.StringSet("100", JsonConvert.SerializeObject(toDoitem)));

                ToDoItem itemFromCache = JsonConvert.DeserializeObject<ToDoItem>(cache.StringGet("100"));
                if (cache.KeyExists("100", flags: CommandFlags.None))
                {
                    Console.WriteLine("Item Id : " + itemFromCache.Itemid);
                    Console.WriteLine("Item Name : " + itemFromCache.ItemName);
                    Console.WriteLine("Item Date Time : " + itemFromCache.ItemDateTime);
                }

                lazyConnection.Value.Dispose();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

            }

        }
    }
}
