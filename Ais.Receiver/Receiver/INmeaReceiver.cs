// <copyright file="NmeaReceiver.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Receiver.Receiver
{
    public interface INmeaReceiver
    {
        IAsyncEnumerable<string> GetAsync(CancellationToken cancellationToken = default);
    }
}