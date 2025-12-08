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
    [Arguments(QueryConfig.None, QueryConfig.WithSoftDeleted, false)]
    
    [Arguments(QueryConfig.Reversed, QueryConfig.Reversed, true)]
    [Arguments(QueryConfig.Reversed, QueryConfig.None, false)]
    
    [Arguments(QueryConfig.WithSoftDeleted, QueryConfig.WithSoftDeleted, true)]
    [Arguments(QueryConfig.WithSoftDeleted, QueryConfig.None, false)]
    
    [Arguments(QueryConfig.WithOptionalInclude, QueryConfig.WithOptionalInclude, true)]
    [Arguments(QueryConfig.WithOptionalInclude, QueryConfig.None, false)]
    
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
