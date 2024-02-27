// <copyright file="IAisMessage.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Ais.Models.Abstractions
{
    public interface IAisMessage : IVesselIdentity, IAisMessageType
    {
        string Channel { get; }
        string OriginalMessage { get; }
    }
}