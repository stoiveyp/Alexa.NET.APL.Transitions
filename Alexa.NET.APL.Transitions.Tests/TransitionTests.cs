using System;
using System.IO;
using System.Linq;
using Alexa.NET.APL.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Alexa.NET.APL.TransitionTests
{
    public class TransitionTests
    {
        private JObject Commands { get; }
        public TransitionTests()
        {
            Commands = LoadTransitions().Value<JObject>("commands");
        }

        [Fact]
        public void RollIn()
        {
            Assert.True(EnsureTransition(Transitions.RollIn(), "rollIn"));
        }

        private bool EnsureTransition(CommandDefinition command, string transitionName)
        {
            var actual = JObject.FromObject(command);
            var expected = Commands.Value<JObject>(transitionName);
            var result = JToken.DeepEquals(expected, actual);
            if (!result)
            {
                OutputTrimEqual(expected, actual);
            }

            return result;
        }


        private static void OutputTrimEqual(JObject expectedJObject, JObject actualJObject, bool output = true)
        {
            foreach (var prop in actualJObject.Properties().ToArray())
            {
                if (JToken.DeepEquals(actualJObject[prop.Name], expectedJObject[prop.Name]))
                {
                    actualJObject.Remove(prop.Name);
                    expectedJObject.Remove(prop.Name);
                }
            }

            foreach (var prop in actualJObject.Properties().Where(p => p.Value is JObject).Select(p => new { name = p.Name, value = p.Value as JObject }).ToArray())
            {
                OutputTrimEqual(prop.value, expectedJObject[prop.name].Value<JObject>(), false);
            }

            if (output)
            {
                Console.WriteLine(expectedJObject.ToString());
                Console.WriteLine(actualJObject.ToString());
            }
        }

        private static JObject LoadTransitions()
        {
            using (var reader = new JsonTextReader(new StreamReader(File.OpenRead("apl-transitions.json"))))
            {
                return new JsonSerializer().Deserialize<JObject>(reader);
            }
        }
    }
}
