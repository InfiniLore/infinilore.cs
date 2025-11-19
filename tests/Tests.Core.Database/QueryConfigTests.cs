// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;

namespace Tests.Core.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class QueryConfigTests {
    [Test]
    [Arguments(QueryConfig.None, QueryConfig.None, false)] // None is the default value and thus should never match
    [Arguments(QueryConfig.None, QueryConfig.IncludeSoftDeleted, false)]
    
    [Arguments(QueryConfig.Reversed, QueryConfig.Reversed, true)]
    [Arguments(QueryConfig.Reversed, QueryConfig.None, false)]
    
    [Arguments(QueryConfig.IncludeSoftDeleted, QueryConfig.IncludeSoftDeleted, true)]
    [Arguments(QueryConfig.IncludeSoftDeleted, QueryConfig.None, false)]
    
    [Arguments(QueryConfig.IncludeOptionalReferences, QueryConfig.IncludeOptionalReferences, true)]
    [Arguments(QueryConfig.IncludeOptionalReferences, QueryConfig.None, false)]
    
    [Arguments(QueryConfig.SortByCreatedAt, QueryConfig.SortByCreatedAt, true)]
    [Arguments(QueryConfig.SortByCreatedAt, QueryConfig.None, false)]
    
    [Arguments(QueryConfig.SortByModifiedAt, QueryConfig.SortByModifiedAt, true)]
    [Arguments(QueryConfig.SortByModifiedAt, QueryConfig.None, false)]
    public async Task HasFlagFast(QueryConfig input, QueryConfig flag, bool expected ) {
        // Arrange
        
        // Act
        bool result = input.HasFlagFast(flag);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }
}
