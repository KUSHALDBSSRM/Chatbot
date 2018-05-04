using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace ChatBot_Project.Dialogs
{
    public enum LaptopBrand
    {
        HP,Dell,Lenovo,Acer,Microsoft
    }
    public enum LaptopType
    {
        Laptop, Gaming, Ultrabook, Netbook
    }
    public enum LaptopProcessor
    {
        [Describe(description:"IntelCoreI3")]
        IntelCoreI3, IntelCoreI5, IntelCoreI7, IntelCoreI9, IntelCoreM
    }
    public enum LaptopOperatingSystem
    {
        Windows8 ,Windows10, MSDos, Linux
    }
    [Serializable]
    public class FormFlowDemo
    {
        public LaptopType? LaptopType;
        [Optional]
        [Describe(description: "Company", title: "Laptop Brand", subTitle: "There are several other brands present but we are not selling those")]
        public LaptopBrand? Brand;
        public LaptopOperatingSystem? OperatingSystem;
        public LaptopProcessor? Processor;
        public bool? RequiresTouch;
        [Numeric(2, 12)]
        [Describe(description: "Minimum Capacity of RAM")]
        [Template(TemplateUsage.NotUnderstood, "Unable to Understand")]
        public int? MinimumRamSize;
        [Pattern(@"^[789]\d{9}$")]
        public string Usermobileno;
        
        public static IForm<FormFlowDemo> GetForm()
        {
            OnCompletionAsyncDelegate<FormFlowDemo> onFormCompletion = async (context, state) =>
            {
                await context.PostAsync(@"We have noted the configurations that you require. We will inform you once we get it.");
            };
            return new FormBuilder<FormFlowDemo>()
                .Message("Welcome to Laptop Suggestion Bot")
                .Field(nameof(Processor))
                .Confirm(async(state) => {
                    int price = 0;
                    switch(state.Processor)
                    {
                        case LaptopProcessor.IntelCoreI3: price = 200; break;
                        case LaptopProcessor.IntelCoreI5: price = 300; break;
                        case LaptopProcessor.IntelCoreI7: price = 400; break;
                        case LaptopProcessor.IntelCoreI9: price = 500; break;
                        case LaptopProcessor.IntelCoreM: price  = 250; break;
                    }
                    return new PromptAttribute($"Minimum price for this processor will be {price}. Is this okay ?");
                } )
                .Field(nameof(Usermobileno),
                validate: async (state, response) =>
                {
                    var validation = new ValidateResult { IsValid = true, Value = response };
                    if((response as string).Equals("9933910176"))
                    {
                        validation.IsValid = false;
                        validation.Feedback = "9933910176 is not allowed";
                    }
                    return validation;
                } )
                .Confirm("You require Laptop with {Processor} and your mobile number is {Usermobileno}")
                .OnCompletion(onFormCompletion)
                .Build();
        }
    }
}