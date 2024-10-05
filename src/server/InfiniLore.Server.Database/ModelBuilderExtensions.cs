// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using Microsoft.EntityFrameworkCore.Query;
// using System.Linq.Expressions;
// using System.Reflection;
//
// namespace InfiniLore.Server.Database;
//
//
// /// <summary>
// ///     Dark magic, refers to https://gist.github.com/haacked/febe9e88354fb2f4a4eb11ba88d64c24
// ///     Made by IamEkoh
// /// </summary>
// public static class ModelBuilderExtensions
// {
//     private static readonly MethodInfo ConvertAndAppendQueryFilterMethod = typeof(ModelBuilderExtensions)
//         .GetMethod(nameof(ConvertAndAppendQueryFilter), BindingFlags.NonPublic | BindingFlags.Static)!;
//
//     public static void AddQueryFilterOnAllEntities<TBaseEntity>(
//         this ModelBuilder builder,
//         Expression<Func<TBaseEntity, bool>> filterExpression)
//     {
//         IEnumerable<Type> entityTypes = builder.Model.GetEntityTypes()
//             .Where(type => type.BaseType is null)
//             .Select(type => type.ClrType)
//             .Where(type => typeof(TBaseEntity).IsAssignableFrom(type));
//
//         foreach (Type entityType in entityTypes) {
//             builder.AppendQueryFilter(entityType, filterExpression);
//         }
//     }
//
//     public static void AddQueryFilter<TEntity>(
//         this EntityTypeBuilder<TEntity> entityTypeBuilder,
//         Expression<Func<TEntity, bool>> expression)
//         where TEntity : class
//     {
//         entityTypeBuilder.AppendQueryFilter(expression);
//     }
//
//     private static void AppendQueryFilter<TBaseEntity>(
//         this ModelBuilder builder,
//         Type entityType,
//         Expression<Func<TBaseEntity, bool>> filterExpression)
//     {
//         ConvertAndAppendQueryFilterMethod
//             .MakeGenericMethod(typeof(TBaseEntity), entityType)
//             .Invoke(null, [builder, filterExpression]);
//     }
//
//     private static void ConvertAndAppendQueryFilter<TBaseEntity, TEntity>(
//         this ModelBuilder builder,
//         Expression<Func<TBaseEntity, bool>> filterExpression)
//         where TBaseEntity : class
//         where TEntity : class, TBaseEntity
//     {
//         var concreteExpression = filterExpression.Convert<TBaseEntity, TEntity>();
//
//         builder.Entity<TEntity>().AppendQueryFilter(concreteExpression);
//     }
//
//     private static void AppendQueryFilter<TEntity>(
//         this EntityTypeBuilder entityTypeBuilder,
//         Expression<Func<TEntity, bool>> expression)
//         where TEntity : class
//     {
//         ParameterExpression parameterType = Expression.Parameter(entityTypeBuilder.Metadata.ClrType);
//
//         Expression expressionFilter = ReplacingExpressionVisitor.Replace(
//             expression.Parameters.Single(),
//             parameterType,
//             expression.Body);
//
//         if (entityTypeBuilder.Metadata.GetQueryFilter() is not null)
//         {
//             LambdaExpression currentQueryFilter = entityTypeBuilder.Metadata.GetQueryFilter()!;
//
//             Expression currentExpressionFilter = ReplacingExpressionVisitor.Replace(
//                 currentQueryFilter.Parameters.Single(),
//                 parameterType,
//                 currentQueryFilter.Body);
//
//             expressionFilter = Expression.AndAlso(currentExpressionFilter, expressionFilter);
//         }
//
//         LambdaExpression lambdaExpression = Expression.Lambda(expressionFilter, parameterType);
//         entityTypeBuilder.HasQueryFilter(lambdaExpression);
//     }
// }
//
// public static class ExpressionExtensions { public static Expression<Func<TTarget, bool>> Convert<TSource, TTarget>(this Expression<Func<TSource, bool>> root) { var visitor = new ParameterTypeVisitor<TSource, TTarget>(); return (Expression<Func<TTarget, bool>>)visitor.Visit(root); } }