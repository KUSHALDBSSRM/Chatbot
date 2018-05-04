using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace ChatBot_Project.Dialogs
{
    [Serializable]
    public class HeroCardDemo : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = context.MakeMessage();
            var activity = await result;
            message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            message.Attachments.Add(GetCard(activity.Text));
            message.Attachments.Add(GetCard("OneMoreCard"));
            message.Attachments.Add(GetCard("LastCard"));
            await context.PostAsync(message);
        }

        private Attachment GetCard(string title)
        {
            string imageUrl = $"https://dummyimage.com/600x300/f0dff0/232540.jpg&text={title}";
            string docsUrl = "http://docs.microsoft.com/";
            var heroCard = new ThumbnailCard()  // new HeroCard() // for herocard
            {
                Title = title,
                Subtitle = "SubTitle for the card",
                Text = "Description",
                Images = new List<CardImage>
                {
                    new CardImage(imageUrl)
                },
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.OpenUrl,"Open Docs", value: docsUrl),
                    new CardAction(ActionTypes.OpenUrl,"View Image", value: imageUrl)
                }
            };
            return heroCard.ToAttachment();
        }
    }
}