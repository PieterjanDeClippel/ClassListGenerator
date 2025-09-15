using MintPlayer.ValueComparerGenerator.Attributes;

namespace ClassListGenerator.Models;

[AutoValueComparer]
public partial class ClassDeclaration
{
    public string? Name { get; set; }
}
