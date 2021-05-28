using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Payments;
using WordShop.Data.Interfaces;
using WordShop.Enums;
using WordShop.Models;

namespace WordShop.Data.Repositories
{
    public class TelegramRepository : ITelegramRepository
    {
        // TODO ---> move private to appsettings
        private static ITelegramBotClient botClient;
        private const string apiToken = "1775052882:AAGxetPJvW9EmulVUxcrp_IzZHAnmsSnIPI";
        
        private const long chatIdGroup = -1001298396195;
        private const long devChatId = 826890977;
        private const long annaChatId = 403845820;

        private const string noData = "Номер не был укащан";
        
        public async Task SendNewCustomerMessageToGroup(CustomerInfo customerInfo)
        {
            botClient = new TelegramBotClient(apiToken) {Timeout = TimeSpan.FromSeconds(10)};
            //var me = botClient.GetMeAsync().Result;

            string template = GetTelegramMessageFromTemplate(TelegramMessageTypes.NewCustomer);
            
            string message = String.Format(
                template, 
                customerInfo.FullName, 
                customerInfo.Id, 
                customerInfo.Email, 
                customerInfo.PhoneNumber.Length > 0 ? customerInfo.PhoneNumber : noData, 
                customerInfo.Courses, 
                customerInfo.Tariff.Name, 
                customerInfo.Tariff.NewPrice
            );

            // long[] senderIDs = {annaChatId, devChatId, chatIdGroup};
            long[] devIDs = {devChatId};
            
            foreach (var chatId in devIDs)
            {
                await botClient.SendPhotoAsync(
                    chatId: chatId,
                    photo: "https://artmuz.com.ua/wp-content/uploads/2015/10/salut.jpg",
                    caption: message,
                    parseMode: ParseMode.Html
                );
            }
        }

        private string GetTelegramMessageFromTemplate(TelegramMessageTypes telegramMessageTypes)
        {
            string template = telegramMessageTypes switch
            {
                TelegramMessageTypes.NewCustomer => NewCustomerTemplate(),
                TelegramMessageTypes.PaymentSuccess => SuccessfulPaymentTemplate(),
                TelegramMessageTypes.SomeError => UnexpectedErrorTemplate(),
                _ => UnexpectedErrorTemplate()
            };

            return template;
        }

        private string UnexpectedErrorTemplate()
        {
            string template = "Some shit happens!!!";
            
            return template;
        }

        private string SuccessfulPaymentTemplate()
        {
            string template = "Подписчик <b>{0}</b> оплатил тариф <b>{1}</b> в размере <b>{2}</b>." +
                              "Номер транзакции - <b>{0}</b>";

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