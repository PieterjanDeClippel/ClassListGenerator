using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MintPlayer.SourceGenerators.Tools;

namespace ClassListGenerator.Generators
{
    [Generator(LanguageNames.CSharp)]
    public class ClassNamesSourceGenerator : IncrementalGenerator
    {
        public override void RegisterComparers()
        {
            // NewtonsoftJsonComparers.Register();
        }

        public override void Initialize(IncrementalGeneratorInitializationContext context, IncrementalValueProvider<Settings> settingsProvider)
        {
            var classDeclarationsProvider = context.SyntaxProvider
                .CreateSyntaxProvider(
                    static (node, ct) =>
                    {
                        return node is ClassDeclarationSyntax { } classDeclaration;
                    },
                    static (context, ct) =>
                    {
                        if (context.Node is ClassDeclarationSyntax classDeclaration &&
                            context.SemanticModel.GetDeclaredSymbol(classDeclaration, ct) is INamedTypeSymbol symbol)
                        {
                            return new Models.ClassDeclaration
                            {
                                Name = symbol.Name,
                            };
                        }
                        else
                        {
                            return default;
                        }
                    }
                )
                .WithNullableComparer()
                .Collect();

            var classNamesSourceProvider = classDeclarationsProvider
                .Join(settingsProvider)
                .Select(static Producer (p, ct) => new ClassNamesProducer(declarations: p.Item1.NotNull(), rootNamespace: p.Item2.RootNamespace!));

            var classNameListSourceProvider = classDeclarationsProvider
                .Join(settingsProvider)
                .Select(static Producer (p, ct) => new ClassNameListProducer(declarations: p.Item1.NotNull(), rootNamespace: p.Item2.RootNamespace!));

            // Combine all Source Providers
            context.ProduceCode(classNamesSourceProvider, classNameListSourceProvider);
        }
    }
}
