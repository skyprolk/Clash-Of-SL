using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSS.Core;
using CSS.Helpers;

namespace CSS.WebAPI
{
    internal class API
    {
        private static IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
        private static HttpListener Listener;
        private static int Port = Utils.ParseConfigInt("APIPort"); // TODO: Add it to the config File
        private static string IP = ipHostInfo.AddressList[0].ToString();
        private static string URL = "http://" + IP + ":" + Port + "/";

        public static string HTML()
        {
            try
            {
                using (StreamReader sr = new StreamReader("WebAPI/HTML/Statistics.html"))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return "File not Found";
            }
        }

        public API()
        {
            new Thread(() =>
            {
                try
                {
                    if (!HttpListener.IsSupported)
                    {
                        Logger.Say("The current System doesn't support the WebAPI.");
                        return;
                    }

                    if (Port == 80)
                    {
                        Logger.Say("Can't start the API on Port 80 using now default Port(88)");
                        Port = 88;
                        URL = "http://" + IP + ":" + Port + "/";
                    }

                    Listener = new HttpListener();
                    Listener.Prefixes.Add(URL);
                    Listener.Prefixes.Add(URL + "api/");
                    Listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
                    Listener.Start();

                    Logger.Say("The WebAPI has been started on '" + Port + "'");

                    ThreadPool.QueueUserWorkItem((o) =>
                    {
                        while (Listener.IsListening)
                        {
                            ThreadPool.QueueUserWorkItem((c) =>
                            {
                                try
                                {
                                    HttpListenerContext ctx = (HttpListenerContext)c;

                                    foreach (string _URL in Listener.Prefixes.ToList<string>())
                                    {
                                        if (ctx.Request.Url.ToString().Contains(_URL))
                                        {
                                            if (ctx.Request.Url.ToString() == URL + "api/")
                                            {
                                                byte[] responseBuf = Encoding.UTF8.GetBytes(GetjsonAPI());
                                                ctx.Response.ContentLength64 = responseBuf.Length;
                                                ctx.Response.OutputStream.Write(responseBuf, 0, responseBuf.Length);
                                                ctx.Response.OutputStream.Close();
                                            }
                                            else
                                            {
                                                byte[] responseBuf = Encoding.UTF8.GetBytes(GetStatisticHTML());
                                                ctx.Response.ContentLength64 = responseBuf.Length;
                                                ctx.Response.OutputStream.Write(responseBuf, 0, responseBuf.Length);
                                                ctx.Response.OutputStream.Close();
                                            }
                                        }
                                        else
                                        {
                                            byte[] responseBuf = Encoding.UTF8.GetBytes(GetStatisticHTML());
                                            ctx.Response.ContentLength64 = responseBuf.Length;
                                            ctx.Response.OutputStream.Write(responseBuf, 0, responseBuf.Length);
                                            ctx.Response.OutputStream.Close();
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                }

                            }, Listener.GetContext());
                        }
                    });
                }
                catch (Exception)
                {
                    Logger.Say("Please check if the Port '" + Port + "' is not in use.");
                }
            }).Start();
        }

        public static void Stop()
        {
            Listener.Stop();
        }

        public static string GetStatisticHTML()
        {
            try
            {
                return HTML()
                    .Replace("%ONLINEPLAYERS%", ResourcesManager.m_vOnlinePlayers.Count.ToString())
                    .Replace("%INMEMORYPLAYERS%", ResourcesManager.m_vInMemoryLevels.Count.ToString())
                    .Replace("%INMEMORYALLIANCES%", ResourcesManager.GetInMemoryAlliances().Count.ToString())
                    .Replace("%TOTALCONNECTIONS%", ResourcesManager.GetConnectedClients().Count.ToString());
            }
            catch(Exception)
            {
                return "The server encountered an internal error or misconfiguration and was unable to complete your request. (500)";
            }
        }

        public static string GetjsonAPI()
        {
            JObject _Data = new JObject
            {
                {"online_players", ResourcesManager.m_vOnlinePlayers.Count.ToString()},
                {"in_mem_players", ResourcesManager.m_vInMemoryLevels.Count.ToString()},
                {"in_mem_alliances", ResourcesManager.GetInMemoryAlliances().Count.ToString()},
                {"connected_sockets", ResourcesManager.GetConnectedClients().Count.ToString()},
                {"all_players", ObjectManager.GetMaxPlayerID()},
                {"all_clans", ObjectManager.GetMaxAllianceID()}
            };
            return JsonConvert.SerializeObject(_Data, Formatting.Indented);
        }
    }
}
