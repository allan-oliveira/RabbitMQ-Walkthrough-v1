﻿
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQWalkthrough.Core.Architecture;
using RabbitMQWalkthrough.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQWalkthrough.Core.Queue
{
    public class Publisher
    {
        private readonly IModel model;
        private readonly string exchange;
        private CancellationTokenSource cancellationToken = new CancellationTokenSource();

        public Publisher(IModel model, string exchange)
        {
            this.model = model;
            this.exchange = exchange;

        }

        public Publisher Start()
        {
            Task.Run(() =>
            {
                while (!cancellationToken.Token.IsCancellationRequested)
                {
                    TimeSpan.FromMilliseconds(500).Wait();

                    var message = new Message()
                    {
                        Created = DateTime.Now
                    };

                    model.ReliablePublish(message, this.exchange, string.Empty);

                }

            }, cancellationToken.Token);
            return this;
        }

        public Publisher Stop()
        {
            this.cancellationToken.Cancel();
            return this;
        }

    }
}