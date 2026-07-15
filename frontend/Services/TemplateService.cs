using System.Text.Json;
using Prelegal.Frontend.Models;

namespace Prelegal.Frontend.Services;

/// <summary>
/// Loads legal document templates from the shared repo dataset
/// (data/templates/*.json) and caches them. Path is configurable via the
/// "TemplatesPath" setting, resolved relative to the app content root.
/// </summary>
public sealed class TemplateService
{
    public const string MutualNdaId = "mutual-nda";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    private readonly string _templatesDir;
    private readonly Dictionary<string, NdaTemplate> _cache = new();
    private readonly Lock _gate = new();

    public TemplateService(IWebHostEnvironment env, IConfiguration config)
    {
        var configured = config["TemplatesPath"] ?? "../data/templates";
        _templatesDir = Path.GetFullPath(configured, env.ContentRootPath);
    }

    /// <summary>Loads a template by id, caching the parsed result. Throws if missing or invalid.</summary>
    public NdaTemplate Get(string id)
    {
        lock (_gate)
        {
            if (_cache.TryGetValue(id, out var cached))
                return cached;

            var path = Path.Combine(_templatesDir, id + ".json");
            if (!File.Exists(path))
                throw new FileNotFoundException(
                    $"Template '{id}' not found at '{path}'. Check the TemplatesPath setting.", path);

            var json = File.ReadAllText(path);
            var template = JsonSerializer.Deserialize<NdaTemplate>(json, JsonOptions)
                ?? throw new InvalidOperationException($"Template '{id}' at '{path}' deserialized to null.");

            _cache[id] = template;
            return template;
        }
    }

    public NdaTemplate GetMutualNda() => Get(MutualNdaId);
}
