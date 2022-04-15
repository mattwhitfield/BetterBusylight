using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace BetterBusylight
{
    public class ConfigReader
    {
        delegate bool Parser<T>(string value, out T result);

        public static SequenceHandler ReadFrom(string configPath, out string audioIdentifier)
        {
            if (string.IsNullOrWhiteSpace(configPath))
            {
                throw new ArgumentNullException(nameof(configPath));
            }

            return ReadFrom(XElement.Parse(File.ReadAllText(configPath)), out audioIdentifier);
        }

        public static SequenceHandler ReadFrom(XElement element, out string audioIdentifier)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            var lightStyles = FindNode(element, "lightStyles");
            var audioDevice = FindNode(element, "audioDevice");

            audioIdentifier = GetAttribute(audioDevice, "searchString", ReadString, string.Empty, false);

            return ReadSequenceHandler(lightStyles);
        }

        private static XElement FindNode(XElement element, string nodeName)
        {
            var found = element.Descendants().FirstOrDefault(x => string.Equals(x.Name.LocalName, nodeName, StringComparison.OrdinalIgnoreCase));
            if (found == null)
            {
                throw new InvalidDataException("Could not find the node named '" + nodeName + "' in the '" + element.Name + "' node.");
            }
            return found;
        }

        private static SequenceHandler ReadSequenceHandler(XElement lightStyles)
        {
            var locked = FindNode(lightStyles, "locked");
            var idle = FindNode(lightStyles, "idle");
            var audioOnly = FindNode(lightStyles, "audioOnly");
            var cameraOnly = FindNode(lightStyles, "cameraOnly");
            var audioAndCamera = FindNode(lightStyles, "audioAndCamera");

            return new SequenceHandler(ReadSequence(locked), ReadSequence(idle), ReadSequence(audioOnly), ReadSequence(cameraOnly), ReadSequence(audioAndCamera));
        }

        private static Sequence ReadSequence(XElement lightStyle)
        {
            var flashFrequency = GetAttribute(lightStyle, "flashFrequency", double.TryParse, 0d);
            var steps = new List<Step>();
            foreach (var element in lightStyle.Elements())
            {
                if (string.Equals(element.Name.LocalName, "static", StringComparison.OrdinalIgnoreCase))
                {
                    steps.Add(ReadStatic(element));
                }
                else if (string.Equals(element.Name.LocalName, "fade", StringComparison.OrdinalIgnoreCase))
                {
                    steps.Add(ReadFade(element));
                }
            }

            return new Sequence(steps, flashFrequency);
        }

        private static Static ReadStatic(XElement element)
        {
            var duration = GetAttribute(element, "duration", double.TryParse, 1d);
            var color = GetAttribute(element, "color", ParseColor, Color.Black, false);

            return new Static(color, duration);
        }

        private static Fade ReadFade(XElement element)
        {
            var duration = GetAttribute(element, "duration", double.TryParse, 1d);
            var from = GetAttribute(element, "from", ParseColor, Color.Black, false);
            var to = GetAttribute(element, "to", ParseColor, Color.Black, false);

            return new Fade(from, to, duration);
        }

        private static bool ReadString(string value, out string result)
        {
            result = value;
            return true;
        }

        private static bool ParseColor(string value, out Color color)
        {
            try
            {
                color = ColorTranslator.FromHtml(value);
                return true;
            }
            catch
            {
                color = Color.Black;
                return false;
            }
        }

        private static T GetAttribute<T>(XElement element, string name, Parser<T> tryParse, T defaultValue, bool allowDefault = true)
        {
            var attribute = element.Attributes().FirstOrDefault(x => string.Equals(x.Name.LocalName, name, StringComparison.OrdinalIgnoreCase));
            if (attribute != null)
            {
                if (tryParse(attribute.Value, out var result))
                {
                    return result;
                }
                else if (!allowDefault)
                {
                    throw new InvalidDataException("The value '" + attribute.Value + "' of the attribute '" + name + "' could not be converted to type '" + typeof(T).Name + "' in node '" + element.Name + "'.");
                }
            }
            else if (!allowDefault)
            {
                throw new InvalidDataException("The attribute '" + name + "' could not be found in node '" + element.Name + "'.");
            }

            return defaultValue;
        }
    }
}
