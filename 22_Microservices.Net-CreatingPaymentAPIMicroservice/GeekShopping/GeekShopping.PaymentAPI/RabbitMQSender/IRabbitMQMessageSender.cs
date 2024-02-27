﻿using GeekShopping.MessageBus;

namespace GeekShopping.PaymentAPI.RabbitMQSender
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(BaseMessage message, string queueName);
    }
}