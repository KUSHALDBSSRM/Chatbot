using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ChatBot_Project.Dialogs
{
    [Serializable]
    public class PromptDemo : IDialog<object>
    {
        private string name;
        private long age;
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Thanks for using Bot Application <br/> Fill the form below <br/>");
            context.Wait(GetNameAsync);
        }
        private Task GetNameAsync(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            PromptDialog.Text(
                context: context,
                resume: ResumeGetName,
                prompt: "Please enter your name",
                retry: "Sorry I didnot understand that. Please try again."

                );
            return Task.CompletedTask;
        }
        private  async Task ResumeGetName(IDialogContext context, IAwaitable<string> result)
        {
            name = await result;
            PromptDialog.Number(
                context: context,
                resume: ResumeGetAge,
                prompt: $"{name},Please enter your age",
                retry: "Sorry I didnot understand that. Please try again.",
                attempts: 3,
                min: 18,
                max: 50
                );
            
        }
        private async Task ResumeGetAge(IDialogContext context, IAwaitable<long> result)
        {
            age = await result;
            PromptDialog.Confirm(
                context: context,
                resume: ResumeConfirm,
                prompt: $"Your name is *{name}*, and age is *{age}* Right?",
                retry: "Sorry I didnot understand that. Please try again.",
                options: new string[] {"Yeah","Nope"},
                promptStyle: PromptStyle.PerLine

                );

        }

        private async Task ResumeConfirm(IDialogContext context, IAwaitable<bool> result)
        {
            if(await result)
            {
                await context.PostAsync($"You are registered successfully.<br/> **{name}**,**{age}**");

            }
            else
            {
                await context.PostAsync("I have doubt");
                context.Done(string.Empty);
            }
        }
    }
}