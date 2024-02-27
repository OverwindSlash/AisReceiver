// <copyright file="IAisPositionFixType.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using Ais.Net;

namespace Ais.Models.Abstractions
{
    public interface IAisPositionFixType
    {
        EpfdFixType PositionFixType { get; }
    }
}