using Nop.Core.Domain.Messages;
using Nop.Plugin.Widgets.CustomerQuery.Domain;
using Nop.Services.Messages;

namespace Nop.Plugin.Widgets.CustomerQuery.Extensions;

public static class WorkflowMessageServiceExtensions
{
    public static async Task<int> SendCustomerQueryStoreOwnerNotificationMessageAsync(this IWorkflowMessageService workflowMessageService,
         MessageTemplate messageTemplate,
        EmailAccount emailAccount,
        CustomerQueryRecord query,
        string emailTo,
        int languageId,
        IList<Token> tokens = null)
    {
        return await workflowMessageService.SendNotificationAsync(messageTemplate, emailAccount,
            languageId,
            tokens,
            toEmailAddress: emailTo,
            toName: query.Name);
    }

    public static async Task<int> SendCustomerQueryCustomerNotificationMessageAsync(this IWorkflowMessageService workflowMessageService, MessageTemplate messageTemplate,
        EmailAccount emailAccount,
        CustomerQueryRecord query,
        int languageId,
        IList<Token> tokens = null)
    {
        return await workflowMessageService.SendNotificationAsync(messageTemplate, emailAccount,
            languageId,
            tokens,
            toEmailAddress: query.Email,
            toName: query.Name           
            );
    }
}