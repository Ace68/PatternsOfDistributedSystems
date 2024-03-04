using NetArchTest.Rules;
using System.Diagnostics.CodeAnalysis;

namespace BrewUp.Sales.Architecture.Tests;

[ExcludeFromCodeCoverage]
public class SalesArchitectureTest
{
    [Fact]
    public void Should_SalesArchitecture_BeCompliant()
    {
        var types = Types.InAssembly(typeof(Program).Assembly);

        var forbiddenAssemblies = new List<string>
        {
            "BrewUp.Sales.Domain",
            "BrewUp.Sales.ReadModel",
            "BrewUp.Sales.SharedKernel"
        };

        var result = types
            .ShouldNot()
            .HaveDependencyOnAny(forbiddenAssemblies.ToArray())
            .GetResult()
            .IsSuccessful;

        Assert.True(result);
    }
}