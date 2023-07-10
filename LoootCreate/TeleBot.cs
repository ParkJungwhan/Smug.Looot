using System;
using System.Threading;
using System.Threading.Tasks;
using LoootCreate.Logic;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace LoootCreate
{
    public class TeleBot
    {
        private TelegramBotClient botClient = new TelegramBotClient("723715956:AAFY3uuQgOECkz_VdSLGhHvFH9erdr4LTEw");
        private LottoMaker Lotto;

        public async void Init()
        {
            Lotto = new LottoMaker();
            var me = await botClient.GetMeAsync();
            Console.Title = me.Username;

            using var cts = new CancellationTokenSource();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            botClient.StartReceiving(
                new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync),
                cts.Token);

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is Message message)
            {
                Console.WriteLine($"Recv Chat : {message.Text}");

                if (message.Text == "/lotto")
                {
                    // make new lotto number
                    await botClient.SendTextMessageAsync(message.Chat, "새로운 로또 번호를 추출합니다(시간이 걸릴수 있습니다).");

                    // 여기서 로또 생성 로직 호출
                    for (int i = 0; i < 5; i++)
                    {
                        var result = Lotto.Make();
                        result.Sort();

                        string strMsg = string.Empty;
                        foreach (var seq in result)
                            strMsg += $",{seq}";

                        await botClient.SendTextMessageAsync(message.Chat, strMsg.Substring(1));
                    }
                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat, "존재하지 않는 메뉴입니다.");
                }

                await botClient.SendTextMessageAsync(message.Chat, "전송완료");
            }
        }

        private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is ApiRequestException apiRequestException)
            {
                await botClient.SendTextMessageAsync(123, apiRequestException.ToString());
            }
        }
    }
}