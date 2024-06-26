﻿// <copyright file="NmeaReceiver.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Runtime.CompilerServices;

namespace Ais.Receiver.Receiver
{
    public class FileStreamNmeaReceiver : INmeaReceiver
    {
        private readonly string path;
        private readonly TimeSpan delay = TimeSpan.Zero;

        public FileStreamNmeaReceiver(string path)
        {
            this.path = path;
        }
        
        public FileStreamNmeaReceiver(string path, TimeSpan delay)
        {
            this.path = path;
            this.delay = delay;
        }

        public async IAsyncEnumerable<string> GetAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            using var sr = new StreamReader(this.path);

            while (sr.Peek() >= 0)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                if (this.delay > TimeSpan.Zero)
                {
                    await Task.Delay(this.delay, cancellationToken).ConfigureAwait(false);
                }

                string? line = await sr.ReadLineAsync().ConfigureAwait(false);

                if (line is not null) { yield return line; }
            }
        }
    }
}