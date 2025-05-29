// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.GeneratorTools;

namespace InfiniLore.Server.SourceGenerators.MessageBroker;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class MessageDtoWriter {
    public static void WriteBrokerExtension(GeneratorStringBuilder builder, MessageDto dto) {
        string returnType = dto.GetReturnType();
        string methodName = dto.MethodName;
        string requestType = dto.ClassNameFull;
        
        builder.AppendLine($"public static async {returnType} {methodName}(")
            .AppendLineIndented("this IMessageBroker broker,")
            .ForEachAppendLineIndented(dto.GetArgDefinitions());
        if (dto.IsEvent) builder.AppendLineIndented("FastEndpoints.Mode waitMode = Mode.WaitForAll,");
        builder.AppendLineIndented("CancellationToken ct = default")
            .AppendLine(") {");
        
        
        if (dto.IsEvent) {
            // ReSharper disable once HeapView.CanAvoidClosure
            builder.Indent(b => {
                b.AppendLine($"var eventMessage = new {requestType}({string.Join(", ", dto.GetArgNames())});");
                b.AppendLine("await eventMessage.PublishAsync(waitMode:waitMode, cancellation: ct).ConfigureAwait(false);");
                b.AppendLine("return;");
            });
        } else {
            // ReSharper disable once HeapView.CanAvoidClosure
            builder.Indent(b => {
                if (dto.HasPaginationInfoParameter) {
                    b.AppendLine($"if (paginationInfo == default({TypeNames.PaginationInfo})) paginationInfo = {TypeNames.PaginationInfo}.Default;");
                }
                
                b.AppendLine("IAccessingUser access = await broker.GetMessageAccessAsync(ct);");
                b.AppendLine($"var request = new {requestType}({string.Join(", ", dto.GetArgNames())}) {{ AccessingUser = access }};");
                b.AppendLine("return await request.ExecuteAsync(ct: ct);");
            });
        }
        
        builder.AppendLine("}");
        builder.AppendLine();
    }
}

