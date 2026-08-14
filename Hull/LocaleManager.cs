using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace HullBreakerCompany.Hull;

public static class LocaleManager
{
    private static Dictionary<string, string> _localeData;
    private static string _loadedLanguage;
    private static string _missingLanguageWarning;
    private static string _parseErrorLanguage;

    private static string CurrentLanguage => Plugin.Language ?? "en";

    private static string PluginLocaleDir => Path.Combine(Path.GetDirectoryName(typeof(Plugin).Assembly.Location), "Languages");

    private static void EnsureLoaded()
    {
        if (_localeData != null && _loadedLanguage == CurrentLanguage) return;
        _localeData = null;
        _loadedLanguage = CurrentLanguage;

        string path = Path.Combine(PluginLocaleDir, CurrentLanguage + ".json");
        if (!File.Exists(path))
        {
            if (!CurrentLanguage.Equals("en", StringComparison.OrdinalIgnoreCase) && _missingLanguageWarning != CurrentLanguage)
            {
                Plugin.Mls.LogWarning($"Language package '{CurrentLanguage}.json' not found in {PluginLocaleDir}. Falling back to English.");
                _missingLanguageWarning = CurrentLanguage;
            }
            return;
        }
        try
        {
            JObject parsed = JObject.Parse(File.ReadAllText(path));
            _localeData = new Dictionary<string, string>();
            foreach (var pair in Flatten(parsed))
            {
                _localeData[pair.Key] = pair.Value;
            }
            Plugin.Mls.LogDebug($"Loaded locale file: {path}");
        }
        catch (Exception e)
        {
            if (_parseErrorLanguage != CurrentLanguage)
            {
                Plugin.Mls.LogError($"Failed to parse locale file {path}: {e.Message}");
                _parseErrorLanguage = CurrentLanguage;
            }
        }
    }

    private static Dictionary<string, string> Flatten(JToken root)
    {
        Dictionary<string, string> result = new();
        Stack<(JToken token, string path)> stack = new();
        stack.Push((root, ""));

        while (stack.Count > 0)
        {
            (JToken token, string path) = stack.Pop();

            switch (token)
            {
                case JObject obj:
                    foreach (var prop in obj.Properties())
                    {
                        string newPath = string.IsNullOrWhiteSpace(path) ? prop.Name : path + "." + prop.Name;
                        stack.Push((prop.Value, newPath));
                    }
                    break;
                case JArray arr:
                    for (int i = arr.Count - 1; i >= 0; i--)
                    {
                        stack.Push((arr[i], path + "." + i));
                    }
                    break;
                default:
                    string value = token.Value<string>();
                    if (value != null)
                    {
                        result[path] = value;
                    }
                    break;
            }
        }

        return result;
    }

    public static string Get(string key)
    {
        EnsureLoaded();
        if (_localeData == null || !_localeData.TryGetValue(key, out string value)) return null;
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    public static string GetParsed(string key, params (string Key, object Value)[] parameters)
    {
        string value = Get(key);
        if (value == null) return null;
        foreach (var parameter in parameters)
        {
            value = value.Replace($"{{{parameter.Key}}}", parameter.Value?.ToString() ?? "");
        }
        return value;
    }

    public static string GetDescription(string eventId) => Get($"events.{eventId}.description");

    public static string GetCommon(string key) => Get($"common.{key}");

    public static string GetConfigDescription(string keyId) => Get($"config.desc.{keyId}");

    public static List<string> GetMessages(string eventId) => GetList($"events.{eventId}.messages");

    public static List<string> GetShortMessages(string eventId) => GetList($"events.{eventId}.shortMessages");

    public static List<string> GetKillMessages(string eventId) => GetList($"events.{eventId}.killMessages");

    private static List<string> GetList(string keyPrefix)
    {
        EnsureLoaded();
        if (_localeData == null) return null;

        List<string> values = new();
        for (int i = 0; ; i++)
        {
            if (!_localeData.TryGetValue(keyPrefix + "." + i, out string value)) break;
            if (!string.IsNullOrWhiteSpace(value)) values.Add(value);
        }
        return values.Count == 0 ? null : values;
    }
}
