using Xunit;

namespace Terminal.Forms.Tests;

public class SanityTests
{
    [Fact]
    public void ApplicationContext_DefaultConstructor_MainFormIsNull()
    {
        var ctx = new ApplicationContext();
        Assert.Null(ctx.MainForm);
    }

    [Fact]
    public void ApplicationContext_Tag_RoundTrips()
    {
        var ctx = new ApplicationContext();
        var tag = new object();
        ctx.Tag = tag;
        Assert.Same(tag, ctx.Tag);
    }
}
