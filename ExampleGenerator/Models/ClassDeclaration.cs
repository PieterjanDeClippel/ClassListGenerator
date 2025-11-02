using MintPlayer.ValueComparerGenerator.Attributes;

namespace ExampleGenerator.Models;

[AutoValueComparer]
public partial class ClassDeclaration
{
    public string? Name { get; set; }
}
