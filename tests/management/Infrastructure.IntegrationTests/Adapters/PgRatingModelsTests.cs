using System.Security.Cryptography;
using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests.TestDoubles;
using CascVel.Modules.Evaluations.Management.Infrastructure.Models;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests.Adapters;

/// <summary>
/// Unit tests that verify the PostgreSQL rating option helpers.
/// </summary>
public sealed class PgRatingModelsTests
{
    /// <summary>
    /// Ensures that PgRatingScore reads numeric values from JSON nodes.
    /// </summary>
    [Fact]
    public void PgRatingScore_reads_numeric_value_from_json_node()
    {
        var number = RandomNumberGenerator.GetInt32(1, 100);
        using var document = JsonDocument.Parse($@"{{""score"":{number}}}");
        var score = new PgRatingScore(document.RootElement);

        var actual = score.Value();

        Assert.Equal(number, actual);
    }

    /// <summary>
    /// Confirms that PgRatingLabel returns Unicode text from the rating node.
    /// </summary>
    [Fact]
    public void PgRatingLabel_returns_unicode_label_from_json_node()
    {
        var text = $"Лейбл-{Guid.NewGuid():N}";
        using var document = JsonDocument.Parse($@"{{""label"":""{text}""}}");
        var label = new PgRatingLabel(document.RootElement);

        var actual = label.Text();

        Assert.Equal(text, actual);
    }

    /// <summary>
    /// Confirms that PgRatingAnnotation returns the raw annotation text when present.
    /// </summary>
    [Fact]
    public void PgRatingAnnotation_returns_annotation_text_when_present()
    {
        var annotation = $"Описание-{Guid.NewGuid():N}";
        using var document = JsonDocument.Parse($@"{{""annotation"":""{annotation}""}}");
        var target = new PgRatingAnnotation(document.RootElement);

        var actual = target.Text();

        Assert.Equal(annotation, actual);
    }

    /// <summary>
    /// Ensures that PgRatingOption writes score, label, and annotation into media.
    /// </summary>
    [Fact]
    public void PgRatingOption_writes_components_into_media()
    {
        var score = RandomNumberGenerator.GetInt32(1, 6);
        var label = $"标签-{Guid.NewGuid():N}";
        var annotation = $"注解-{Guid.NewGuid():N}";
        using var document = JsonDocument.Parse($@"{{""score"":{score},""label"":""{label}"",""annotation"":""{annotation}""}}");
        var option = new PgRatingOption(document.RootElement);
        using var media = new FakeMedia();

        option.Print(media);

        var actual = (List<(string Key, object Value)>)media.Output();
        var expected = new List<(string, object)>
        {
            ("score", score),
            ("label", label),
            ("annotation", annotation)
        };

        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Validates that PgJsonRatingOptions orders rating options by payload keys before printing.
    /// </summary>
    [Fact]
    public void PgJsonRatingOptions_orders_options_by_key_before_printing()
    {
        var firstScore = RandomNumberGenerator.GetInt32(1, 6);
        var secondScore = RandomNumberGenerator.GetInt32(1, 6);
        var firstLabel = $"Лейбл-{Guid.NewGuid():N}";
        var secondLabel = $"标记-{Guid.NewGuid():N}";
        var payload = $@"{{""1"":{{""score"":{secondScore},""label"":""{secondLabel}"",""annotation"":""""}},""0"":{{""score"":{firstScore},""label"":""{firstLabel}"",""annotation"":""""}}}}";
        var options = new PgJsonRatingOptions(payload);
        using var media = new FakeMedia();

        options.Print(media);

        var array = media.GetValue<List<object>>("ratingOptions");
        var typed = array!.Cast<List<(string Key, object Value)>>().ToArray();
        var actualScores = typed.Select(list => (int)list.Single(item => item.Key == "score").Value).ToArray();

        Assert.Equal(new[] { firstScore, secondScore }, actualScores);
    }
}
