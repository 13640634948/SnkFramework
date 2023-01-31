using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Newtonsoft.Json;
using SnkFramework.Network.ContentDelivery.Editor;
using SnkFramework.NuGet;
using SnkFramework.NuGet.Basic;
using SnkFramework.NuGet.Features.Logging;
using SnkFramework.NuGet.Features.Patch;
using UnityEditor;
using UnityEngine;

namespace SnkFramework.PatchService
{
    namespace Demo
    {
        internal class SnkJsonParser : ISnkJsonParser
        {
            public T FromJson<T>(string json) where T : class => JsonConvert.DeserializeObject<T>(json);

            public string ToJson(object target) => JsonConvert.SerializeObject(target);
        }

        public static class ChannelPatcherDemo
        {
            static string PersistentRepo = "PersistentRepo";
            static string ChannelName = "windf_iOS"; 
            static string AppVersion = "1.0.0";
            private static void internalTest(bool upload = false)
            {
                try
                {
                    //SnkPatch.JsonParser = new SnkJsonParser();
                    //Snk.IoCProvider.RegisterSingleton<ISnkJsonParser>(new SnkJsonParser());
                    //Snk.IoCProvider.RegisterSingleton<ISnkCodeGenerator>(new SnkCodeGenerator());
                    //Snk.IoCProvider.RegisterSingleton<ISnkLogger>(new SnkLogger());

                    var builder = SnkPatch.CreatePatchBuilder<SnkJsonParser>(PersistentRepo, ChannelName, AppVersion);
                    ISnkFileFinder[] sourcePaths =
                    {
                        new SnkFileFinder("ProjectSettingsDemo")
                        {
                            //filters = new [] {"FSTimeGet"},
                            ignores = new[] { ".DS_Store" },
                        },
                    };
                    builder.Build(sourcePaths.ToList());
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                if (upload)
                    Upload();
            }

            [MenuItem("SnkPatcher/Demo-Test")]
            public static void Test()
            {
                internalTest();
            }

            [MenuItem("SnkPatcher/Demo-Test_Upload")]
            public static void Test_Upload()
            {
                internalTest(true);
            }

            [MenuItem("SnkPatcher/Upload")]
            public static void Upload()
            {
                var storage = new SnkCOSStorage();
                var keys = Directory.GetFiles(Path.Combine(PersistentRepo,ChannelName), "*.*", SearchOption.AllDirectories);
                var list = keys.Where(key => !Path.GetFileName(key).StartsWith(".")).ToList();
                storage.PutObjects(list);
            }
        }
    }
}