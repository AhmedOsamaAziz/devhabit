#pragma warning disable S2326
namespace DevHabit.Api.Services.Sorting;

public sealed record SortMapping(string SortField, string PropertyName, bool Reverse = false);

public interface ISortMappingDefinition;

public sealed class SortMappingDefinition<TSource, TDestination> : ISortMappingDefinition
{
    public required SortMapping[] Mappings { get; init; }
}