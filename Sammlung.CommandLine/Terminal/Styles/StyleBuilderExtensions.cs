namespace Sammlung.CommandLine.Terminal.Styles
{
    public static class StyleBuilderExtensions
    {
        public static IStyleBuilder Append(this IStyleBuilder builder, string text)
        {
            var styledText = builder.Factory.Create(text).Build();
            return builder.Append(styledText);
        }
    }
}