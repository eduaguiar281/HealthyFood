﻿using HowToDevelop.Core.Comunicacao.Interfaces;
using MediatR;
using System;

namespace HowToDevelop.Core.Comunicacao
{
    public abstract class Evento: Mensagem, IEvento
    {
        public DateTime Timestamp { get; private set; }

        protected Evento()
        {
            Timestamp = DateTime.UtcNow;
        }

    }
}
