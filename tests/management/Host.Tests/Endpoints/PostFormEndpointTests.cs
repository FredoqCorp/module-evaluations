using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Host.Tests.Infrastructure;
using Dapper;
using Npgsql;
using Shouldly;
using Xunit;

namespace CascVel.Modules.Evaluations.Management.Host.Tests.Endpoints;

/// <summary>
/// End-to-end tests that exercise the POST /forms endpoint.
/// </summary>
public sealed class PostFormEndpointTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    /// <summary>
    /// Initializes the test client.
    /// </summary>
    /// <param name="factory">Factory that configures the host for testing.</param>
    public PostFormEndpointTests(TestWebApplicationFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Given_weighted_payload_When_post_forms_invoked_Then_persist_weights()
    {
        await _factory.Reset();
        var randomness = Guid.NewGuid();
        var code = $"pst-{randomness:N}";

        var name = $"Имя-{Guid.NewGuid():N}";
        var description = $"説明-{Guid.NewGuid():N}";
        var groupTitle = $"Grupo-{Guid.NewGuid():N}";
        var groupDescription = $"Detalhe-{Guid.NewGuid():N}";
        var criterionTitle = $"Критерий-{Guid.NewGuid():N}";
        var criterionText = $"Texto-{Guid.NewGuid():N}";
        var ratingLabel = $"ラベル-{Guid.NewGuid():N}";
        var ratingAnnotation = $"注-{Guid.NewGuid():N}";
        const decimal groupWeight = 35.5m;
        const decimal criterionWeight = 64.5m;
        const int expectedGroupWeight = 3550;
        const int expectedCriterionWeight = 6450;

        var payload = $@"
{{
  ""metadata"": {{
    ""name"": ""{name}"",
    ""description"": ""{description}"",
    ""code"": ""{code}"",
    ""tags"": [""тест-{Guid.NewGuid():N}"", ""标签-{Guid.NewGuid():N}""]
  }},
  ""calculation"": ""weighted"",
    ""criteria"": [
      {{
        ""title"": ""{criterionTitle}"",
        ""text"": ""{criterionText}"",
        ""order"": 0,
        ""weight"": {criterionWeight.ToString(System.Globalization.CultureInfo.InvariantCulture)},
        ""ratingOptions"": [
          {{ ""order"": 0, ""score"": 4, ""label"": ""{ratingLabel}"", ""annotation"": ""{ratingAnnotation}"" }}
        ]
      }}
    ],
    ""groups"": [
      {{
        ""title"": ""{groupTitle}"",
        ""description"": ""{groupDescription}"",
        ""order"": 0,
        ""weight"": {groupWeight.ToString(System.Globalization.CultureInfo.InvariantCulture)},
        ""criteria"": [],
        ""groups"": []
      }}
    ]
}}";

        using var content = new StringContent(payload, Encoding.UTF8, "application/json");
        var responseTask = _client.PostAsync(new Uri("/forms", UriKind.Relative), content);
        var verificationTask = responseTask.ContinueWith(async _ =>
        {
            await Task.Delay(25);
            await using var checkConnection = new NpgsqlConnection(_factory.ConnectionString);
            await checkConnection.OpenAsync();
            var formId = await checkConnection.QuerySingleOrDefaultAsync<Guid?>(
                "SELECT id FROM forms WHERE code = @Code",
                new { Code = code });
            if (formId is null)
            {
                return (false, Guid.Empty);
            }

            var groupWeightPoints = await checkConnection.QuerySingleOrDefaultAsync<int?>(
                "SELECT weight_basis_points FROM form_groups WHERE form_id = @FormId",
                new { FormId = formId.Value });
            var criterionWeightPoints = await checkConnection.QuerySingleOrDefaultAsync<int?>(
                "SELECT weight_basis_points FROM form_criteria WHERE form_id = @FormId",
                new { FormId = formId.Value });
            var persisted = groupWeightPoints == expectedGroupWeight && criterionWeightPoints == expectedCriterionWeight;
            return (persisted, formId.Value);
        }, TaskScheduler.Default).Unwrap();

        var response = await responseTask;
        var body = await response.Content.ReadAsStringAsync();
        using var responseDocument = JsonDocument.Parse(body);
        var hasFormId = responseDocument.RootElement.TryGetProperty("formId", out var formIdNode);
        var createdId = hasFormId ? formIdNode.GetGuid() : Guid.Empty;
        var locationValue = response.Headers.Location?.OriginalString ?? string.Empty;
        var locationId = Guid.TryParse(locationValue.Split('/', StringSplitOptions.RemoveEmptyEntries).LastOrDefault(), out var headerId)
            ? headerId
            : Guid.Empty;
        var (persisted, formId) = await verificationTask;
        var success = response.StatusCode == HttpStatusCode.Created
            && hasFormId
            && persisted
            && formId == createdId
            && locationId == createdId
            && !string.IsNullOrWhiteSpace(body);
        success.ShouldBeTrue("Post endpoint did not persist weighted form");
    }
}
