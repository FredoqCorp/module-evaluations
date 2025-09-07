using CascVel.Modules.Evaluations.Management.Domain.Common;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Common;

/// <summary>
/// Unit tests for Option value object covering mapping, binding, reduction and factories.
/// </summary>
public sealed class OptionTests
{
    /// <summary>
    /// Verifies that Reduce returns the same string when value is present.
    /// </summary>
    [Fact(DisplayName = "Option returns the same string on reduce when present")]
    public void Option_returns_the_same_string_on_reduce_when_present()
    {
        var s = "тест-✓-" + Guid.NewGuid();
        var opt = Option.Of(s);
        opt.Reduce("fallback-✓-" + Guid.NewGuid()).ShouldBe(s, "Option returned an unexpected value which is incorrect");
    }

    /// <summary>
    /// Verifies that Reduce(value) returns the fallback when option is empty.
    /// </summary>
    [Fact(DisplayName = "Option returns fallback for reduce with value when empty")]
    public void Option_returns_fallback_for_reduce_with_value_when_empty()
    {
        var fb = "резерв-✓-" + Guid.NewGuid();
        var opt = Option.None<string>();
        opt.Reduce(fb).ShouldBe(fb, "Option ignored the fallback which is incorrect");
    }

    /// <summary>
    /// Verifies that Reduce(func) returns the factory value when option is empty.
    /// </summary>
    [Fact(DisplayName = "Option returns fallback for reduce with func when empty")]
    public void Option_returns_fallback_for_reduce_with_func_when_empty()
    {
        var fb = "значение-✓-" + Guid.NewGuid();
        var opt = Option.None<string>();
        opt.Reduce(() => fb).ShouldBe(fb, "Option ignored the factory which is incorrect");
    }

    /// <summary>
    /// Verifies that Of throws when content is null.
    /// </summary>
    [Fact(DisplayName = "Option cannot accept null content for Of")]
    public void Option_cannot_accept_null_content_for_Of()
    {
        Should.Throw<ArgumentException>(() => Option.Of<string>(null!), "Option accepted a null content which is incorrect");
    }

    /// <summary>
    /// Verifies that Map transforms the value when present.
    /// </summary>
    [Fact(DisplayName = "Option maps value when present")]
    public void Option_maps_value_when_present()
    {
        var s = "абв-✓-" + Guid.NewGuid();
        var opt = Option.Of(s).Map(x => x.Length);
        opt.Reduce(-1).ShouldBe(s.Length, "Option map produced an unexpected length which is incorrect");
    }

    /// <summary>
    /// Verifies that Map returns None when the option is empty.
    /// </summary>
    [Fact(DisplayName = "Option map returns none when empty")]
    public void Option_map_returns_none_when_empty()
    {
        var fb = "резерв-✓-" + Guid.NewGuid();
        var opt = Option.None<string>().Map(x => x + "-mapped-✓-");
        opt.Reduce(fb).ShouldBe(fb, "Option map returned a value which is incorrect");
    }

    /// <summary>
    /// Verifies that Bind flattens the value when present.
    /// </summary>
    [Fact(DisplayName = "Option bind flattens value when present")]
    public void Option_bind_flattens_value_when_present()
    {
        var s = "данные-✓-" + Guid.NewGuid();
        var opt = Option.Of(s).Bind(x => Option.Of(x.Length));
        opt.Reduce(-3).ShouldBe(s.Length, "Option bind produced an unexpected value which is incorrect");
    }

    /// <summary>
    /// Verifies that Bind returns None when the option is empty.
    /// </summary>
    [Fact(DisplayName = "Option bind returns none when empty")]
    public void Option_bind_returns_none_when_empty()
    {
        var fb = "fallback-✓-" + Guid.NewGuid();
        var opt = Option.None<string>().Bind(x => Option.Of(x + "-bound-✓-"));
        opt.Reduce(fb).ShouldBe(fb, "Option bind returned a value which is incorrect");
    }

    /// <summary>
    /// Verifies that ToString returns an empty string when option is empty.
    /// </summary>
    [Fact(DisplayName = "Option to string returns empty for none")]
    public void Option_to_string_returns_empty_for_none()
    {
        var opt = Option.None<string>();
        opt.ToString().ShouldBe(string.Empty, "Option to string returned a non empty string which is incorrect");
    }

    /// <summary>
    /// Verifies that ToString returns content string when present.
    /// </summary>
    [Fact(DisplayName = "Option to string returns content when present")]
    public void Option_to_string_returns_content_when_present()
    {
        var s = "строка-✓-" + Guid.NewGuid();
        var opt = Option.Of(s);
        opt.ToString().ShouldBe(s, "Option to string returned an unexpected string which is incorrect");
    }

    /// <summary>
    /// Verifies that Map throws when mapper function is null.
    /// </summary>
    [Fact(DisplayName = "Option map cannot accept null mapper")]
    public void Option_map_cannot_accept_null_mapper()
    {
        var opt = Option.Of("значение-✓-" + Guid.NewGuid());
        Should.Throw<ArgumentNullException>(() => opt.Map<string>(null!), "Option map accepted a null mapper which is incorrect");
    }

    /// <summary>
    /// Verifies that Bind throws when binder function is null.
    /// </summary>
    [Fact(DisplayName = "Option bind cannot accept null binder")]
    public void Option_bind_cannot_accept_null_binder()
    {
        var opt = Option.Of("значение-✓-" + Guid.NewGuid());
        Should.Throw<ArgumentNullException>(() => opt.Bind<string>(null!), "Option bind accepted a null binder which is incorrect");
    }

    /// <summary>
    /// Verifies that Reduce(func) throws when factory is null.
    /// </summary>
    [Fact(DisplayName = "Option reduce cannot accept null factory")]
    public void Option_reduce_cannot_accept_null_factory()
    {
        var opt = Option.None<string>();
        Should.Throw<ArgumentNullException>(() => opt.Reduce((Func<string>)null!), "Option reduce accepted a null factory which is incorrect");
    }
}
