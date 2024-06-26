﻿// <copyright file="IAisMessageType1to3.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Ais.Net;

namespace Ais.Models.Abstractions
{
    public interface IAisMessageType1to3
    {
        ManoeuvreIndicator ManoeuvreIndicator { get; }

        NavigationStatus NavigationStatus { get; }

        uint RadioSlotTimeout { get; }

        uint RadioSubMessage { get; }

        RadioSyncState RadioSyncState { get; }

        int RateOfTurn { get; }

        uint SpareBits145 { get; }
    }
}