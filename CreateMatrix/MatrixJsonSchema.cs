﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema.Generation;
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace CreateMatrix;

public class MatrixJsonSchemaBase
{
    protected const string SchemaUri = "https://raw.githubusercontent.com/ptr727/NxWitness/main/CreateMatrix/JSON/Matrix.schema.json";

    [JsonProperty(PropertyName = "$schema", Order = -3)]
    public string Schema { get; } = SchemaUri;

    [DefaultValue(0)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, Order = -2)]
    public int SchemaVersion { get; set; } = MatrixJsonSchema.Version;
}

public class MatrixJsonSchema : MatrixJsonSchemaBase
{
    public const int Version = 1;

    private static readonly JsonSerializerSettings Settings = new()
    {
        Formatting = Formatting.Indented,
        StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
        NullValueHandling = NullValueHandling.Ignore,
        ObjectCreationHandling = ObjectCreationHandling.Replace
    };

    [Required]
    public List<ImageInfo> Images { get; set; } = new();

    public static void ToFile(string path, MatrixJsonSchema jsonSchema)
    {
        File.WriteAllText(path, ToJson(jsonSchema));
    }

    private static string ToJson(MatrixJsonSchema jsonSchema)
    {
        return JsonConvert.SerializeObject(jsonSchema, Settings);
    }

    public static void GenerateSchema(string path)
    {
        var generator = new JSchemaGenerator
        {
            DefaultRequired = Required.Default
        };
        var schema = generator.Generate(typeof(MatrixJsonSchema));
        schema.Title = "CreateMatrix Matrix Schema";
        schema.SchemaVersion = new Uri("https://json-schema.org/draft-06/schema");
        schema.Id = new Uri(SchemaUri);
        File.WriteAllText(path, schema.ToString());
    }
}