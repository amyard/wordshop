using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WordShop.Data.Interfaces;
using WordShop.Enums;
using WordShop.Models.CustomerInfo;
using WordShop.Models.Settings;

namespace WordShop.Data.Repositories
{
    public class TelegramRepository : ITelegramRepository
    {
        private readonly IOptions<TelegramSettingsModel> _telegramSettings;
        private static ITelegramBotClient botClient;
        private const string noData = "Номер не был укащан";

        public TelegramRepository(IOptions<TelegramSettingsModel> telegramSettings)
        {
            _telegramSettings = telegramSettings;
        }   
        
        public async Task SendNewCustomerMessageToGroup(CustomerInfo customerInfo, TelegramMessageTypes telegramMessageTypes)
        {
            botClient = new TelegramBotClient(_telegramSettings.Value.ApiToken) {Timeout = TimeSpan.FromSeconds(10)};

            (string template, string image) = GetTelegramTemplate(telegramMessageTypes);
            string message = GetMessageByTemplate(customerInfo, telegramMessageTypes, template);

            //long[] senderIDs = { _telegramSettings.Value.AnnaChatId, _telegramSettings.Value.DevChatId, _telegramSettings.Value.ChatIdGroup };
            long[] devIDs = { _telegramSettings.Value.DevChatId };
            
            foreach (var chatId in devIDs)
            {
                await botClient.SendPhotoAsync(
                    chatId: chatId,
                    photo: image,
                    caption: message,
                    parseMode: ParseMode.Html
                );
            }
        }

        private string GetMessageByTemplate(CustomerInfo customerInfo, TelegramMessageTypes telegramMessageTypes, string template)
        {
            string message = telegramMessageTypes switch
            {
                TelegramMessageTypes.NewCustomer => String.Format(
                    template, 
                    customerInfo.FullName, 
                    customerInfo.Id, 
                    customerInfo.Email, 
                    customerInfo.PhoneNumber.Length > 0 ? customerInfo.PhoneNumber : noData, 
                    customerInfo.Courses, 
                    customerInfo.Tariff.Name, 
                    customerInfo.Tariff.NewPrice
                ),
                TelegramMessageTypes.PaymentSuccess => String.Format(
                    template, 
                    customerInfo.FullName, 
                    customerInfo.Id, 
                    customerInfo.Tariff.Name, 
                    customerInfo.Tariff.NewPrice,
                    customerInfo.OrderId.ToString()
                ),
                TelegramMessageTypes.SomeError => String.Format(template, null),
                _ => String.Format(template, null)
                
            };

            return message;
        }

        private (string template, string image) GetTelegramTemplate(TelegramMessageTypes telegramMessageTypes)
        {
            var newCustomer = "https://artmuz.com.ua/wp-content/uploads/2015/10/salut.jpg";
            var paymentSuccess = "https://www.ft.com/__origami/service/image/v2/images/raw/https%3A%2F%2Fd1e00ek4ebabms.cloudfront.net%2Fproduction%2F6b7c5fa6-b824-4430-b544-2102ad555fd1.jpg?fit=scale-down&source=next&width=700";
            
            (string template, string image) = telegramMessageTypes switch
            {
                TelegramMessageTypes.NewCustomer => (NewCustomerTemplate(), newCustomer),
                TelegramMessageTypes.PaymentSuccess => (SuccessfulPaymentTemplate(), paymentSuccess),
                TelegramMessageTypes.SomeError => (UnexpectedErrorTemplate(), newCustomer),
                _ => (UnexpectedErrorTemplate(), newCustomer)
            };

            return (template, image);
        }

        private string UnexpectedErrorTemplate()
        {
            string template = "Some shit happens!!!";
            
            return template;
        }

        private string SuccessfulPaymentTemplate()
        {
            string template = "Подписчик <b>{0}</b> (ID: {1}) оплатил тариф <b>{2}</b> в размере <b>${3}</b>.\n" +
                              "Номер транзакции - <b>{4}</b>";

            return template;
        }

        private string NewCustomerTemplate()
        {
            string template = "<b>УРАААААА. Новый подписчик!!!!</b>\n" +
                              "<b>Имя</b> - <i>{0}</i> (ID: {1})\n" +
                              "<b>Емейл</b> - <i>{2}</i>\n" +
                              "<b>Телефон</b> - <i>{3}</i>\n" +
                              "<b>Курс</b> - <i>{4}. <b>Тариф</b> - {5}</i>. <b>Цена курса</b> - <i>${6}</i>";

            return template;
        }
    }
}